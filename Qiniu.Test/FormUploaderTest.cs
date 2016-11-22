using Qiniu.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Qiniu.Util;
using Qiniu.Http;

namespace QiniuTest
{    
    /// <summary>
    ///This is a test class for FormUploaderTest and is intended
    ///to contain all FormUploaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FormUploaderTest
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
        ///A test for uploadData
        ///</summary>
        [TestMethod()]
        public void uploadDataTest()
        {
            //Settings.load();
            Settings.LoadFromFile("F:\\test.cfg");
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FormUploader target = new FormUploader();
            byte[] data = Encoding.UTF8.GetBytes("hello world");
            string key = "test_FormUploaderUploadData.txt";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1; 
            string token = Auth.createUploadToken(putPolicy, mac);
            UploadOptions uploadOptions = null;
            UpCompletionHandler upCompletionHandler = new UpCompletionHandler(delegate(string fileKey, ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(200, respInfo.StatusCode);
            });
            target.uploadData(data, key, token, uploadOptions, upCompletionHandler);
        }

        /// <summary>
        ///A test for uploadFile
        ///</summary>
        [TestMethod()]
        public void uploadFileTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            string key = "test_FormUploaderUploadFile.png";
          
            FormUploader target = new FormUploader();
            string filePath = "F:\\test.png";

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Settings.Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.createUploadToken(putPolicy, mac);
            UploadOptions uploadOptions = null;

            UpCompletionHandler upCompletionHandler = new UpCompletionHandler(delegate(string fileKey, ResponseInfo respInfo, string response)
            {
                Assert.AreEqual(200, respInfo.StatusCode);
            });
            target.uploadFile(filePath, key, token, uploadOptions, upCompletionHandler);
        }
    }
}
