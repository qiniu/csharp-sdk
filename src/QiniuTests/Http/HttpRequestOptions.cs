using System;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using Qiniu.Http;

namespace QiniuTests.Http
{
    [TestFixture]
    public class HttpRequestOptionsTests
    {
        [Test]
        public void SetUrlTest()
        {
            HttpRequestOptions reqOpts = new HttpRequestOptions();
            reqOpts.Url = "https://qiniu.com/index.html";
            reqOpts.Method = "GET";
            
            var request = reqOpts.CreateHttpRequestMessage();
            Assert.AreEqual("https://qiniu.com/index.html", request.RequestUri?.ToString());

            reqOpts.Url = "https://www.qiniu.com/index.html";
            request = reqOpts.CreateHttpRequestMessage();
            Assert.AreEqual("https://www.qiniu.com/index.html", request.RequestUri?.ToString());
        }
        
        [Test]
        public void SetPropertiesTest()
        {
            HttpRequestOptions reqOpts = new HttpRequestOptions();
            reqOpts.Url = "https://qiniu.com/index.html";

            // some items are hard to set and test, skipped.
            reqOpts.AllowAutoRedirect = false; // default true
            reqOpts.AllowReadStreamBuffering = true; // default false
            reqOpts.AllowWriteStreamBuffering = false; // default true
            // reqOpts.AutomaticDecompression = ;
            // reqOpts.CachePolicy =;
            // reqOpts.ClientCertificates = System.Security.Cryptography.X509Certificates.X509CertificateCollection;
            reqOpts.ConnectionGroupName = "qngroup"; // default ""
            // reqOpts.ContinueDelegate =;
            reqOpts.ContinueTimeout = 360; // default 350
            // reqOpts.CookieContainer =;
            // reqOpts.Credentials =;
            // reqOpts.ImpersonationLevel = Delegation;
            reqOpts.KeepAlive = false; // default true
            reqOpts.MaximumAutomaticRedirections = 10; // default 50
            reqOpts.MaximumResponseHeadersLength = 32; // default 64
            reqOpts.MediaType = "video/mp4"; // default ""
            reqOpts.Method = "POST"; // default "GET"
            reqOpts.Pipelined = false; // default true
            reqOpts.PreAuthenticate = true; // default false
            // reqOpts.Proxy = System.Net.SystemWebProxy;
            reqOpts.ReadWriteTimeout = 200000; // default 300000
            reqOpts.SendChunked = true; // default false
            // reqOpts.ServerCertificateValidationCallback =;
            reqOpts.Timeout = 50000; // default 100000
            reqOpts.UnsafeAuthenticatedConnectionSharing = true; // default false
            reqOpts.UseDefaultCredentials = true; // default false

            var handler = reqOpts.CreateHttpClientHandler();
            var request = reqOpts.CreateHttpRequestMessage();

            Assert.AreEqual(false, handler.AllowAutoRedirect);
            Assert.AreEqual(true, handler.PreAuthenticate);
            Assert.AreEqual(true, handler.UseDefaultCredentials);
            Assert.AreEqual("POST", request.Method.Method);
        }

        [Test]
        public void SetHeadersTest()
        {
            HttpRequestOptions reqOpts = new HttpRequestOptions();
            reqOpts.Url = "https://qiniu.com/index.html";

            reqOpts.Headers = new StringDictionary
            {
                { "Accept", "text/plain" },
                { "content-Type", "text/plain" },
                { "date", "Wed, 03 Aug 2011 04:00:00 GMT" },
                { "expect", "200-ok" },
                { "host", "qiniu.com" },
                { "if-modified-since", "Wed, 03 Aug 2011 04:00:00 GMT" },
                { "referer", "https://qiniu.com/" },
                { "transfer-encoding", "gzip" },
                { "user-agent", "qn-csharp-sdk" },
                { "X-Qiniu-A", "qn" }
            };
            
            // TransferEncoding requires the SendChunked property to be set to true
            reqOpts.SendChunked = true;

            reqOpts.Method = "GET";
            var request = reqOpts.CreateHttpRequestMessage();

            Assert.That(request.Headers.Accept.Any(h => h.MediaType == "text/plain"));
            Assert.AreEqual("200-ok", request.Headers.GetValues("expect").FirstOrDefault());
            Assert.AreEqual("qn-csharp-sdk", string.Join("", request.Headers.UserAgent.Select(u => u.ToString())));
            Assert.AreEqual("qn", request.Headers.GetValues("X-Qiniu-A").FirstOrDefault());
        }
    }
}