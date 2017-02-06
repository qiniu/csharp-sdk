using System;
using Qiniu.Util;

namespace Qiniu.CDN.Model
{
    /// <summary>
    /// 时间戳防盗链 - 请求
    /// </summary>
    public class TimestampAntiLeechUrlRequest
    {
        /// <summary>
        /// 原始链接，host+path+file+query
        /// </summary>
        public string RawUrl
        {
            get
            {
                return Host + Path + File + Query;
            }
        }

        /// <summary>
        /// URL的Host部分，例如http://cxxx.dyyy.com
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// URL的目录部分，例如/DIR1/dir2/
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// URL中的文件名，例如vodfile.mp4
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// URL中的参数部分，例如?v1.1
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// QINIU提供的key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 链接失效时刻(时间戳)
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 初始化(全部为空，需要后续赋值)
        /// </summary>
        public TimestampAntiLeechUrlRequest()
        {
            Host = "";
            Path = "";
            File = "";
            Query = "";
            Key = "";
            Timestamp = "";
        }

        /// <summary>
        /// 初始化，提供原始URL，key，有效时长expire(当前时刻起多少秒以后失效)
        /// </summary>
        /// <param name="url">原始URL</param>
        /// <param name="key">QINIU提供的key</param>
        /// <param name="expire">有效时长(秒)</param>
        public TimestampAntiLeechUrlRequest(string url, string key, int expire)
        {
            string host, path, file, query;
            UrlHelper.UrlSplit(url, out host, out path, out file, out query);

            Host = host;
            Path = path;
            File = file;
            Query = query;
            Key = key;

            SetLinkExpire(expire);
        }

        /// <summary>
        /// 设置失效时间戳，当前时刻起多少秒以后失效
        /// </summary>
        /// <param name="seconds">单位:秒</param>
        public void SetLinkExpire(int seconds)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            DateTime dt1 = DateTime.Now.AddSeconds(seconds);
            TimeSpan tsx = dt1.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            Timestamp = sts.Substring(0, sts.Length - 7);
        }

        /// <summary>
        /// 设置失效时间戳
        /// </summary>
        /// <param name="stopAt">失效时刻</param>
        public void SetLinkExpire(DateTime stopAt)
        {
            DateTime dt0 = (new DateTime(1910, 1, 1)).ToLocalTime();
            TimeSpan tsx = stopAt.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            Timestamp = sts.Substring(0, sts.Length - 7);
        }
    }
}
