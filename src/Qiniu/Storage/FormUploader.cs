using Qiniu.Http;
using Qiniu.Util;

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HttpRequestOptions = Qiniu.Http.HttpRequestOptions;

namespace Qiniu.Storage
{
    /// <summary>
    /// 简单上传，适合于以下"情形1":  
    /// (1)网络较好并且待上传的文件体积较小时(比如100MB或更小一点)使用简单上传;
    /// (2)文件较大或者网络状况不理想时请使用分片上传;
    /// (3)文件较大并且需要支持断点续上传，请使用分片上传(断点续上传)
    /// 上传时需要提供正确的上传凭证(由对应的上传策略生成)
    /// 上传策略 http://developer.qiniu.com/article/developer/security/upload-token.html
    /// 上传凭证 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    public class FormUploader
    {
        private readonly Config _config;
        private readonly HttpManager _httpManager;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="config">表单上传的配置信息</param>
        public FormUploader(Config config)
        {
            this._config = config;
            this._httpManager = new HttpManager();
        }

        /// <summary>
        /// 上传文件 - 可附加自定义参数
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="key">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra? extra)
        {
            try
            {
                await using FileStream fs = new FileStream(localFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                return await this.UploadStreamAsync(fs, key, token, extra);
            }
            catch (Exception ex)
            {
                HttpResult ret = HttpResult.InvalidFile;
                ret.RefText = ex.Message;
                return ret;
            }
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="key">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传数据后的返回结果</returns>
        public async Task<HttpResult> UploadDataAsync(byte[] data, string key, string token, PutExtra extra)
        {
            using MemoryStream stream = new MemoryStream(data);
            return await this.UploadStreamAsync(stream, key, token, extra);
        }

        /// <summary>
        /// 上传数据流
        /// </summary>
        /// <param name="stream">(确定长度的)数据流</param>
        /// <param name="key">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <param name="putExtra">上传可选设置</param>
        /// <returns>上传数据流后的返回结果</returns>
        public async Task<HttpResult> UploadStreamAsync(Stream stream, string? key, string token, PutExtra? putExtra)
        {
            if (putExtra == null)
            {
                putExtra = new PutExtra();
                putExtra.MaxRetryTimes = _config.MaxRetryTimes;
            }
            if (string.IsNullOrEmpty(putExtra.MimeType))
            {
                putExtra.MimeType = "application/octet-stream";
            }
            if (putExtra.ProgressHandler == null)
            {
                putExtra.ProgressHandler = DefaultUploadProgressHandler;
            }
            if (putExtra.UploadController == null)
            {
                putExtra.UploadController = DefaultUploadController;
            }
            string? fileName = key;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "fname_temp";
            }

            HttpResult result = new HttpResult();

            try
            {
                var startPosition = stream.Position;
                Stream uploadStream = await ReadonlyWrapperStream.CreateWrapperStreamAsync(stream);
                var uploadedBytes = uploadStream.Length;

                string boundary = HttpManager.CreateFormDataBoundary();
                var multipartFormDataContent = new MultipartFormDataContent(boundary);

                StringBuilder bodyBuilder = new StringBuilder();
                bodyBuilder.AppendLine("--" + boundary);

                if (key != null)
                {
                    //write key when it is not null
                    multipartFormDataContent.Add(new StringContent(key), "key");
                }

                //write token
                multipartFormDataContent.Add(new StringContent(token), "token");

                //write extra params
                if (putExtra.Params != null && putExtra.Params.Count > 0)
                {
                    foreach (var p in putExtra.Params)
                    {
                        if (p.Key.StartsWith("x:"))
                        {
                            multipartFormDataContent.Add(new StringContent(p.Value), p.Key);
                        }
                    }
                }

                //prepare data buffer
                putExtra.ProgressHandler(0, uploadedBytes);

                //write crc32
                uint crc32 = await CRC32.CheckSumBytes(uploadStream);
                uploadStream.Position = startPosition;

                multipartFormDataContent.Add(new StringContent(crc32.ToString(CultureInfo.InvariantCulture)), "crc32");

                //write fname
                multipartFormDataContent.Add(new StreamContent(uploadStream)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(putExtra.MimeType) }
                }, "file", fileName);

                putExtra.ProgressHandler(uploadedBytes / 5, uploadedBytes);

                string? ak = UpToken.GetAccessKeyFromUpToken(token);
                string? bucket = UpToken.GetBucketFromUpToken(token);
                if (ak == null || bucket == null)
                {
                    return HttpResult.InvalidToken;
                }

                string uploadHost = this._config.UpHost(ak, bucket);
                HttpRequestOptions reqOpts = _httpManager.CreateHttpRequestOptions("POST", uploadHost, null, token);
                reqOpts.RequestContent = multipartFormDataContent;
                result = await _httpManager.SendRequestAsync(reqOpts);

                putExtra.ProgressHandler(uploadedBytes, uploadedBytes);
                if (result.Code == (int) HttpCode.OK)
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Uploaded: #STREAM# ==> \"{1}\"\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), key);
                }
                else
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), result.Code, result.Text);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                // 不要自己循环去获取内部异常，而是直接使用 ToString 方法输出，这样还能获取正确的堆栈信息
                //Exception e = ex;
                //while (e != null)
                //{
                //    sb.Append(e.Message + " ");
                //    e = e.InnerException;
                //}
                sb.AppendLine(ex.ToString());
                sb.AppendLine();

                if (ex is QiniuException)
                {
                    QiniuException qex = (QiniuException) ex;
                    result.Code = qex.HttpResult.Code;
                    result.RefCode = qex.HttpResult.Code;
                    result.Text = qex.HttpResult.Text;
                    result.RefText += sb.ToString();
                }
                else
                {
                    result.RefCode = (int) HttpCode.USER_UNDEF;
                    result.RefText += sb.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 默认的进度处理函数-上传文件
        /// </summary>
        /// <param name="uploadedBytes">已上传的字节数</param>
        /// <param name="totalBytes">文件总字节数</param>
        public static void DefaultUploadProgressHandler(long uploadedBytes, long totalBytes)
        {
            if (uploadedBytes < totalBytes)
            {
                Console.WriteLine("[{0}] [FormUpload] Progress: {1,7:0.000}%", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), 100.0 * uploadedBytes / totalBytes);
            }
            else
            {
                Console.WriteLine("[{0}] [FormUpload] Progress: {1,7:0.000}%\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), 100.0);
            }
        }

        /// <summary>
        /// 默认的上传控制函数，默认不执行任何控制
        /// </summary>
        /// <returns>控制状态</returns>
        public static UploadControllerAction DefaultUploadController()
        {
            return UploadControllerAction.Activated;
        }

        private HttpResult PostFormWithRetry(string token, byte[] data, string boundary, PutExtra putExtra)
        {
            //get upload host
            string? ak = UpToken.GetAccessKeyFromUpToken(token);
            string? bucket = UpToken.GetBucketFromUpToken(token);
            if (ak == null || bucket == null)
            {
                return HttpResult.InvalidToken;
            }

            string uploadHost = this._config.UpHost(ak, bucket);
            HttpResult result = _httpManager.PostMultipart(uploadHost, data, boundary, null);

            int retryTimes = 0;
            while (
                retryTimes < putExtra.MaxRetryTimes &&
                UploadUtil.ShouldRetry(result.Code, result.RefCode)
            )
            {
                result = _httpManager.PostMultipart(uploadHost, data, boundary, null);
                retryTimes += 1;
            }

            return result;
        }
    }

    class ReadonlyWrapperStream : Stream
    {
        public static async ValueTask<ReadonlyWrapperStream> CreateWrapperStreamAsync(Stream inputStream)
        {
            if (inputStream.CanSeek)
            {
                var wrapperStream = new ReadonlyWrapperStream(inputStream, leaveOpen: true);
                return wrapperStream;
            }
            else
            {
                var memoryStream = new MemoryStream();
                await inputStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var wrapperStream = new ReadonlyWrapperStream(memoryStream, leaveOpen: false);
                return wrapperStream;
            }
        }

        private ReadonlyWrapperStream(Stream innerStream, bool leaveOpen)
        {
            _innerStream = innerStream;
            _leaveOpen = leaveOpen;
        }

        private readonly Stream _innerStream;

        private readonly bool _leaveOpen;
        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            return _innerStream.Read(buffer);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _innerStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
        {
            return _innerStream.ReadAsync(buffer, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => false;

        public override long Length => _innerStream.Length;

        public override long Position
        {
            get => _innerStream.Position;
            set => _innerStream.Position = value;
        }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            _innerStream.CopyTo(destination, bufferSize);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return _innerStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_leaveOpen)
            {
                _innerStream.Dispose();
            }

            base.Dispose(disposing);
        }

        public override async ValueTask DisposeAsync()
        {
            if (!_leaveOpen)
            {
                await _innerStream.DisposeAsync();
            }

            await base.DisposeAsync();
        }
    }
}