using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.Storage
{
    /// <summary>
    ///     简单上传，适合于以下"情形1":
    ///     (1)网络较好并且待上传的文件体积较小时(比如100MB或更小一点)使用简单上传;
    ///     (2)文件较大或者网络状况不理想时请使用分片上传;
    ///     (3)文件较大并且需要支持断点续上传，请使用分片上传(断点续上传)
    ///     上传时需要提供正确的上传凭证(由对应的上传策略生成)
    ///     上传策略 http://developer.qiniu.com/article/developer/security/upload-token.html
    ///     上传凭证 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    public class FormUploader
    {
        private readonly Config _config;
        private readonly HttpManager _httpManager;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="config">表单上传的配置信息</param>
        public FormUploader(Config config)
        {
            _config = config;
            _httpManager = new HttpManager();
        }

        /// <summary>
        ///     上传文件 - 可附加自定义参数
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="key">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra extra)
        {
            try
            {
                var fs = new FileStream(localFile, FileMode.Open);
                return await UploadStream(fs, key, token, extra);
            }
            catch (Exception ex)
            {
                var ret = HttpResult.InvalidFile;
                ret.RefText = ex.Message;
                return ret;
            }
        }

        /// <summary>
        ///     上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="key">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传数据后的返回结果</returns>
        public Task<HttpResult> UploadData(byte[] data, string key, string token, PutExtra extra)
        {
            var stream = new MemoryStream(data);
            return UploadStream(stream, key, token, extra);
        }

        /// <summary>
        ///     上传数据流
        /// </summary>
        /// <param name="stream">(确定长度的)数据流</param>
        /// <param name="key">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <param name="putExtra">上传可选设置</param>
        /// <returns>上传数据流后的返回结果</returns>
        public async Task<HttpResult> UploadStream(Stream stream, string key, string token, PutExtra putExtra)
        {
            if (putExtra == null)
            {
                putExtra = new PutExtra();
            }

            if (string.IsNullOrEmpty(putExtra.MimeType))
            {
                putExtra.MimeType = ContentType.APPLICATION_OCTET_STREAM;
            }

            if (putExtra.ProgressHandler == null)
            {
                putExtra.ProgressHandler = DefaultUploadProgressHandler;
            }

            if (putExtra.UploadController == null)
            {
                putExtra.UploadController = DefaultUploadController;
            }

            var fileName = key;
            if (string.IsNullOrEmpty(key))
            {
                fileName = "fname_temp";
            }

            var result = new HttpResult();

            try
            {
                var boundary = HttpManager.CreateFormDataBoundary();
                var content = new MultipartFormDataContent(boundary);
                var length = stream.Length;
                putExtra.ProgressHandler(0, length);

                // Key
                if (!string.IsNullOrEmpty(key))
                {
                    content.Add(new StringContent(key), "key");
                }

                // Token
                content.Add(new StringContent(token), "token");

                // Other params
                if (putExtra.Params != null)
                {
                    foreach (var param in putExtra.Params)
                    {
                        content.Add(new StringContent(param.Value), param.Key);
                    }
                }

                // Reuse stream
                if (!stream.CanSeek)
                {
                    var ms = new MemoryStream((int)stream.Length);
                    stream.CopyTo(ms);
                    stream.Dispose();
                    stream = ms;
                }

                // CRC32
                var crc32 = Crc32.CheckSumStream(stream);
                stream.Seek(0, SeekOrigin.Begin);
                content.Add(new StringContent(crc32.ToString()), "crc32");

                // Primary content
                var part = new StreamContent(stream);
                part.Headers.ContentType = MediaTypeHeaderValue.Parse(putExtra.MimeType);
                content.Add(part, "file", fileName);

                // Get upload host
                var ak = UpToken.GetAccessKeyFromUpToken(token);
                var bucket = UpToken.GetBucketFromUpToken(token);
                if (ak == null || bucket == null)
                {
                    return HttpResult.InvalidToken;
                }

                var uploadHost = await _config.UpHost(ak, bucket);

                // TODO: Real progress
                putExtra.ProgressHandler(length / 5, length);
                result = await _httpManager.PostAsync(uploadHost, content, boundary);
                putExtra.ProgressHandler(length, length);
                if (result.Code == (int)HttpCode.OK)
                {
                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [FormUpload] Uploaded: #STREAM# ==> \"{key}\"\n";
                }
                else
                {
                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [FormUpload] Failed: code = {result.Code}, text = {result.Text}\n";
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [FormUpload] Error: ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                if (ex is QiniuException qex)
                {
                    result.Code = qex.HttpResult.Code;
                    result.RefCode = qex.HttpResult.Code;
                    result.Text = qex.HttpResult.Text;
                    result.RefText += sb.ToString();
                }
                else
                {
                    result.RefCode = (int)HttpCode.USER_UNDEF;
                    result.RefText += sb.ToString();
                }
            }

            return result;
        }

        /// <summary>
        ///     默认的进度处理函数-上传文件
        /// </summary>
        /// <param name="uploadedBytes">已上传的字节数</param>
        /// <param name="totalBytes">文件总字节数</param>
        public static void DefaultUploadProgressHandler(long uploadedBytes, long totalBytes)
        {
            if (uploadedBytes < totalBytes)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [FormUpload] Progress: {100.0 * uploadedBytes / totalBytes,7:0.000}%");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [FormUpload] Progress: {100.0,7:0.000}%\n");
            }
        }

        /// <summary>
        ///     默认的上传控制函数，默认不执行任何控制
        /// </summary>
        /// <returns>控制状态</returns>
        public static UploadControllerAction DefaultUploadController()
        {
            return UploadControllerAction.Activated;
        }
    }
}
