using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace QBox
{
    public class DigestAuthClient : Client
    {
        public override void SetAuth(HttpWebRequest request, byte[] body)
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
                        buffer.Write(body, 0, body.Length);
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
