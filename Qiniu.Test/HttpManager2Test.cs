using Qiniu.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace QiniuTest
{


    /// <summary>
    ///This is a test class for HttpManager2Test and is intended
    ///to contain all HttpManager2Test Unit Tests
    ///</summary>
    [TestClass()]
    public class HttpManager2Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for reqGet
        ///</summary>
        [TestMethod()]
        public void reqGetTest()
        {
            HttpManager target = new HttpManager();
            string url = "http://ip.taobao.com/service/getIpInfo.php?ip=100.123.199.44";
            Dictionary<string, string> pHeaders = new Dictionary<string, string>();
            pHeaders.Add("X-Reqid", "TestReqId");
            CompletionHandler pCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(respInfo.StatusCode, 200);
            });
            target.get(url, pHeaders, pCompletionHandler);
        }

        /// <summary>
        ///A test for reqPost
        ///</summary>
        [TestMethod()]
        public void reqPostTest()
        {
            HttpManager target = new HttpManager();
            string pUrl = "http://ip.taobao.com/service/getIpInfo.php";
            Dictionary<string, string> pHeaders = new Dictionary<string, string>();
            Dictionary<string, string[]> pPostParams = new Dictionary<string, string[]>();
            pPostParams.Add("ip", new string[] { "100.123.199.44", "100.123.199.45" });

            CompletionHandler pCompletionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(respInfo.StatusCode, 200);
            });
            target.postForm(pUrl, pHeaders, pPostParams, pCompletionHandler);
        }
    }
}
