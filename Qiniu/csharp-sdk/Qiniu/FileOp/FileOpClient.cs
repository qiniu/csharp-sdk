using System;
using System.Net;
using System.IO;
using Qiniu.RPC;

namespace Qiniu.FileOp
{
	static class FileOpClient
	{
		public static CallRet Get (string url)
		{
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.Method = "GET";
				request.UserAgent = Conf.Config.USER_AGENT;
				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					return HandleResult (response);
				}
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return new CallRet (HttpStatusCode.BadRequest, e);
			}
		}

		public static CallRet HandleResult (HttpWebResponse response)
		{
			HttpStatusCode statusCode = response.StatusCode;
			using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
				string responseStr = reader.ReadToEnd ();
				return new CallRet (statusCode, responseStr);
			}
		}
	}
}
