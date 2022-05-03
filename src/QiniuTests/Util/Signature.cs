using System.Collections.Specialized;
using System.Text;
using NUnit.Framework;
using Qiniu.Tests;
using Qiniu.Util;

namespace QiniuTests.Util
{
    [TestFixture]
    public class SignatureTest : TestEnv
    {
        static Mac mac = new Mac("ak", "sk");
        static Signature sign = new Signature(mac);

        [TestCaseSource(typeof(SignatureV2DataClass), nameof(SignatureV2DataClass.TestCases))]
        public string SignatureV2Test(string method, string url, StringDictionary headers, string body)
        {
            return string.Format("Qiniu {0}", sign.SignRequestV2(method, url, headers, body));
        }
        
        [TestCaseSource(typeof(SignatureV2DataClass), nameof(SignatureV2DataClass.TestCases))]
        public string SignatureV2ByBytesTest(string method, string url, StringDictionary headers, string body)
        {
            return string.Format("Qiniu {0}", sign.SignRequestV2(method, url, headers, Encoding.UTF8.GetBytes(body)));
        }
    }
}