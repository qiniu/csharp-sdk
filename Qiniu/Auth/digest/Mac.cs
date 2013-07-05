using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Qiniu.Conf;
using Qiniu.Util;

namespace Qiniu.Auth.digest
{
    /// <summary>
    /// Message Authentication Code
    /// </summary>
    public class Mac
    {
        string accessKey;
        public string AccessKey
        {
            get { return accessKey; }
            set { accessKey = value; }
        }

        byte[] secretKey;
        public byte[] SecretKey
        {
            get { return secretKey; }
        }

        public Mac()
        {
            this.accessKey = Conf.Config.ACCESS_KEY;
            this.secretKey = Config.Encoding.GetBytes(Config.SECRET_KEY);
        }

        public Mac(string access, byte[] secretKey)
        {
            this.accessKey = access;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string _sign(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(SecretKey);
            byte[] digest = hmac.ComputeHash(data);
            return Base64URLSafe.Encode(digest);
        }
        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string Sign(byte[] b)
        {
            string data = Base64URLSafe.Encode(b);
            return string.Format("{0}:{1}", this.accessKey, _sign(b));
        }
        /// <summary>
        /// SignWithData
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string SignWithData(byte[] b)
        {
            string data = Base64URLSafe.Encode(b);
            return string.Format("{0}:{1}:{2}", this.accessKey, _sign(Config.Encoding.GetBytes(data)), data);
        }
       /// <summary>
        /// SignRequest
       /// </summary>
       /// <param name="request"></param>
       /// <param name="body"></param>
       /// <returns></returns>
        public string SignRequest(System.Net.HttpWebRequest request, byte[] body)
        {
            MemoryStream mstream = new MemoryStream();
            Uri u = request.Address;
            using (HMACSHA1 hmac = new HMACSHA1(secretKey))
            {
                string pathAndQuery = request.Address.PathAndQuery;
                byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    if (body.Length > 0)
                    {
                        buffer.Write(body, 0, body.Length);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = Base64URLSafe.Encode(digest);
                    return this.accessKey + ":" + digestBase64;
                }
            }
        }
    }
}