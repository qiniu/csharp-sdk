using System;
using System.Collections.Specialized;
using System.Net;
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
            
            HttpWebRequest wReq = reqOpts.CreateHttpWebRequest();
            Assert.AreEqual("https://qiniu.com/index.html", wReq.Address.ToString());
            wReq.Abort();

            reqOpts.Url = "https://www.qiniu.com/index.html";
            wReq = reqOpts.CreateHttpWebRequest();
            Assert.AreEqual("https://www.qiniu.com/index.html", wReq.Address.ToString());
            wReq.Abort();
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

            HttpWebRequest wReq = reqOpts.CreateHttpWebRequest();
            Assert.AreEqual(false, wReq.AllowAutoRedirect); // default true
            Assert.AreEqual(true, wReq.AllowReadStreamBuffering); // default false
            Assert.AreEqual(false, wReq.AllowWriteStreamBuffering); // default true
            // Assert.AreEqual(, wReq.AutomaticDecompression); // default Null 
            // Assert(, wReq.CachePolicy); // default Null
            // Assert(, wReq.ClientCertificates); // default System.Security.Cryptography.X509Certificates.X509CertificateCollection
            Assert.AreEqual("qngroup", wReq.ConnectionGroupName); // default ""
            // Assert(, wReq.ContinueDelegate); // default Null
            Assert.AreEqual(360, wReq.ContinueTimeout); // default 350
            // Assert(, wReq.CookieContainer); // default Null
            // Assert(, wReq.Credentials); // default Null
            // Assert(, wReq.ImpersonationLevel); // default Null
            Assert.AreEqual(false, wReq.KeepAlive); // default true
            Assert.AreEqual(10, wReq.MaximumAutomaticRedirections); // default 50
            Assert.AreEqual(32, wReq.MaximumResponseHeadersLength); // default 64
            Assert.AreEqual("video/mp4", wReq.MediaType); // default ""
            Assert.AreEqual("POST", wReq.Method); // default "GET"
            Assert.AreEqual(false, wReq.Pipelined); // default true
            Assert.AreEqual(true, wReq.PreAuthenticate); // default false
            // Assert(, wReq.Proxy); // default System.Net.SystemWebProxy;
            Assert.AreEqual(200000, wReq.ReadWriteTimeout); // default 300000
            Assert.AreEqual(true, wReq.SendChunked); // default false
            // Assert(, wReq.ServerCertificateValidationCallback); // default Null
            Assert.AreEqual(50000, wReq.Timeout); // default 100000
            Assert.AreEqual(true, wReq.UnsafeAuthenticatedConnectionSharing); // default false
            Assert.AreEqual(true, wReq.UseDefaultCredentials); // default false
            
            wReq.Abort();
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

            HttpWebRequest wReq = reqOpts.CreateHttpWebRequest();
            Assert.AreEqual("text/plain", wReq.Accept);
            Assert.AreEqual("text/plain", wReq.ContentType);
            Assert.AreEqual(DateTime.Parse("Wed, 03 Aug 2011 04:00:00 GMT"), wReq.Date);
            Assert.AreEqual("200-ok", wReq.Expect);
            Assert.AreEqual("qiniu.com", wReq.Host);
            Assert.AreEqual(DateTime.Parse("Wed, 03 Aug 2011 04:00:00 GMT"), wReq.IfModifiedSince);
            Assert.AreEqual("https://qiniu.com/", wReq.Referer);
            Assert.AreEqual("gzip", wReq.TransferEncoding);
            Assert.AreEqual("qn-csharp-sdk", wReq.UserAgent);
            Assert.AreEqual("qn", wReq.Headers["X-Qiniu-A"]);

            wReq.Abort();
        }
    }
}