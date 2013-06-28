using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QBox.Auth.digest;
namespace QBox.RS
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPolicy
    {
        private UInt32 expires;

        public UInt32 Expires
        {
            get { return expires; }
            set { expires = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expires"></param>
        public GetPolicy(UInt32 expires=3600)
        {
            this.expires = expires;
        }

        public string MakeRequest(string baseUrl, Mac mac = null)
        {
            if (mac == null)
                mac = new Mac();

            DateTime begin = new DateTime(1970, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan interval = new TimeSpan(now.Ticks - begin.Ticks);
            UInt32 deadline = (UInt32)interval.TotalSeconds + expires;
            if (baseUrl.Contains('&'))
            {
                baseUrl += "&e";
            }
            else
            {
                baseUrl += "?e";
            }
            baseUrl += deadline;
            string token = mac.Sign(Conf.Config.Encoding.GetBytes(baseUrl));
            return string.Format("{0}&token={1}", baseUrl, token);
        }
        
    }
}
