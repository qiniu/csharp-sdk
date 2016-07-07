using System;
using System.IO;
using System.Net;
#if ABOVE45
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace Qiniu.RPC
{
	public class Client
	{
#if !ABOVE45
        public virtual void SetAuth (HttpWebRequest request, Stream body)
		{
		}

		public CallRet Call (string url)
		{
			Console.WriteLine ("Client.Post ==> URL: " + url);
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.UserAgent = Conf.Config.USER_AGENT;
				request.Method = "POST";                
				SetAuth (request, null);
				using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
					return HandleResult (response);
				}
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return new CallRet (HttpStatusCode.BadRequest, e);
			}
		}

		public CallRet CallWithBinary (string url, string contentType, Stream body, long length)
		{
			Console.WriteLine ("Client.PostWithBinary ==> URL: {0} Length:{1}", url, length);
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
				request.UserAgent = Conf.Config.USER_AGENT;
				request.Method = "POST";
				request.ContentType = contentType;
				request.ContentLength = length;
				SetAuth (request, body);
				using (Stream requestStream = request.GetRequestStream()) {
					Util.IO.CopyN (requestStream, body, length);
				}
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
        public virtual Task SetAuth(HttpRequestMessage request)
        {
            return Task.FromResult(0);
        }

        public async Task<CallRet> CallAsync(string url)
        {
            Console.WriteLine("Client.Post ==> URL: " + url);
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    using (var client = new HttpClient())
                    {
                        request.Headers.Add("User-Agent", Conf.Config.USER_AGENT);
                        await SetAuth(request);
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

        public async Task<CallRet> CallWithBinaryAsync(string url, HttpContent content)
        {
            Console.WriteLine("Client.PostWithBinary ==> URL: {0}", url);
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    using (var client = new HttpClient())
                    {
                        request.Headers.Add("User-Agent", Conf.Config.USER_AGENT);
                        request.Content = content;
                        await SetAuth(request);
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