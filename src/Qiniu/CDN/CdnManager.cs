using System;
using System.Text;
using Qiniu.Common;
using Qiniu.Util;
using Qiniu.Http;
using Qiniu.CDN.Model;

namespace Qiniu.CDN
{
    /// <summary>
    /// 融合CDN加速-功能模块： 缓存刷新、文件预取、流量/带宽查询、日志查询、时间戳防盗链
    /// 另请参阅 http://developer.qiniu.com/article/index.html#fusion-api-handbook
    /// 关于时间戳防盗链可参阅 https://support.qiniu.com/question/195128
    /// </summary>
    public class CdnManager
    {
        private Auth auth;
        private HttpManager httpManager;

        /// <summary>
        /// 初始化FusionManager
        /// </summary>
        /// <param name="mac">账户访问控制(密钥)</param>
        public CdnManager(Mac mac)
        {
            auth = new Auth(mac);
            httpManager = new HttpManager();
        }

        private string refreshEntry()
        {
            return string.Format("{0}/v2/tune/refresh", Config.FUSION_API_HOST);
        }

        private string prefetchEntry()
        {
            return string.Format("{0}/v2/tune/prefetch", Config.FUSION_API_HOST);
        }

        private string bandwidthEntry()
        {
            return string.Format("{0}/v2/tune/bandwidth", Config.FUSION_API_HOST);
        }

        private string fluxEntry()
        {
            return string.Format("{0}/v2/tune/flux", Config.FUSION_API_HOST);
        }

        private string loglistEntry()
        {
            return string.Format("{0}/v2/tune/log/list", Config.FUSION_API_HOST);
        }

        /// <summary>
        /// 缓存刷新，是指删除客户资源在 CDN 节点的缓存，以便更新新的资源。
        /// 具体做法是客户提交资源 url 到 CDN，由 CDN 来操作刷新。
        /// 另请参阅 http://developer.qiniu.com/article/fusion/api/refresh.html
        /// </summary>
        /// <param name="request">“缓存刷新”请求，详情请参见该类型的说明</param>
        /// <returns>缓存刷新的结果</returns>
        public RefreshResult refreshUrlsAndDirs(RefreshRequest request)
        {
            RefreshResult result = new RefreshResult();

            try
            {
                string url = refreshEntry();
                string body = request.ToJsonStr();
                string token = auth.createManageToken(url);

                HttpResult hr = httpManager.postJson(url, body, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] refresh Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 缓存刷新
        /// </summary>
        /// <param name="urls">要刷新的URL列表</param>
        /// <returns>刷新的结果</returns>
        public RefreshResult refreshUrls(string[] urls)
        {
            RefreshRequest request = new RefreshRequest(urls, null);
            return refreshUrlsAndDirs(request);
        }

        /// <summary>
        /// 缓存刷新
        /// </summary>
        /// <param name="dirs">要刷新的URL目录列表</param>
        /// <returns></returns>
        public RefreshResult refreshDirs(string[] dirs)
        {
            RefreshRequest request = new RefreshRequest(null,dirs);
            return refreshUrlsAndDirs(request);
        }

        /// <summary>
        /// 缓存刷新
        /// </summary>
        /// <param name="urls">要刷新的URL列表</param>
        /// <param name="dirs">要刷新的URL目录列表</param>
        /// <returns>刷新的结果</returns>
        public RefreshResult refreshUrlsAndDirs(string[] urls,string[] dirs)
        {
            RefreshRequest request = new RefreshRequest(urls, dirs);
            return refreshUrlsAndDirs(request);
        }

        /// <summary>
        /// 文件预取，也可称为预加热或预缓存，是指客户新资源提前由 CDN 拉取到 CDN 缓存节点。
        /// 具体做法是客户提交资源 url 到 CDN，由 CDN 来操作预取。
        /// 另请参阅 http://developer.qiniu.com/article/fusion/api/prefetch.html
        /// </summary>
        /// <param name="request">“文件预取”请求，详情请参阅该类型的说明</param>
        /// <returns>文件预取操作的结果</returns>
        public PrefetchResult prefetchUrls(PrefetchRequest request)
        {
            PrefetchResult result = new PrefetchResult();

            try
            {
                string url = prefetchEntry();
                string body = request.ToJsonStr();
                string token = auth.createManageToken(url);

                HttpResult hr = httpManager.postJson(url, body, token);
                result.shadow(hr);                
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] prefetch Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 文件预取
        /// </summary>
        /// <param name="urls">待预取的文件URL列表</param>
        /// <returns>文件预取结果</returns>
        public PrefetchResult prefetchUrls(string[] urls)
        {
            PrefetchRequest request = new PrefetchRequest(urls);
            return prefetchUrls(request);
        }

        /// <summary>
        /// 批量查询 cdn 带宽，另请参阅
        /// http://developer.qiniu.com/article/fusion/api/traffic-bandwidth.html#batch-bandwidth
        /// </summary>
        /// <param name="request">“带宽查询”请求，详情请参阅该类型的说明</param>
        /// <returns>带宽查询结果</returns>
        public BandwidthResult getBandwidthData(BandwidthRequest request)
        {
            BandwidthResult result = new BandwidthResult();

            try
            {
                string url = bandwidthEntry();
                string body = request.ToJsonStr();
                string token = auth.createManageToken(url);

                HttpResult hr = httpManager.postJson(url, body, token);
                result.shadow(hr);                
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] bandwidth Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批量查询 cdn 带宽
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="startDate">起始日期，如2017-01-01</param>
        /// <param name="endDate">结束日期，如2017-01-02</param>
        /// <param name="granularity">时间粒度，如day</param>
        /// <returns>带宽数居</returns>
        public BandwidthResult getBandwidthData(string[] domains,string startDate,string endDate,string granularity)
        {
            BandwidthRequest request = new BandwidthRequest(startDate, endDate, granularity, StringHelper.join(domains, ";"));
            return getBandwidthData(request);
        }

        /// <summary>
        /// 批量查询 cdn 流量，另请参阅
        /// http://developer.qiniu.com/article/fusion/api/traffic-bandwidth.html#batch-flux
        /// </summary>
        /// <param name="request">“流量查询”请求，详情请参阅该类型的说明</param>
        /// <returns>流量查询结果</returns>
        public FluxResult getFluxData(FluxRequest request)
        {
            FluxResult result = new FluxResult();

            try
            {
                string url = fluxEntry();
                string body = request.ToJsonStr();
                string token = auth.createManageToken(url);

                HttpResult hr = httpManager.postJson(url, body, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] flux Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 批量查询 cdn 流量
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="startDate">起始日期，如2017-01-01</param>
        /// <param name="endDate">结束日期，如2017-01-02</param>
        /// <param name="granularity">时间粒度，如day</param>
        /// <returns>流量数据</returns>
        public FluxResult getFluxData(string[] domains, string startDate, string endDate, string granularity)
        {
            FluxRequest request = new FluxRequest(startDate, endDate, granularity, StringHelper.join(domains, ";"));
            return getFluxData(request);
        }

        /// <summary>
        /// 日志下载(查询)接口可以查询域名日志列表，获取日志的下载外链，只提供 30 个自然日内的日志下载。
        /// 例如当前日期为 2016-08-31，则只提供 2016-08-01 ~ 2016-08-30 的日志。
        /// 另请参阅 http://developer.qiniu.com/article/fusion/api/log.html
        /// </summary>
        /// <param name="request">“日志查询”请求，详情请参阅该类型的说明</param>
        /// <returns>日志列表</returns>
        public LogListResult getCdnLogList(LogListRequest request)
        {
            LogListResult result = new LogListResult();

            try
            {
                string url = loglistEntry();
                string body = request.ToJsonStr();
                string token = auth.createManageToken(url);

                HttpResult hr = httpManager.postJson(url, body, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] loglist Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 查询日志列表
        /// </summary>
        /// <param name="domains">域名列表</param>
        /// <param name="date">指定日期，如2017-01-01</param>
        /// <returns>日志列表</returns>
        public LogListResult getCdnLogList(string[] domains,string date)
        {
            LogListRequest request = new LogListRequest(date, StringHelper.join(domains, ";"));
            return getCdnLogList(request);
        }

        /// <summary>
        /// 时间戳防盗链
        /// 另请参阅https://support.qiniu.com/question/195128
        /// </summary>
        /// <param name="request">“时间戳防盗链”请求，详情请参阅该类型的说明</param>
        /// <returns>已授权链接(包含过期时间戳)</returns>
        public string createTimestampAntiLeechUrl(HotLinkRequest request)
        {
            string RAW = request.RawUrl;

            string key = request.Key;
            string path = Uri.EscapeUriString(request.Path);
            string file = request.File;
            string ts = (int.Parse(request.Timestamp)).ToString("x");
            string SIGN = Hashing.calcMD5(key + path + file + ts);

            return string.Format("{0}&sign={1}&t={2}", RAW, SIGN, ts);
        }

        /// <summary>
        /// 时间戳防盗链
        /// </summary>
        /// <param name="host">主机，如http://domain.com</param>
        /// <param name="path">路径，如/dir1/dir2/</param>
        /// <param name="fileName">文件名，如1.jpg</param>
        /// <param name="query">请求参数，如?v=1.1</param>
        /// <param name="encryptKey">后台提供的key</param>
        /// <param name="expireInSeconds">链接有效时长</param>
        /// <returns>已授权链接(包含过期时间戳)</returns>
        public string createTimestampAntiLeechUrl(string host, string path, string fileName, string query, string encryptKey, int expireInSeconds)
        {
            HotLinkRequest request = new HotLinkRequest();
            request.Host = host;
            request.Path = path;
            request.File = fileName;
            request.Query = query;
            request.Key = encryptKey;
            request.SetLinkExpire(expireInSeconds);

            return createTimestampAntiLeechUrl(request);
        }

    }
}
