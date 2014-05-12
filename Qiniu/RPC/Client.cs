using System;
using System.IO;
using System.Net;

namespace Qiniu.RPC
{
	public class Client
	{       
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

        /// <summary>
        /// 调用Get方法, 比如有saveas功能(管道)的地址可以用Get来调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isNeedAuth">需要验证权限</param>
        /// <param name="dontEscape">取消escape Url</param>
        /// <returns></returns>
        public CallRet Get(string url, bool isNeedAuth = false, bool dontEscape = false)
        {
            Console.WriteLine("Client.Get ==> URL: " + url);
            try
            {
                var newUrl = new Uri(url, dontEscape);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newUrl);
                request.UserAgent = Conf.Config.USER_AGENT;
                request.Method = "GET";
                if (isNeedAuth)
                {
                    SetAuth(request, null);
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
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