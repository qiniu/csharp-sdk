using System;
using System.Linq;
using Qiniu.Auth.digest;
using Qiniu.Conf;
namespace Qiniu.RS
{
    /// <summary>
    /// GetPolicy
    /// </summary>
    public class GetPolicy
    {
        public static string MakeRequest(string baseUrl, UInt32 expires = 3600, Mac mac = null)
        {
            if (mac == null)
            {
                mac = new Mac(Config.ACCESS_KEY, Config.Encoding.GetBytes(Config.SECRET_KEY));
            }
            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            UInt32 deadline = (UInt32)interval.TotalSeconds + expires;
            if (baseUrl.Contains('?'))
            {
                baseUrl += "&e=";
            }
            else
            {
                baseUrl += "?e=";
            }
            baseUrl += deadline;
            string token = mac.Sign(Conf.Config.Encoding.GetBytes(baseUrl));
            return string.Format("{0}&token={1}", baseUrl, token);
        }
        public static string MakeBaseUrl(string domain, string key)
        {
            return string.Format("http://{0}/{1}",domain, System.Web.HttpUtility.UrlEncode(key));
        }

    }
}
