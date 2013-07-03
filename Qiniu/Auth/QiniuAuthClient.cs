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
        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            byte[] secretKey = Config.Encoding.GetBytes(Config.SECRET_KEY);
            using (HMACSHA1 hmac = new HMACSHA1(secretKey))
            {
                string pathAndQuery = request.Address.PathAndQuery;
                byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    if (request.ContentType == "application/x-www-form-urlencoded" && body != null)
                    {
                        if (!body.CanSeek)
                        {
                            throw new Exception("stream can not seek");
                        }
                        Util.IO.Copy(buffer, body);
                        body.Seek(0, SeekOrigin.Begin);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = Base64URLSafe.Encode(digest);
                    string authHead = "Qiniu " + Config.ACCESS_KEY + ":" + digestBase64;
                    request.Headers.Add("Authorization", authHead);
                }
            }
        }
    }
}
