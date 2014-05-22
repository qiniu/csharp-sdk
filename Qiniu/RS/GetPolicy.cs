using System;
using Qiniu.Auth.digest;
using Qiniu.Conf;
using System.Net;
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
			key = Uri.EscapeUriString (key);
			return string.Format ("http://{0}/{1}", domain, key);
		}
	}
}
