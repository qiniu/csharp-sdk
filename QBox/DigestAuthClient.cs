using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace QBox
{
    public class DigestAuthClient : Client
    {
        public override void SetAuth(HttpWebRequest request)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(Config.SECRET_KEY);
            using (HMACSHA1 hmac = new HMACSHA1(secretKey))
            {
                string pathAndQuery = request.Address.PathAndQuery;
                byte[] bytesIn = Encoding.ASCII.GetBytes(pathAndQuery + "\n");
                byte[] digest = hmac.ComputeHash(bytesIn);
                string digestBase64 = Base64UrlSafe.Encode(digest);

                string authHead = "QBox " + Config.ACCESS_KEY + ":" + digestBase64;
                request.Headers.Add("Authorization", authHead);
            }
        }
    }
}
