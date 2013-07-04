using Qiniu.RSF;
using Qiniu.Conf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace QiniuSDKTest
{
    
    
    /// <summary>
    ///这是 RSFClientTest 的测试类，旨在
    ///包含所有 RSFClientTest 单元测试
    ///</summary>
    [TestClass()]
    public class RSFClientTest
    {
        
        public RSFClientTest()
        {
            
        }
        
        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        

        /// <summary>
        ///Next 的测试
        ///</summary>
        [TestMethod()]
        public void NextTest()
        {
            string bucketName = string.Empty; // TODO: 初始化为适当的值
            RSFClient target = new RSFClient(bucketName); // TODO: 初始化为适当的值
            List<DumpItem> expected = null; // TODO: 初始化为适当的值
            List<DumpItem> actual;
            actual = target.Next();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ListPrefix 的测试
        ///</summary>
        [TestMethod()]
        public void ListPrefixTest()
        {
            string bucketName = "icattlecoder3"; // TODO: 初始化为适当的值
            Console.WriteLine("sdlkfj");
            RSFClient target = new RSFClient(bucketName); // TODO: 初始化为适当的值
            string bucketName1 = string.Empty; // TODO: 初始化为适当的值
            string prefix = string.Empty; // TODO: 初始化为适当的值
            string markerIn = string.Empty; // TODO: 初始化为适当的值
            int limit = 0; // TODO: 初始化为适当的值

            DumpRet expected = null; // TODO: 初始化为适当的值
            DumpRet actual;
            actual = target.ListPrefix(bucketName1, prefix, markerIn, limit);
            Assert.IsTrue(actual.Items.Count > 0, "false");
            //Assert.AreEqual(
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
