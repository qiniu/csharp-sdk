#if LOCAL_TEST

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Text;
using Qiniu.Http;

namespace Qiniu.UnitTest
{
    [TestClass]
    public class HttpManagerTest:QiniuTestEnvars
    {
        [TestMethod]
        public async Task GetTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = await target.GetAsync(TestURL1, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostTest()
        {
            HttpManager target = new HttpManager();
            HttpResult result = await target.PostAsync(TestURL1, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostDataTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Test data");
            HttpResult result = await target.PostDataAsync(TestURL1, data, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostJsonTest()
        {
            HttpManager target = new HttpManager();
            string json = "{ \"Name\":\"Tester\"}";
            HttpResult result = await target.PostJsonAsync(TestURL1, json, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostTextTest()
        {
            HttpManager target = new HttpManager();
            string text = "Hello world";
            HttpResult result = await target.PostTextAsync(TestURL1, text, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostFormTest()
        {
            HttpManager target = new HttpManager();
            Dictionary<string, string> kvd = new Dictionary<string, string>();
            kvd.Add("TestKey", "TestValue");
            HttpResult result = await target.PostFormAsync(TestURL1, kvd, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }

        [TestMethod]
        public async Task PostMultipartTest()
        {
            HttpManager target = new HttpManager();
            byte[] data = Encoding.UTF8.GetBytes("Hello world");
            string boundary = "BOUNDARY";
            HttpResult result = await target.PostMultipartAsync(TestURL1,data,boundary, null);
            Assert.AreEqual((int)HttpCode.OK, result.Code);
        }
    }
}

#endif