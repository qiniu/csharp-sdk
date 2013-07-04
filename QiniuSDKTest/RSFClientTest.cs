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
    public class RSFClientTest:Test
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
            RSFClient target = new RSFClient(Bucket); // TODO: 初始化为适当的值
            target.Marker = string.Empty;
            target.Prefix = string.Empty;
            target.Limit = 100;
            List<DumpItem> actual;
            actual = target.Next();
            Assert.IsTrue(actual.Count > 0, "ListPrefixTest Failure");
        }

        /// <summary>
        ///ListPrefix 的测试
        ///</summary>
        [TestMethod()]
        public void ListPrefixTest()
        {
            RSFClient target = new RSFClient(Bucket); // TODO: 初始化为适当的值
            target.Marker = string.Empty;
            target.Prefix = string.Empty;
            target.Limit = 100;
            DumpRet actual;
            actual = target.ListPrefix(Bucket);
            foreach (DumpItem item in actual.Items)
            {
                Console.WriteLine("Key:{0},Hash:{1},Mime:{2},PutTime:{3},EndUser:{4}", item.Key, item.Hash, item.Mime, item.PutTime, item.EndUser);
            }
            PrintLn(actual.Items.Count.ToString());
            //error params
            Assert.IsTrue(actual.Items.Count > 0, "ListPrefixTest Failure");

        }
    }
}
