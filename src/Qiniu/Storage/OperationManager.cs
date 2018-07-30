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
    ///     数据处理
    /// </summary>
    public class OperationManager
    {
        private readonly Auth _auth;
        private readonly Config _config;
        private readonly HttpManager _httpManager;
        private readonly Mac _mac;

        /// <summary>
        ///     构建新的数据处理对象
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="config"></param>
        public OperationManager(Mac mac, Config config)
        {
            _mac = mac;
            _auth = new Auth(mac);
            _config = config;
            _httpManager = new HttpManager();
        }


        /// <summary>
        ///     数据处理
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns>pfop操作返回结果，正确返回结果包含persistentId</returns>
        public async Task<PfopResult> Pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force)
        {
            var result = new PfopResult();

            try
            {
                var host = await _config.ApiHost(_mac.AccessKey, bucket);
                var pfopUrl = $"{host}/pfop/";

                var sb = new StringBuilder();
                sb.Append($"bucket={StringHelper.UrlEncode(bucket)}&key={StringHelper.UrlEncode(key)}&fops={StringHelper.UrlEncode(fops)}");
                if (!string.IsNullOrEmpty(notifyUrl))
                {
                    sb.Append($"&notifyURL={StringHelper.UrlEncode(notifyUrl)}");
                }

                if (force)
                {
                    sb.Append("&force=1");
                }

                if (!string.IsNullOrEmpty(pipeline))
                {
                    sb.Append($"&pipeline={pipeline}");
                }

                var data = Encoding.UTF8.GetBytes(sb.ToString());
                var token = _auth.CreateManageToken(pfopUrl, data);

                var hr = await _httpManager.PostFormAsync(pfopUrl, data, token);
                result.Shadow(hr);
            }
            catch (QiniuException ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [pfop] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.Code = ex.HttpResult.Code;
                result.RefCode = ex.HttpResult.Code;
                result.Text = ex.HttpResult.Text;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     数据处理，操作字符串拼接后与另一种形式等价
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)列表</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns>操作返回结果，正确返回结果包含persistentId</returns>
        public Task<PfopResult> Pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force)
        {
            var ops = string.Join(";", fops);
            return Pfop(bucket, key, ops, pipeline, notifyUrl, force);
        }

        /// <summary>
        ///     查询pfop操作处理结果(或状态)
        /// </summary>
        /// <param name="persistentId">持久化ID</param>
        /// <returns>操作结果</returns>
        public async Task<PrefopResult> Prefop(string persistentId)
        {
            var result = new PrefopResult();

            var scheme = _config.UseHttps ? "https://" : "http://";
            var prefopUrl = $"{scheme}{Config.DefaultApiHost}/status/get/prefop?id={persistentId}";

            var httpMgr = new HttpManager();
            var httpResult = await httpMgr.GetAsync(prefopUrl);
            result.Shadow(httpResult);

            return result;
        }

        /// <summary>
        ///     根据uri的类型(网络url或者本地文件路径)自动选择dfop_url或者dfop_data
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="uri">资源/文件URI</param>
        /// <returns>操作结果/返回数据</returns>
        public Task<HttpResult> Dfop(string fop, string uri)
        {
            if (UrlHelper.IsValidUrl(uri))
            {
                return DfopUrl(fop, uri);
            }

            return DfopFile(fop, uri);
        }

        /// <summary>
        ///     文本处理(直接传入文本内容)
        /// </summary>
        /// <param name="fop">文本处理命令</param>
        /// <param name="text">文本内容</param>
        /// <returns></returns>
        public async Task<HttpResult> DfopText(string fop, string text)
        {
            var result = new HttpResult();

            try
            {
                var scheme = _config.UseHttps ? "https://" : "http://";
                var dfopUrl = $"{scheme}{Config.DefaultApiHost}/dfop?fop={fop}";
                var token = _auth.CreateManageToken(dfopUrl);
                var boundary = HttpManager.CreateFormDataBoundary();
                var part = new StringContent(text);
                part.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType.TEXT_PLAIN);

                var content = new MultipartFormDataContent(boundary)
                {
                    { part, "data", "text" }
                };

                result = await _httpManager.PostAsync(dfopUrl, content, token, true);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [dfop] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_UNDEF;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     文本处理(从文件读取文本)
        /// </summary>
        /// <param name="fop">文本处理命令</param>
        /// <param name="textFile">文本文件</param>
        /// <returns></returns>
        public Task<HttpResult> DfopTextFile(string fop, string textFile)
        {
            return DfopFile(fop, textFile, ContentType.TEXT_PLAIN);
        }

        [Obsolete("请使用DfopFile方法")]
        public Task<HttpResult> DfopData(string fop, string localFile)
        {
            return DfopFile(fop, localFile);
        }

        /// <summary>
        ///     如果uri是本地文件路径则使用此方法
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="localFile">文件名</param>
        /// <param name="mimeType">数据内容类型</param>
        /// <returns>处理结果</returns>
        public Task<HttpResult> DfopFile(string fop, string localFile, string mimeType = "application/octet-stream")
        {
            if (File.Exists(localFile))
            {
                return DfopStream(fop, new FileStream(localFile, FileMode.Open), mimeType, Path.GetFileName(localFile));
            }

            var result = new HttpResult
            {
                RefCode = (int)HttpCode.INVALID_FILE,
                RefText = $"[dfop-error] File not found: {localFile}"
            };
            return Task.FromResult(result);
        }

        /// <summary>
        ///     如果处理内容是流则使用此方法
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="stream">数据流</param>
        /// <param name="mimeType">数据内容类型</param>
        /// <param name="fileName">文件名</param>
        /// <returns>处理结果</returns>
        public async Task<HttpResult> DfopStream(string fop, Stream stream, string mimeType, string fileName)
        {
            var result = new HttpResult();

            try
            {
                var scheme = _config.UseHttps ? "https://" : "http://";
                var dfopUrl = $"{scheme}{Config.DefaultApiHost}/dfop?fop={fop}";
                var token = _auth.CreateManageToken(dfopUrl);
                var boundary = HttpManager.CreateFormDataBoundary();
                var part = new StreamContent(stream);
                part.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);

                var content = new MultipartFormDataContent(boundary)
                {
                    { part, "data", fileName }
                };

                result = await _httpManager.PostAsync(dfopUrl, content, token, true);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [dfop] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_UNDEF;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     如果uri是网络url则使用此方法
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="url">资源URL</param>
        /// <returns>处理结果</returns>
        public async Task<HttpResult> DfopUrl(string fop, string url)
        {
            var scheme = _config.UseHttps ? "https://" : "http://";
            var encodedUrl = StringHelper.UrlEncode(url);
            var dfopUrl = $"{scheme}{Config.DefaultApiHost}/dfop?fop={fop}&url={encodedUrl}";
            var token = _auth.CreateManageToken(dfopUrl);

            var result = await _httpManager.PostAsync(dfopUrl, token, true);
            return result;
        }
    }
}
