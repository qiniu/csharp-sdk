using System;
using System.Collections.Specialized;
using System.Reflection;
using NUnit.Framework;
using Qiniu.Http;
using Qiniu.Util;

namespace QiniuTests.Http
{
    [TestFixture]
    public class HttpManagerTests
    {
        private static HttpManager httpManager = new HttpManager();
        private static MethodInfo dynMethod = httpManager.GetType().GetMethod("addAuthHeaders", 
             BindingFlags.NonPublic | BindingFlags.Instance);
        private static Mac mac = new Mac("ak", "sk");

        [TearDown]
        public void EachTeardown()
        {
            Environment.SetEnvironmentVariable("DISABLE_QINIU_TIMESTAMP_SIGNATURE", null);
        }

        [Test]
        public void DisableQiniuTimestampSignatureDefaultTest()
        {
            StringDictionary headers = new StringDictionary();
            Auth auth = new Auth(mac);
            dynMethod.Invoke(httpManager, new object[] { headers, auth });

            Assert.True(headers.ContainsKey("X-Qiniu-Date"));
        }
        
        [Test]
        public void DisableQiniuTimestampSignatureTest()
        {
            StringDictionary headers = new StringDictionary();
            Auth auth = new Auth(mac, new AuthOptions
            {
                DisableQiniuTimestampSignature = true
            });
            dynMethod.Invoke(httpManager, new object[] { headers, auth });

            Assert.False(headers.ContainsKey("X-Qiniu-Date"));
        }
        
        [Test]
        public void DisableQiniuTimestampSignatureEnvTest()
        {
            Environment.SetEnvironmentVariable("DISABLE_QINIU_TIMESTAMP_SIGNATURE", "true");

            StringDictionary headers = new StringDictionary();
            Auth auth = new Auth(mac);
            dynMethod.Invoke(httpManager, new object[] { headers, auth });

            Assert.False(headers.ContainsKey("X-Qiniu-Date"));
        }
        
        [Test]
        public void DisableQiniuTimestampSignatureEnvBeIgnoredTest()
        {
            Environment.SetEnvironmentVariable("DISABLE_QINIU_TIMESTAMP_SIGNATURE", "true");

            StringDictionary headers = new StringDictionary();
            Auth auth = new Auth(mac, new AuthOptions
            {
                DisableQiniuTimestampSignature = false
            });
            dynMethod.Invoke(httpManager, new object[] { headers, auth });

            Assert.True(headers.ContainsKey("X-Qiniu-Date"));
        }
    }
}