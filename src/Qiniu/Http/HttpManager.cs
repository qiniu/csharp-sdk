using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Util;

namespace Qiniu.Http
{
    /// <summary>
    ///     HttpManager for .NET 4.5+
    /// </summary>
    public class HttpManager : IDisposable
    {
        public static readonly HttpManager SharedInstance = new HttpManager();

        private readonly HttpClient _client;
        private string _userAgent;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="allowAutoRedirect">是否允许HttpWebRequest的“重定向”，默认禁止</param>
        public HttpManager(bool allowAutoRedirect = false) : this(null, allowAutoRedirect)
        {
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="baseUrl">API Host，可选</param>
        /// <param name="allowAutoRedirect">是否允许HttpWebRequest的“重定向”，默认禁止</param>
        public HttpManager(string baseUrl, bool allowAutoRedirect = false)
        {
            _client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = allowAutoRedirect });
            if (!string.IsNullOrEmpty(baseUrl)) _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.ExpectContinue = false;
            SetUserAgent(GetUserAgent());
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        /// <summary>
        ///     客户端标识(UserAgent)，示例："SepcifiedClient/1.1 (Universal)"
        /// </summary>
        /// <returns>客户端标识UA</returns>
        public static string GetUserAgent()
        {
#if NETSTANDARD1_3
            var osDesc = $"{System.Runtime.InteropServices.RuntimeInformation.OSDescription}";
#else
            var osDesc = $"{Environment.OSVersion.Platform}; {Environment.OSVersion.Version}";
#endif
            return $"{QiniuCSharpSdk.Alias}/{QiniuCSharpSdk.Version} ({QiniuCSharpSdk.RTFX}; {osDesc})";
        }

        /// <summary>
        ///     设置自定义的客户端标识(UserAgent)，示例："SepcifiedClient/1.1 (Universal)"
        ///     如果设置为空白或者不设置，SDK会自动使用默认的UserAgent
        /// </summary>
        /// <param name="userAgent">用户自定义的UserAgent</param>
        /// <returns>客户端标识UA</returns>
        public void SetUserAgent(string userAgent)
        {
            _userAgent = userAgent;
            _client.DefaultRequestHeaders.UserAgent.Clear();
            if (!string.IsNullOrEmpty(userAgent))
            {
                _client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            }
        }

        /// <summary>
        ///     多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>分界(boundary)标识字符串</returns>
        public static string CreateFormDataBoundary()
        {
            var now = DateTime.UtcNow.Ticks.ToString();
            return $"-------{QiniuCSharpSdk.Alias}Boundary{Hashing.CalcMd5X(now)}";
        }

        /// <summary>
        ///     发送HTTP请求
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP请求的响应结果</returns>
        public async Task<HttpResult> SendAsync(HttpRequestMessage request, string token = null, bool binaryMode = false)
        {
            var result = new HttpResult();
            HttpResponseMessage response = null;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Add("Authorization", token);
            }

            try
            {
                response = await _client.SendAsync(request);
                await result.ReadAsync(response, binaryMode);
            }
            catch (Exception exception)
            {
                var wrapper = new Exception($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{_userAgent}] [HTTP-{request.Method.Method}] Error: ", exception);
                await result.ReadErrorAsync(wrapper, response);
            }
            finally
            {
                response?.Dispose();
            }

            return result;
        }

        /// <summary>
        ///     HTTP-GET方法
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-GET的响应结果</returns>
        public Task<HttpResult> GetAsync(string url, string token = null, bool binaryMode = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return SendAsync(request, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(不包含body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostAsync(string url, string token = null, bool binaryMode = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            return SendAsync(request, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="content">主体数据[可选]</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostAsync(string url, HttpContent content = null, string token = null, bool binaryMode = false)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            return SendAsync(request, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(包含body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(字节数据)</param>
        /// <param name="mimeType">主体数据内容类型</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostDataAsync(string url, byte[] data, string mimeType = "application/octet-stream", string token = null, bool binaryMode = false)
        {
            var content = new ByteArrayContent(data);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(mimeType);
            return PostAsync(url, content, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostFormAsync(string url, string data, string token = null, bool binaryMode = false)
        {
            var content = new StringContent(data, Encoding.UTF8, ContentType.WWW_FORM_URLENC);
            return PostAsync(url, content, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostFormAsync(string url, byte[] data, string token = null, bool binaryMode = false)
        {
            var content = new ByteArrayContent(data);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType.WWW_FORM_URLENC);
            return PostAsync(url, content, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(包含JSON文本的body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(JSON文本)</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostJsonAsync(string url, string data, string token = null, bool binaryMode = false)
        {
            var content = new StringContent(data, Encoding.UTF8, ContentType.APPLICATION_JSON);
            return PostAsync(url, content, token, binaryMode);
        }

        /// <summary>
        ///     HTTP-POST方法(包含普通文本的body数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据(普通文本)</param>
        /// <param name="token">令牌(凭证)[可选]</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容(默认:否，即表示以文本方式读取)</param>
        /// <returns>HTTP-POST的响应结果</returns>
        public Task<HttpResult> PostTextAsync(string url, string data, string token = null, bool binaryMode = false)
        {
            var content = new StringContent(data, Encoding.UTF8, ContentType.TEXT_PLAIN);
            return PostAsync(url, content, token, binaryMode);
        }
    }
}
