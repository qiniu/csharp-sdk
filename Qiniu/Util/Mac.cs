using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Qiniu.Util
{
    public class Mac
    {
        public string AccessKey { set; get; }
        public string SecretKey { set; get; }
        public Mac(string accessKey, string secretKey)
        {
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
        }

        private string _sign(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(this.SecretKey));
            byte[] digest = hmac.ComputeHash(data);
            return StringUtils.urlSafeBase64Encode(digest);
        }

        public string Sign(byte[] data)
        {
            return string.Format("{0}:{1}", this.AccessKey, this._sign(data));
        }

        public string SignWithData(byte[] data)
        {
            string encodedData = StringUtils.urlSafeBase64Encode(data);
            return string.Format("{0}:{1}:{2}", this.AccessKey, this._sign(Encoding.UTF8.GetBytes(encodedData)), encodedData);
        }

        public string SignRequest(string url, byte[] reqBody)
        {
            Uri u = new Uri(url);
            using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(this.SecretKey)))
            {
                string pathAndQuery = u.PathAndQuery;
                byte[] pathAndQueryBytes = Encoding.UTF8.GetBytes(pathAndQuery);
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    if (reqBody!=null && reqBody.Length > 0)
                    {
                        buffer.Write(reqBody, 0, reqBody.Length);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = StringUtils.urlSafeBase64Encode(digest);
                    return string.Format("{0}:{1}", this.AccessKey, digestBase64);
                }
            }
        }
    }
}