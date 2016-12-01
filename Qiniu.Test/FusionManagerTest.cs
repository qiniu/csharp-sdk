using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.Fusion;
using Qiniu.Fusion.Model;
using Qiniu.Util;

namespace QiniuTest
{
    /// <summary>
    /// Test class of BucketManager
    /// </summary>
    [TestClass]
    public class FusionManagerTest
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
        public void fusionMgrTest()
        {
            //Settings.load();
            Settings.LoadFromFile();
            string testResUrl = "http://test.fengyh.cn/qiniu/files/hello.txt";
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            FusionManager target = new FusionManager(mac);

            //      
        }
    }
}
