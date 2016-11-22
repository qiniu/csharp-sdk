using Qiniu.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Util;
using Qiniu.Http;
using System.IO;

namespace QiniuTest
{


    /// <summary>
    ///This is a test class for UploadManagerTest and is intended
    ///to contain all UploadManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UploadManagerTest
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
        ///A test for uploadFile
        ///</summary>
        [TestMethod()]
        public void uploadFileTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            UploadManager target = new UploadManager();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_UploadManagerUploadFile.png";
            string filePath = "F:\\test.png";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);
            UploadOptions uploadOptions = null;

            UpCompletionHandler upCompletionHandler = new UpCompletionHandler(delegate (string fileKey, ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(200, respInfo.StatusCode);
            });
            target.uploadFile(filePath, key, token, uploadOptions, upCompletionHandler);
        }

        [TestMethod()]
        public void uploadStreamTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            UploadManager target = new UploadManager();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_UploadManagerUploadStream.png";
            string filePath = "F:\\test.png";

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);
            UploadOptions uploadOptions = null;

            UpCompletionHandler upCompletionHandler = new UpCompletionHandler(delegate (string fileKey, ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(200, respInfo.StatusCode);
            });
            target.uploadStream(fs, key, token, uploadOptions, upCompletionHandler);
        }
    }
}
