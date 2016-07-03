using System;
using System.Net;
using System.IO;
using Qiniu.RPC;
#if ABOVE45
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace Qiniu.FileOp
{
	static class FileOpClient
	{
#if !ABOVE45
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
#else
        public static async Task<CallRet> GetAsync(string url)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var client = new HttpClient())
                    {
                        request.Headers.Add("User-Agent", Conf.Config.USER_AGENT);
                        var response = await client.SendAsync(request);
                        return await HandleResultAsync(response);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
        }

        public static async Task<CallRet> HandleResultAsync(HttpResponseMessage response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            var responseStr = await response.Content.ReadAsStringAsync();
            return new CallRet(statusCode, responseStr);
        }
#endif
    }
}
