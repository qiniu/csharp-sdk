using System;
using System.IO;
using Qiniu.RPC;
using Qiniu.Conf;
using Qiniu.Auth.digest;
#if ABOVE45
using System.Net.Http;
using System.Threading.Tasks;
#else
using System.Net;
#endif

namespace Qiniu.Auth
{
	public class QiniuAuthClient : Client
	{
		protected Mac mac;

		public QiniuAuthClient (Mac mac = null)
		{
			this.mac = mac == null ? new Mac () : mac;
		}

#if !ABOVE45
		private string SignRequest (System.Net.HttpWebRequest request, byte[] body)
		{
            return this.mac.SignRequest(request, body);
		}

        public override void SetAuth (HttpWebRequest request, Stream body)
		{
			string pathAndQuery = request.Address.PathAndQuery;
			byte[] pathAndQueryBytes = Config.Encoding.GetBytes (pathAndQuery);
			using (MemoryStream buffer = new MemoryStream()) {
				string digestBase64 = null;
				if (request.ContentType == "application/x-www-form-urlencoded" && body != null) {
					if (!body.CanSeek) {
						throw new Exception ("stream can not seek");
					}
					Util.IO.Copy (buffer, body);
					digestBase64 = SignRequest (request, buffer.ToArray());
				} else {
					buffer.Write (pathAndQueryBytes, 0, pathAndQueryBytes.Length);
					buffer.WriteByte ((byte)'\n');
					digestBase64 = mac.Sign (buffer.ToArray ());
				}
				string authHead = "QBox " + digestBase64;
				request.Headers.Add ("Authorization", authHead);
			}
		}
#else
        private string SignRequest(HttpRequestMessage request, byte[] body)
        {
            return this.mac.SignRequest(request, body);
        }

        public override async Task SetAuth(HttpRequestMessage request)
        {
            string pathAndQuery = request.RequestUri.PathAndQuery;
            byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
            using (MemoryStream buffer = new MemoryStream())
            {
                string digestBase64 = null;
                if (request.Content != null && request.Content.Headers.ContentType.MediaType == "application/x-www-form-urlencoded")
                {
                    var bytes = await request.Content.ReadAsByteArrayAsync();
                    digestBase64 = SignRequest(request, bytes);
                }
                else
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    digestBase64 = mac.Sign(buffer.ToArray());
                }
                string authHead = "QBox " + digestBase64;
                request.Headers.Add("Authorization", authHead);
            }
        }
#endif
    }
}
