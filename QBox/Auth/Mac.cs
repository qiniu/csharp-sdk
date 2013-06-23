using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBox.Auth.digest
{
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
            set { secretKey = value; }
        }
    }
}
