using System;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.CDN
{
    /// <summary>
    ///     融合CDN加速-功能模块： 缓存刷新、文件预取、流量/带宽查询、日志查询、时间戳防盗链
    ///     另请参阅 http://developer.qiniu.com/article/index.html#fusion-api-handbook
    ///     关于时间戳防盗链可参阅 https://support.qiniu.com/question/195128
    /// </summary>
    public class CdnManager
    {
        private const string FusionApiHost = "http://fusion.qiniuapi.com";

        private readonly Auth _auth;
        private readonly HttpManager _httpManager;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="mac">账号(密钥)</param>
        public CdnManager(Mac mac)
        {
            _auth = new Auth(mac);
            _httpManager = new HttpManager();
        }

        private static string RefreshEntry => $"{FusionApiHost}/v2/tune/refresh";
        private static string PrefetchEntry => $"{FusionApiHost}/v2/tune/prefetch";
        private static string BandwidthEntry => $"{FusionApiHost}/v2/tune/bandwidth";
        private static string FluxEntry => $"{FusionApiHost}/v2/tune/flux";
        private static string LogListEntry => $"{FusionApiHost}/v2/tune/log/list";

        /// <summary>
        ///     缓存刷新-刷新URL和URL目录
        /// </summary>
        /// <param name="urls">要刷新的URL列表</param>
        /// <param name="dirs">要刷新的URL目录列表</param>
        /// <returns>缓存刷新的结果</returns>
        public async Task<RefreshResult> RefreshUrlsAndDirs(string[] urls, string[] dirs)
        {
            var request = new RefreshRequest(urls, dirs);
            var result = new RefreshResult();

            try
            {
                var url = RefreshEntry;
                var body = request.ToJsonStr();
                var token = _auth.CreateManageToken(url);

                var hr = await _httpManager.PostJsonAsync(RefreshEntry, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [refresh] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     缓存刷新-刷新URL
        /// </summary>
        /// <param name="urls">要刷新的URL列表</param>
        /// <returns>缓存刷新的结果</returns>
        public Task<RefreshResult> RefreshUrls(string[] urls)
        {
            return RefreshUrlsAndDirs(urls, null);
        }

        /// <summary>
        ///     缓存刷新-刷新URL目录
        /// </summary>
        /// <param name="dirs">要刷新的URL目录列表</param>
        /// <returns>缓存刷新的结果</returns>
        public Task<RefreshResult> RefreshDirs(string[] dirs)
        {
            return RefreshUrlsAndDirs(null, dirs);
        }

        /// <summary>
        ///     文件预取
        /// </summary>
        /// <param name="urls">待预取的文件URL列表</param>
        /// <returns>文件预取的结果</returns>
        public async Task<PrefetchResult> PrefetchUrls(string[] urls)
        {
            var request = new PrefetchRequest();
            request.AddUrls(urls);

            var result = new PrefetchResult();

            try
            {
                var url = PrefetchEntry;
                var body = request.ToJsonStr();
                var token = _auth.CreateManageToken(url);

                var hr = await _httpManager.PostJsonAsync(url, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [prefetch] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     批量查询cdn带宽
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="startDate">起始日期，如2017-01-01</param>
        /// <param name="endDate">结束日期，如2017-01-02</param>
        /// <param name="granularity">时间粒度，如day</param>
        /// <returns>带宽查询的结果</returns>
        public async Task<BandwidthResult> GetBandwidthData(string[] domains, string startDate, string endDate, string granularity)
        {
            var request = new BandwidthRequest
            {
                Domains = string.Join(";", domains),
                StartDate = startDate,
                EndDate = endDate,
                Granularity = granularity
            };

            var result = new BandwidthResult();

            try
            {
                var url = BandwidthEntry;
                var body = request.ToJsonStr();
                var token = _auth.CreateManageToken(url);

                var hr = await _httpManager.PostJsonAsync(url, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [bandwidth] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     批量查询cdn流量
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="startDate">起始日期，如2017-01-01</param>
        /// <param name="endDate">结束日期，如2017-01-02</param>
        /// <param name="granularity">时间粒度，如day</param>
        /// <returns>流量查询的结果</returns>
        public async Task<FluxResult> GetFluxData(string[] domains, string startDate, string endDate, string granularity)
        {
            var request = new FluxRequest
            {
                Domains = string.Join(";", domains),
                StartDate = startDate,
                EndDate = endDate,
                Granularity = granularity
            };

            var result = new FluxResult();

            try
            {
                var url = FluxEntry;
                var body = request.ToJsonStr();
                var token = _auth.CreateManageToken(url);

                var hr = await _httpManager.PostJsonAsync(url, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [flux] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     查询日志列表，获取日志的下载外链
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="day">具体日期，例如2017-08-12</param>
        /// <returns>日志查询的结果</returns>
        public async Task<LogListResult> GetCdnLogList(string[] domains, string day)
        {
            var request = new LogListRequest
            {
                Domains = string.Join(";", domains),
                Day = day
            };
            var result = new LogListResult();

            try
            {
                var url = LogListEntry;
                var body = request.ToJsonStr();
                var token = _auth.CreateManageToken(url);

                var hr = await _httpManager.PostJsonAsync(url, body, token);
                result.Shadow(hr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [loglist] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.INVALID_ARGUMENT;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        ///     时间戳防盗链
        /// </summary>
        /// <param name="host">主机，如http://domain.com</param>
        /// <param name="fileName">文件名，如 hello/world/test.jpg</param>
        /// <param name="query">请求参数，如?v=1.1</param>
        /// <param name="encryptKey">后台提供的key</param>
        /// <param name="expireInSeconds">链接有效时长</param>
        /// <returns>时间戳防盗链接</returns>
        public static string CreateTimestampAntiLeechUrl(
            string host,
            string fileName,
            string query,
            string encryptKey,
            int expireInSeconds)
        {
            var expireAt = UnixTimestamp.GetUnixTimestamp(expireInSeconds);
            var expireHex = expireAt.ToString("x");
            var path = $"/{Uri.EscapeUriString(fileName)}";
            var toSign = $"{encryptKey}{path}{expireHex}";
            var sign = Hashing.CalcMd5X(toSign);
            string finalUrl;
            if (!string.IsNullOrEmpty(query))
            {
                finalUrl = $"{host}{path}?{query}&sign={sign}&t={expireHex}";
            }
            else
            {
                finalUrl = $"{host}{path}?sign={sign}&t={expireHex}";
            }

            return finalUrl;
        }
    }
}
