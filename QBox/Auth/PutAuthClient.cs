using System;
using System.Net;
using System.IO;
using QBox.RS;

namespace QBox.Auth
{
    public class PutAuthClient : Client
    {
        public string UpToken { get; set; }

        public PutAuthClient(string upToken)
        {
            UpToken = upToken;
        }

        public override void SetAuth(HttpWebRequest request, Stream body)
        {
            string authHead = "UpToken " + UpToken;
            request.Headers.Add("Authorization", authHead);
        }
    }
}
