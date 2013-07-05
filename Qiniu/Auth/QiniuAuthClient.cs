using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Qiniu.Util;
using Qiniu.RPC;
using Qiniu.Conf;
using Qiniu.Auth.digest;
namespace Qiniu.Auth
{
    public class QiniuAuthClient : Client
    {
        protected Mac mac;
        public QiniuAuthClient(Mac mac = null)
        {
            this.mac = mac == null ? new Mac() : mac;
        }
        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            string pathAndQuery = request.Address.PathAndQuery;
            byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
            using (MemoryStream buffer = new MemoryStream())
            {
                string digestBase64 = null;
                if (request.ContentType == "application/x-www-form-urlencoded" && body != null)
                {
                    if (!body.CanSeek)
                    {
                        throw new Exception("stream can not seek");
                    }
                    Util.IO.Copy(buffer, body);
                    body.Seek(0, SeekOrigin.Begin);
                    digestBase64 = mac.SignRequest(request, buffer.ToArray());
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
    }
}
