using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using QBox.RS;
using QBox.Util;

namespace QBox.Auth
{
    public static class AuthToken
    {
        public static byte[] Make(string scope)
        {
            Encoding encoding = Encoding.ASCII;
            byte[] accessKey = encoding.GetBytes(Config.ACCESS_KEY);
            byte[] secretKey = encoding.GetBytes(Config.SECRET_KEY);
            byte[] upToken = null;
            try
            {
                byte[] policyBase64 = encoding.GetBytes(Base64UrlSafe.Encode(scope));
                byte[] digestBase64 = null;
                using (HMACSHA1 hmac = new HMACSHA1(secretKey))
                {
                    byte[] digest = hmac.ComputeHash(policyBase64);
                    digestBase64 = encoding.GetBytes(Base64UrlSafe.Encode(digest));
                }
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(accessKey, 0, accessKey.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(digestBase64, 0, digestBase64.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(policyBase64, 0, policyBase64.Length);
                    upToken = buffer.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return upToken;
        }
        
    }
}
