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

        public string SignWithData(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(SecretKey);
            byte[] digest = hmac.ComputeHash(data);
            string sign = Base64URLSafe.Encode(digest);
            return null;
        }
    }
}
