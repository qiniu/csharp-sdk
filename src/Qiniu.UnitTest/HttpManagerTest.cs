#if LOCAL_TEST

using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class HttpManagerTest:QiniuTestEnvars
    {
        [Test]
        public void GetTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = target.Get(TestURL1, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = target.Post(TestURL1, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostDataTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Test data");
            HttpResult result = target.PostData(TestURL1, data, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostJsonTest()
        {
            HttpManager target = new HttpManager();
            string json = "{ \"Name\":\"Tester\"}";
            HttpResult result = target.PostJson(TestURL1, json, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostTextTest()
        {
            HttpManager target = new HttpManager();
            string text = "Hello world";
            HttpResult result = target.PostText(TestURL1, text, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostFormTest()
        {
            HttpManager target = new HttpManager();
            Dictionary<string, string> kvd = new Dictionary<string, string>();
            kvd.Add("TestKey", "TestValue");
            HttpResult result = target.PostForm(TestURL1, kvd, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [Test]
        public void PostMultipartTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Hello world");
            string boundary = "BOUNDARY";
            HttpResult result = target.PostMultipart(TestURL1,data,boundary, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }
    }
}

#endif