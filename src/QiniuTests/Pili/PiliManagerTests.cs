using NUnit.Framework;
using Qiniu.Http;
using Qiniu.Pili;
using Qiniu.Tests;
using Qiniu.Util;
using System;

namespace QiniuTests.Pili
{
    [TestFixture]
    public class PiliManagerTests : TestEnv
    {
        [Test]
        public void SaveAsTest()
        {
            Mac mac = new Mac("S6ziV-VtPwBs0Ytg2G3UAgK7cZHvZBYhSIPdZ0uc", "kOAl_KmCdV7R5Wkj8G1U0HCiVuExD1b2vZolDaVr");
            PiliManager manager = new PiliManager(mac, "rtn-e57vvyeev", "test");

            string fname = "test1";
            string format = "mp4";

            SaveAsResult ret = manager.SaveAs(fname: fname, format: format);
            if (ret.Code != (int)HttpCode.OK)
            {
                Assert.Fail(ret.ToString());
            }

            Console.WriteLine(ret.ToString());
        }
    }
}