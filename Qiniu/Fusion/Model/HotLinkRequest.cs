using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Util;

namespace Qiniu.Fusion.Model
{
    public class HotLinkRequest
    {
        public string RawUrl
        {
            get
            {
                return Host + Path + File + Query;
            }
        }

        public string Host { get; set; }

        public string Path { get; set; }

        public string File { get; set; }

        public string Query { get; set; }

        public string Key { get; set; }

        public string Timestamp { get; set; }

        public HotLinkRequest()
        {
            Host = "";
            Path = "";
            File = "";
            Query = "";
            Key = "";
            Timestamp = "";
        }

        public HotLinkRequest(string url, string key, int expire)
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

        public void SetLinkExpire(int seconds)
        {
            DateTime dt0 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dt1 = DateTime.Now.AddSeconds(seconds);
            TimeSpan tsx = dt1.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            Timestamp = sts.Substring(0, sts.Length - 7);
        }

        public void SetLinkExpire(DateTime stopAt)
        {
            DateTime dt0 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan tsx = stopAt.Subtract(dt0);
            string sts = tsx.Ticks.ToString();
            Timestamp = sts.Substring(0, sts.Length - 7);
        }
    }
}
