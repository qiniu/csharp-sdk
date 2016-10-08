using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Storage;
using Qiniu.Util;
using Qiniu.Storage.Model;

namespace QiniuTest
{
    /// <summary>
    /// Test class of BucketManager
    /// </summary>
    [TestClass]
    public class BucketManagerTest
    {
        /// <summary>
        /// get/set
        /// </summary>
        public TestContext Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Test method of BucketManager
        /// </summary>
        [TestMethod]
        public void bktMgrTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            string testResUrl = "http://test.fengyh.cn/qiniu/files/hello.txt";
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager target = new BucketManager(mac);

            target.fetch(testResUrl, Settings.Bucket, "test_BucketManager.txt");

            target.stat(Settings.Bucket, "test_BucketManager.txt");

            target.copy(Settings.Bucket, "test_BucketManager.txt", Settings.Bucket, "copy_BucketManager.txt", true);

            target.move(Settings.Bucket, "copy_BucketManager.txt", Settings.Bucket, "move_BucketManager.txt", true);

            target.delete(Settings.Bucket, "test_BucketManager.txt");

            DomainsResult domainsResult = target.domains(Settings.Bucket);

            BucketsResult bucketsResult = target.buckets();
        }
    }
}
