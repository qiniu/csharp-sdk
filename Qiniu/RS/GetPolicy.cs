using System;
using Qiniu.Auth.digest;
using Qiniu.Conf;

namespace Qiniu.RS
{
	/// <summary>
	/// GetPolicy
	/// </summary>
	public class GetPolicy
	{
		public static string MakeRequest (string baseUrl, UInt32 expires = 3600, Mac mac = null)
		{
			if (mac == null) {
				mac = new Mac (Config.ACCESS_KEY, Config.Encoding.GetBytes (Config.SECRET_KEY));
			}

			UInt32 deadline = (UInt32)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 + expires);
			if (baseUrl.Contains ("?")) {
				baseUrl += "&e=";
			} else {
				baseUrl += "?e=";
			}
			baseUrl += deadline;
			string token = mac.Sign (Conf.Config.Encoding.GetBytes (baseUrl));
			return string.Format ("{0}&token={1}", baseUrl, token);
		}

		public static string MakeBaseUrl (string domain, string key)
		{
			return string.Format ("http://{0}/{1}", domain, key);
		}

        public static string MakeSaveasUrl(string baseUrl, string saveBucket, string saveKey, Mac mac = null)
        {
            if (mac == null)
            {
                mac = new Mac(Config.ACCESS_KEY, Config.Encoding.GetBytes(Config.SECRET_KEY));
            }

            baseUrl = baseUrl.Replace(@"http://", "").Replace(@"https://", "");

            string encodedEntryURI = Util.Base64URLSafe.ToBase64URLSafe(saveBucket + ":" + saveKey);

            string url = baseUrl + "|saveas/" + encodedEntryURI;

            string sign = mac.Sign(Conf.Config.Encoding.GetBytes(url));

            return string.Format("http://{0}/sign/{1}", url, sign);

        }
	}
}
