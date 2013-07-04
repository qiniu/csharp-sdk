using Qiniu.RS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Qiniu.RPC;
using System.Collections.Generic;

namespace QiniuSDKTest
{
    
    
    /// <summary>
    ///这是 RSClientTest 的测试类，旨在
    ///包含所有 RSClientTest 单元测试
    ///</summary>
    [TestClass()]
    public class RSClientTest:Test
    {


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
        ///Stat 的测试
        ///</summary>
        [TestMethod()]
        public void StatTest()
        {
            RSClient target = new RSClient(); 
            //YES
            Scope scope = new Scope("icattlecoder3", "3695df2e-304a-4af2-a401-74e791c05cf4"); ; 
            Entry actual;
            actual = target.Stat(scope);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Hash), "Failure");
            //NO
            scope = new Scope("notexsit", "errorkey");
            actual = target.Stat(scope);
            Assert.IsTrue(string.IsNullOrEmpty(actual.Hash), "Faliure");

        }

        /// <summary>
        ///Move 的测试
        ///</summary>
        [TestMethod()]
        public void MoveTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair pathPair = new EntryPathPair("icattlecoder3", "moveTest", "moveTest2"); ; // TODO: 初始化为适当的值
            CallRet actual;
            //YES
            actual = target.Move(pathPair);
            Assert.IsTrue(actual.OK, "Move Failure");
            //NO
            pathPair = new EntryPathPair("icattlecoder3", "moveTest2", "moveTest"); // TODO: 初始化为适当的值
            actual = target.Move(pathPair);
            Assert.IsNotNull(actual.OK, "Move Failure");

        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope scope = new Scope("icattlecoder3", "moveTest"); // TODO: 初始化为适当的值
       
            CallRet actual;
            actual = target.Delete(scope);
            Assert.IsTrue(actual.OK, "Delete Failure");

            scope = new Scope("icattlecoder3", "moveTest");
            Assert.IsTrue(!actual.OK, "Delete Failure");
        }

        /// <summary>
        ///Copy 的测试
        ///</summary>
        [TestMethod()]
        public void CopyTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair pathPair = null; // TODO: 初始化为适当的值
            CallRet expected = null; // TODO: 初始化为适当的值
            CallRet actual;
            actual = target.Copy(pathPair);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchStat 的测试
        ///</summary>
        [TestMethod()]
        public void BatchStatTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope[] keys = null; // TODO: 初始化为适当的值
            List<BatchRetItem> expected = null; // TODO: 初始化为适当的值
            List<BatchRetItem> actual;
            actual = target.BatchStat(keys);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchMove 的测试
        ///</summary>
        [TestMethod()]
        public void BatchMoveTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair[] entryPathPairs = null; // TODO: 初始化为适当的值
            CallRet expected = null; // TODO: 初始化为适当的值
            CallRet actual;
            actual = target.BatchMove(entryPathPairs);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchDelete 的测试
        ///</summary>
        [TestMethod()]
        public void BatchDeleteTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope[] keys = null; // TODO: 初始化为适当的值
            CallRet expected = null; // TODO: 初始化为适当的值
            CallRet actual;
            actual = target.BatchDelete(keys);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchCopy 的测试
        ///</summary>
        [TestMethod()]
        public void BatchCopyTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair[] entryPathPari = null; // TODO: 初始化为适当的值
            CallRet expected = null; // TODO: 初始化为适当的值
            CallRet actual;
            actual = target.BatchCopy(entryPathPari);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
