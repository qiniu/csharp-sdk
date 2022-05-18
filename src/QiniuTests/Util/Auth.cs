using System;
using System.Collections.Specialized;
using NUnit.Framework;
using Qiniu.Util;

namespace QiniuTests.Util
{
    [TestFixture]
    public class AuthTests
    {
        private static Mac mac = new Mac("ak", "sk");
        private static Auth auth = new Auth(mac);

        [TestCaseSource(typeof(SignatureV2DataClass), nameof(SignatureV2DataClass.TestCases))]
        public string CreateManageTokenV2Test(string method, string url, StringDictionary headers,
            string body)
        {
            return auth.CreateManageTokenV2(method, url, headers, body);
        }

        [Test]
        public void CreateManageTokenV2Test()
        {
            string actual = auth.CreateManageTokenV2("GET", "http://rs.qbox.me");
            Assert.AreEqual("Qiniu ak:bgfeAqx6xXMIXA232e8ocxfhINc=", actual);
        }
    }
}