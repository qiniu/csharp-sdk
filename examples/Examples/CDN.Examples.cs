using System;
using Qiniu.Util;
using Qiniu.CDN;
using Qiniu.CDN.Model;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 融合CDN功能,另请参阅
    /// http://developer.qiniu.com/article/index.html#fusion-api-handbook
    /// </summary>
    public class FusionDemo
    {
        /// <summary>
        /// 缓存刷新
        /// </summary>
        public static void cdnRefresh()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            string[] urls = new string[] { "http://yourdomain.bkt.clouddn.com/somefile.php" };
            string[] dirs = new string[] { "http://yourdomain.bkt.clouddn.com/" };
            RefreshRequest request = new RefreshRequest();
            request.AddUrls(urls);
            request.AddDirs(dirs);

            var result = fusionMgr.RefreshUrlsAndDirs(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 文件预取
        /// </summary>
        public static void cdnPrefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            string[] urls = new string[] { "http://yourdomain.clouddn.com/somefile.php" };
            PrefetchRequest request = new PrefetchRequest(urls);
            PrefetchResult result = fusionMgr.PrefetchUrls(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 带宽
        /// </summary>
        public static void cdnBandwidth()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            BandwidthRequest request = new BandwidthRequest();
            request.StartDate = "2016-09-01"; 
            request.EndDate = "2016-09-20";
            request.Granularity = "day";
            request.Domains = "yourdomain.bkt.clouddn.com;yourdomain2;yourdomain3";
            BandwidthResult result = fusionMgr.GetBandwidthData(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 流量
        /// </summary>
        public static void cdnFlux()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            FluxRequest request = new FluxRequest();
            request.StartDate = "START_DATE"; 
            request.EndDate = "END_DATE"; 
            request.Granularity = "GRANU";
            request.Domains = "DOMAIN1;DOMAIN2"; 
            FluxResult result = fusionMgr.GetFluxData(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 日志查询
        /// </summary>
        public static void cdnLogList()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            LogListRequest request = new LogListRequest();
            request.Day = "2016-09-01"; // date:which-day
            request.Domains = "DOMAIN1;DOMAIN2"; // domains
            LogListResult result = fusionMgr.GetCdnLogList(request);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 时间戳防盗链
        /// </summary>
        public void hotLink()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            CdnManager fusionMgr = new CdnManager(mac);

            TimestampAntiLeechUrlRequest request = new TimestampAntiLeechUrlRequest();
            request.Host = "http://your-host";
            request.Path = "/path/";
            request.File = "file-name";
            request.Query = "?version=1.1";
            request.SetLinkExpire(600);

            //request.RawUrl

            string prefLink = fusionMgr.CreateTimestampAntiLeechUrl(request);

            Console.WriteLine(prefLink);
        }
    }
}
