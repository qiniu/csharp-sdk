using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Qiniu.Util;

namespace Qiniu.Common
{
    /// <summary>
    /// 签名/加密
    /// </summary>
    public class Signature
    {
        private Mac mac;

        public Signature(Mac mac)
        {
            this.mac = mac;
        }

        private string encodedSign(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(mac.SecretKey));
            byte[] digest = hmac.ComputeHash(data);
            return StringHelper.UrlSafeBase64Encode(digest);
        }

        private string encodedSign(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return encodedSign(data);
        }

        public string Sign(byte[] data)
        {
            return string.Format("{0}:{1}", mac.AccessKey,encodedSign(data));
        }

        public string Sign(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return Sign(data);
        }

        public string SignWithData(byte[] data)
        {
            string sstr = StringHelper.UrlSafeBase64Encode(data);
            return string.Format("{0}:{1}:{2}", mac.AccessKey, encodedSign(sstr), sstr);
        }

        public string SignWithData(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return SignWithData(data);
        }

        public string SignRequest(string url, byte[] body)
        {
            Uri u = new Uri(url);
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(mac.SecretKey)))
            {
                string pathAndQuery = u.PathAndQuery;
                byte[] pathAndQueryBytes = Encoding.UTF8.GetBytes(pathAndQuery);
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    if (body != null && body.Length > 0)
                    {
                        buffer.Write(body, 0, body.Length);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = StringHelper.UrlSafeBase64Encode(digest);
                    return string.Format("{0}:{1}", mac.AccessKey, digestBase64);
                }
            }
        }

        public string SignRequest(string url, string body)
        {
            byte[] data = Encoding.UTF8.GetBytes(body);
            return SignRequest(url, data);
        }
    }
}
