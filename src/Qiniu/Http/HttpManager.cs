using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Qiniu.Util;

namespace Qiniu.Http
{
    /// <summary>
    /// HttpManager for .NET 2.0/3.0/3.5/4.0
    /// </summary>
    public class HttpManager
    {
        private readonly bool _allowAutoRedirect;
        private string _userAgent;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="allowAutoRedirect">是否允许HttpWebRequest的“重定向”，默认禁止</param>
        public HttpManager(bool allowAutoRedirect = false)
        {
            _allowAutoRedirect = allowAutoRedirect;
            _userAgent = GetUserAgent();
        }

        /// <summary>
        /// 客户端标识(UserAgent)，示例："SepcifiedClient/1.1 (Universal)"
        /// </summary>
        /// <returns>客户端标识UA</returns>
        public static string GetUserAgent()
        {
            string osDesc = Environment.OSVersion.Platform + "; " + Environment.OSVersion.Version;
            return $"{QiniuCSharpSDK.ALIAS}/{QiniuCSharpSDK.VERSION} ({QiniuCSharpSDK.RTFX}; {osDesc})";
        }

        /// <summary>
        /// 设置自定义的客户端标识(UserAgent)，示例："SepcifiedClient/1.1 (Universal)"
        /// 如果设置为空白或者不设置，SDK会自动使用默认的UserAgent
        /// </summary>
        /// <param name="userAgent">用户自定义的UserAgent</param>
        /// <returns>客户端标识UA</returns>
        public void SetUserAgent(string? userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                _userAgent = userAgent;
            }
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>分界(boundary)标识字符串</returns>
        public static string CreateFormDataBoundary()
        {
            string now = DateTime.UtcNow.Ticks.ToString();
            return $"-------{QiniuCSharpSDK.ALIAS}Boundary{Hashing.CalcMD5X(now)}";
        }

        public HttpRequestOptions CreateHttpRequestOptions(
            string method,
            string url,
            StringDictionary? headers,
            string? token = null
        )
        {
            HttpRequestOptions reqOpts = new HttpRequestOptions();

            reqOpts.Method = method;
            reqOpts.Url = url;

            if (headers != null)
            {
                reqOpts.Headers = headers;
            }

            if (!string.IsNullOrEmpty(token))
            {
                reqOpts.Headers.Add("Authorization", token);
            }

            reqOpts.Headers.Add("User-Agent", _userAgent);
            reqOpts.AllowAutoRedirect = _allowAutoRedirect;

            return reqOpts;
        }

        public HttpResult CreateHttpResult(HttpResponseMessage response, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            if (response == null)
            {
                return result;
            }

            result.Code = (int)response.StatusCode;
            result.RefCode = (int)response.StatusCode;

            getHeaders(ref result, response);

            using (response)
            {
                var content = response.Content;
                if (content == null)
                {
                    return result;
                }

                if (binaryMode)
                {
                    result.Data = content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                {
                    result.Text = content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                }
            }
            return result;
        }

        public HttpResult SendRequest(HttpRequestOptions reqOpts, Boolean binaryMode = false)
        {
            HttpResult result;

            try
            {
                using var handler = reqOpts.CreateHttpClientHandler();
                using var client = new HttpClient(handler);
                if (reqOpts.Timeout.HasValue)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(reqOpts.Timeout.Value);
                }

                using var request = reqOpts.CreateHttpRequestMessage();
                using HttpResponseMessage response = client.Send(request);

                result = CreateHttpResult(response, binaryMode);
            }
            catch (HttpRequestException httpRequestException) when (httpRequestException.StatusCode.HasValue)
            {
                result = new HttpResult
                {
                    Code = (int)httpRequestException.StatusCode.Value,
                    RefCode = (int)httpRequestException.StatusCode.Value,
                    RefText = httpRequestException.Message
                };
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{_userAgent}] [HTTP-{reqOpts.Method}] Error:  ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result = CreateHttpResult(null);
                result.RefCode = (int)HttpCode.USER_UNDEF;
                result.RefText += sb.ToString();
            }

            return result;
        }

        public HttpResult SendRequest(HttpRequestOptions reqOpts, List<IMiddleware> middlewares, Boolean binaryMode = false)
        {
            if (middlewares == null || middlewares.Count == 0)
            {
                return SendRequest(reqOpts, binaryMode);
            }

            List<IMiddleware> reversedMiddlewares = new List<IMiddleware>(middlewares.Count);
            reversedMiddlewares.AddRange(middlewares);
            reversedMiddlewares.Reverse();
            DNextSend composedHandle = reversedMiddlewares.Aggregate<IMiddleware, DNextSend>(
                req => SendRequest(req, binaryMode),
                (handle, middleware) => request => middleware.Send(request, handle)
            );

            return composedHandle(reqOpts);
        }

        /// <summary>
        /// HTTP-GET 方法（不包含 headers）
        /// </summary>
        /// <param name="url">请求目标 URL</param>
        /// <param name="token">令牌(凭证)[可选 -> 设置为 null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-GET的响应结果</returns>
        public HttpResult Get(string url, string token, bool binaryMode = false)
        {
            return Get(url, null, token, binaryMode);
        }

        public HttpResult Get(string url, StringDictionary headers, Auth auth, bool binaryMode = false)
        {
            if (headers == null)
            {
                headers = new StringDictionary{
                    {"Content-Type", ContentType.WWW_FORM_URLENC}
                };
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                headers["Content-Type"] = ContentType.WWW_FORM_URLENC;
            }
            
            addAuthHeaders(ref headers, auth);

            string token = auth.CreateManageTokenV2("GET", url, headers);
            return Get(url, headers, token, binaryMode);
        }

        /// <summary>
        /// HTTP-GET 方法
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="headers">请求 Headers[可选 -> 设置为 null]</param>
        /// <param name="token">令牌(凭证)[可选 -> 设置为 null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-GET的响应结果</returns>
        public HttpResult Get(string url, StringDictionary headers, string token, bool binaryMode = false)
        {
            return Get(url, headers, token, null, binaryMode);
        }

        public HttpResult Get(string url, StringDictionary headers, string token, List<IMiddleware> middlewares, bool binaryMode = false)
        {
            if (headers == null)
            {
                headers = new StringDictionary{
                    {"Content-Type", ContentType.WWW_FORM_URLENC}
                };
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                headers["Content-Type"] = ContentType.WWW_FORM_URLENC;
            }

            HttpRequestOptions requestOptions = CreateHttpRequestOptions("GET", url, headers, token);
            return SendRequest(requestOptions, middlewares, binaryMode);
        }

        /// <summary>
        /// HTTP-POST 方法（不包含 headers，不包含 body 数据）
        /// </summary>
        /// <param name="url">请求目标 URL</param>
        /// <param name="token">令牌(凭证)[可选 -> 设置为 null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST 的响应结果</returns>
        public HttpResult Post(string url, string token, bool binaryMode = false)
        {
            return Post(url, null, token, binaryMode);
        }

        public HttpResult Post(string url, StringDictionary headers, Auth auth, bool binaryMode = false)
        {
            if (headers == null)
            {
                headers = new StringDictionary{
                    {"Content-Type", ContentType.WWW_FORM_URLENC}
                };
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                headers["Content-Type"] = ContentType.WWW_FORM_URLENC;
            }

            addAuthHeaders(ref headers, auth);

            string token = auth.CreateManageTokenV2("POST", url, headers);
            return Post(url, headers, token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST 方法（不包含 body 数据）
        /// </summary>
        /// <param name="url">请求目标 URL</param>
        /// <param name="token">令牌(凭证)[可选 -> 设置为 null]</param>
        /// <param name="headers">请求 Headers[可选 -> 设置为 null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST 的响应结果</returns>
        public HttpResult Post(string url, StringDictionary headers, string token, bool binaryMode = false)
        {
            HttpRequestOptions reqOpts = CreateHttpRequestOptions("POST", url, headers, token);
            return SendRequest(reqOpts, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(字节数据)</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostData(string url, byte[] data, string token, bool binaryMode = false)
        {
            return PostData(
                url,
                data,
                ContentType.APPLICATION_OCTET_STREAM,
                token,
                binaryMode
            );
        }

        /// <summary>
        /// HTTP-POST方法(包含body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(字节数据)</param>
        /// <param name="mimeType">主体数据内容类型</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostData(string url, byte[] data, string mimeType, string token, bool binaryMode = false)
        {
            HttpRequestOptions reqOpts = CreateHttpRequestOptions("POST", url, null, token);

            reqOpts.Headers.Add("Content-Type", mimeType);
            if (data != null)
            {
                reqOpts.AllowWriteStreamBuffering = true;
                reqOpts.RequestData = data;
            }

            return SendRequest(reqOpts, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含JSON文本的body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(JSON文本)</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostJson(string url, string data, string token, bool binaryMode = false)
        {
            byte[] utf8Data = null;
            if (!string.IsNullOrEmpty(data))
            {
                utf8Data = Encoding.UTF8.GetBytes(data);
            }
            return PostData(url, utf8Data, ContentType.APPLICATION_JSON, token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含普通文本的body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(普通文本)</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostText(string url, string data, string token, bool binaryMode = false)
        {
            byte[] utf8Data = null;
            if (!string.IsNullOrEmpty(data))
            {
                utf8Data = Encoding.UTF8.GetBytes(data);
            }
            return PostData(url, utf8Data, ContentType.TEXT_PLAIN, token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="kvData">键值对数据</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostForm(string url, Dictionary<string, string> kvData, string token, bool binaryMode = false)
        {
            string data = null;
            if (kvData != null)
            {
                data = string.Join("&",
                    kvData.Select(kvp => Uri.EscapeDataString(kvp.Key) + "=" + Uri.EscapeDataString(kvp.Value)));
            }
            return PostForm(url, data, token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostForm(string url, string data, string token, bool binaryMode = false)
        {
            byte[] utf8Data = null;
            if (!string.IsNullOrEmpty(data))
            {
                utf8Data = Encoding.UTF8.GetBytes(data);
            }
            return PostData(url, utf8Data, ContentType.WWW_FORM_URLENC, token, binaryMode);
        }

        public HttpResult PostForm(string url, StringDictionary headers, string data, Auth auth, bool binaryMode = false)
        {
            if (headers == null)
            {
                headers = new StringDictionary{
                    {"Content-Type", ContentType.WWW_FORM_URLENC}
                };
            }

            if (!headers.ContainsKey("Content-Type"))
            {
                headers["Content-Type"] = ContentType.WWW_FORM_URLENC;
            }
            
            addAuthHeaders(ref headers, auth);

            string token = auth.CreateManageTokenV2("POST", url, headers, data);
            return PostForm(url, headers, Encoding.UTF8.GetBytes(data), token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostForm(string url, byte[] data, string token, bool binaryMode = false)
        {
            return PostForm(url, null, data, token, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="headers">请求 Headers[可选 -> 设置为 null]</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostForm(string url, StringDictionary headers, byte[] data, string token, bool binaryMode = false)
        {
            HttpRequestOptions reqOpts = CreateHttpRequestOptions("POST", url, headers, token);
            if (!reqOpts.Headers.ContainsKey("Content-Type"))
            {
                reqOpts.Headers.Add("Content-Type", ContentType.WWW_FORM_URLENC);
            }
            if (data != null)
            {
                reqOpts.AllowWriteStreamBuffering = true;
                reqOpts.RequestData = data;
            }

            return SendRequest(reqOpts, binaryMode);
        }

        /// <summary>
        /// HTTP-POST方法(包含多分部数据,multipart/form-data)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="boundary">分界标志</param>
        /// <param name="token">令牌(凭证)[可选->设置为null]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public HttpResult PostMultipart(string url, byte[] data, string boundary, string token, bool binaryMode = false)
        {
            HttpRequestOptions reqOpts = CreateHttpRequestOptions("POST", url, null, token);

            reqOpts.Headers.Add(
                "Content-Type",
                $"{ContentType.MULTIPART_FORM_DATA}; boundary={boundary}"
            );

            if (data != null)
            {
                reqOpts.AllowWriteStreamBuffering = true;
                reqOpts.RequestData = data;
            }

            return SendRequest(reqOpts, binaryMode);
        }

        /// <summary>
        /// HTTP-PUT方法(包含body数据, headers)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(字节数据)</param>
        /// <param name="headers">上传设置的headers</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-PUT的响应结果</returns>
        public HttpResult PutDataWithHeaders(string url, byte[] data, Dictionary<string, string> headers, bool binaryMode = false)
        {
            // converse headers type for compaction
            StringDictionary headersDict = new StringDictionary();
            foreach (KeyValuePair<string, string> kvp in headers)
            {
                headersDict.Add(kvp.Key, kvp.Value);
            }

            HttpRequestOptions wReq = CreateHttpRequestOptions("PUT", url, headersDict);

            wReq.Headers.Add("Content-Type", ContentType.APPLICATION_OCTET_STREAM);

            if (data != null)
            {
                wReq.AllowWriteStreamBuffering = true;
                wReq.RequestData = data;
            }

            return SendRequest(wReq, binaryMode);
        }

        /// <summary>
        /// 获取返回信息头
        /// </summary>
        /// <param name="hr">即将被HTTP请求封装函数返回的HttpResult变量</param>
        /// <param name="resp">正在被读取的HTTP响应</param>
        private void getHeaders(ref HttpResult hr, HttpResponseMessage resp)
        {
            if (resp != null)
            {
                if (hr.RefInfo == null)
                {
                    hr.RefInfo = new Dictionary<string, string>();
                }

                hr.RefInfo.Add("ProtocolVersion", resp.Version.ToString());

                if (resp.Content?.Headers?.ContentType?.CharSet is string characterSet && !string.IsNullOrEmpty(characterSet))
                {
                    hr.RefInfo.Add("Characterset", characterSet);
                }

                if (resp.Content?.Headers?.ContentEncoding != null)
                {
                    hr.RefInfo.Add("ContentEncoding", string.Join(",", resp.Content.Headers.ContentEncoding));
                }

                if (resp.Content?.Headers?.ContentType != null)
                {
                    hr.RefInfo.Add("ContentType", resp.Content.Headers.ContentType.ToString());
                }

                hr.RefInfo.Add("ContentLength", (resp.Content?.Headers?.ContentLength ?? 0).ToString());

                foreach (var header in resp.Headers)
                {
                    hr.RefInfo[header.Key] = string.Join(",", header.Value);
                }

                if (resp.Content?.Headers != null)
                {
                    foreach (var header in resp.Content.Headers)
                    {
                        hr.RefInfo[header.Key] = string.Join(",", header.Value);
                    }
                }
            }
        }


        private void addAuthHeaders(ref StringDictionary headers, Auth auth)
        {
            string xQiniuDate = DateTime.UtcNow.ToString("yyyyMMdd'T'HHmmss'Z'");
            string xQiniuDateDisableEnv = Environment.GetEnvironmentVariable("DISABLE_QINIU_TIMESTAMP_SIGNATURE");
            if (auth.AuthOptions.DisableQiniuTimestampSignature is bool disableQiniuTimestampSignature)
            {
                if (!disableQiniuTimestampSignature)
                {
                    headers["X-Qiniu-Date"] = xQiniuDate;
                }
            }
            else if (!String.IsNullOrEmpty(xQiniuDateDisableEnv))
            {
                if (xQiniuDateDisableEnv.ToLower() != "true")
                {
                    headers["X-Qiniu-Date"] = xQiniuDate;
                }
            }
            else
            {
                headers["X-Qiniu-Date"] = xQiniuDate;
            }
        }
    }
}