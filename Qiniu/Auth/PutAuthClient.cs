using System.IO;
using Qiniu.RPC;
#if ABOVE45
using System.Net.Http;
using System.Threading.Tasks;
#else
using System.Net;
#endif

namespace Qiniu.Auth
{
	public class PutAuthClient : Client
	{
		public string UpToken { get; set; }

		public PutAuthClient (string upToken)
		{
			UpToken = upToken;
		}
#if !ABOVE45
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="body"></param>
        public override void SetAuth (HttpWebRequest request, Stream body)
		{
			string authHead = "UpToken " + UpToken;
			request.Headers.Add ("Authorization", authHead);
		}
#else
        public override Task SetAuth(HttpRequestMessage request)
        {
            string authHead = "UpToken " + UpToken;
            request.Headers.Add("Authorization", authHead);
            return base.SetAuth(request);
        }
#endif
    }
}
