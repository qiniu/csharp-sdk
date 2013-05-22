using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using QBox.Conf;
using QBox.Util;

namespace QBox.Auth
{
    public static class AuthToken
    {
        public static byte[] Make(string scope)
        {
            byte[] accessKey = Config.Encoding.GetBytes(Config.ACCESS_KEY);
            byte[] secretKey = Config.Encoding.GetBytes(Config.SECRET_KEY);
            byte[] token = null;
            try
            {
                byte[] encodedScope = Config.Encoding.GetBytes(Base64URLSafe.Encode(scope));
                byte[] digestScope = null;
                using (HMACSHA1 hmac = new HMACSHA1(secretKey))
                {
                    byte[] digest = hmac.ComputeHash(encodedScope);
                    digestScope = Config.Encoding.GetBytes(Base64URLSafe.Encode(digest));
                }
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(accessKey, 0, accessKey.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(digestScope, 0, digestScope.Length);
                    buffer.WriteByte((byte)':');
                    buffer.Write(encodedScope, 0, encodedScope.Length);
                    token = buffer.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return token;
        }
    }
}
