using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestFixture]
    public class HttpManagerTest:QiniuTestEnvars
    {
        private string testUrl;
        [SetUp]
        public void Init()
        {
            this.testUrl = "http://devtools.qiniu.com/qiniu.png";
        }

        [Test]
        public void GetTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = target.Get(testUrl, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = target.Post(testUrl, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostDataTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Test data");
            HttpResult result = target.PostData(testUrl, data, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostJsonTest()
        {
            HttpManager target = new HttpManager();
            string json = "{ \"Name\":\"Tester\"}";
            HttpResult result = target.PostJson(testUrl, json, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostTextTest()
        {
            HttpManager target = new HttpManager();
            string text = "Hello world";
            HttpResult result = target.PostText(testUrl, text, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostFormTest()
        {
            HttpManager target = new HttpManager();
            Dictionary<string, string> kvd = new Dictionary<string, string>();
            kvd.Add("TestKey", "TestValue");
            HttpResult result = target.PostForm(testUrl, kvd, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }

        [Test]
        public void PostMultipartTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Hello world");
            string boundary = "BOUNDARY";
            HttpResult result = target.PostMultipart(testUrl,data,boundary, null);
            //Assert.AreEqual((int)HttpCode.OK, result.Code);
            Assert.AreNotEqual((int)HttpCode.USER_EXCEPTION, result.Code);
        }
    }
}