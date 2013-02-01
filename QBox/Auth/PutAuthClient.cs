using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using QBox.RS;

namespace QBox.Auth
{
    class PutAuthClient : Client
    {
        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            string authHead = "QBox " + Config.ACCESS_KEY + ":" + digestBase64;
            request.Headers.Add("Authorization", authHead);
        }
    }
}
