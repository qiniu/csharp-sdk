using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using QBox.RS;
using QBox.Util;

namespace QBox.Auth
{
    public class DigestAuthClient : Client
    {
        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(Config.SECRET_KEY);
            using (HMACSHA1 hmac = new HMACSHA1(secretKey))
            {
                string pathAndQuery = request.Address.PathAndQuery;
                byte[] pathAndQueryBytes = Encoding.ASCII.GetBytes(pathAndQuery);
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
                        StreamUtil.Copy(body, buffer);
                        body.Seek(0, SeekOrigin.Begin);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = Base64UrlSafe.Encode(digest);

                    string authHead = "QBox " + Config.ACCESS_KEY + ":" + digestBase64;
                    request.Headers.Add("Authorization", authHead);
                }
            }
        }
    }
}
