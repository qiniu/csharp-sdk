using System;
using Qiniu.Util;
using Qiniu.Fusion;
using Qiniu.Fusion.Model;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 融合CDN功能
    /// </summary>
    public class FusionDemo
    {
        /// <summary>
        /// 缓存刷新
        /// </summary>
        public static void refresh()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            string[] urls = new string[] { "URL1", "URL2" };
            string[] dirs = new string[] { "DIR1", "DIR2" };
            RefreshRequest request = new RefreshRequest();
            request.AddUrls(urls);
            request.AddDirs(dirs);
            RefreshResult result = fusionMgr.Refresh(request);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 文件预取
        /// </summary>
        public static void prefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            string[] urls = new string[] { "URL1", "URL2" };
            PrefetchRequest request = new PrefetchRequest(urls);
            PrefetchResult result = fusionMgr.Prefetch(request);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 带宽
        /// </summary>
        public static void bandwidth()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            BandwidthRequest request = new BandwidthRequest();
            request.StartDate = "START_DATE"; // "2016-09-01"
            request.EndDate = "END_DATE"; // "2016-09-20"
            request.Granularity = "GRANU"; // "day"
            request.Domains = "DOMAIN1;DOMAIN2"; // domains
            BandwidthResult result = fusionMgr.Bandwidth(request);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 流量
        /// </summary>
        public static void flux()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            FluxRequest request = new FluxRequest();
            request.StartDate = "START_DATE"; // "2016-09-01"
            request.EndDate = "END_DATE"; // "2016-09-20"
            request.Granularity = "GRANU"; // "day"
            request.Domains = "DOMAIN1;DOMAIN2"; // domains
            FluxResult result = fusionMgr.Flux(request);
            Console.WriteLine(result);
        }

        /// <summary>
        /// 日志查询
        /// </summary>
        public static void loglist()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager fusionMgr = new FusionManager(mac);

            LogListRequest request = new LogListRequest();
            request.Day = "DAY"; // "2016-09-01"
            request.Domains = "DOMAIN1"; // domains
            LogListResult result = fusionMgr.LogList(request);
            Console.WriteLine(result);
        }
    }
}
