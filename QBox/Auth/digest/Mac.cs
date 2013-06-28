using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using QBox.Conf;
using QBox.Util;

namespace QBox.Auth.digest
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
            get { return Conf.Config.Encoding.GetBytes(Config.ACCESS_KEY); }
            set { secretKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string _sign(byte[] data)
        {
              HMACSHA1 hmac = new HMACSHA1(SecretKey);
            byte[] digest = hmac.ComputeHash(data);
            return Base64URLSafe.Encode(digest);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Sign(byte[] data)
        {
            return string.Format("{0}:{1}",this.accessKey,_sign(data));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string SignWithData(byte[] b)
        {         
            string data = Base64URLSafe.Encode(b);
            return string.Format("{0}:{1}:{2}",this.accessKey,data);
        }
       // public string SignRequest(string path,string )
    }
}
