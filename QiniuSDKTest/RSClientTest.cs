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
            Scope scope = new Scope(Bucket, LocalKey); 
            Entry actual;
            actual = target.Stat(scope);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Hash), "StatTest Failure");
            //NO
            scope = new Scope("notexsit", "errorkey");
            actual = target.Stat(scope);
            Assert.IsTrue(string.IsNullOrEmpty(actual.Hash), "StatTest Faliure");

        }

        /// <summary>
        ///Move 的测试
        ///</summary>
        [TestMethod()]
        public void MoveTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair pathPair = new EntryPathPair(Bucket, LocalKey, NewKey); ; // TODO: 初始化为适当的值
            CallRet actual;
            //YES
            actual = target.Move(pathPair);
            Assert.IsTrue(actual.OK, "MoveTest Failure");          
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope scope = new Scope(Bucket,LocalKey); // TODO: 初始化为适当的值       
            CallRet actual;
            actual = target.Delete(scope);
            Assert.IsTrue(actual.OK, "DeleteTest Failure");            
        }

        /// <summary>
        ///Copy 的测试
        ///</summary>
        [TestMethod()]
        public void CopyTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair pathPair = new EntryPathPair(Bucket, LocalKey, NewKey); // TODO: 初始化为适当的值
            CallRet actual;
            actual = target.Copy(pathPair);
            Assert.IsTrue(actual.OK, "CopyTest Failure");   
        }

        /// <summary>
        ///BatchStat 的测试
        ///</summary>
        [TestMethod()]
        public void BatchStatTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope[] keys = new Scope[2]; // TODO: 初始化为适当的值
            keys[0] = new Scope(Bucket, LocalKey);
            keys[1] = new Scope("xxx", "xxx");//error params
            List<BatchRetItem> actual;
            actual = target.BatchStat(keys);
            Assert.IsTrue(actual.Count > 0, "BatchStatTest Failure");
        }

        /// <summary>
        ///BatchMove 的测试
        ///</summary>
        [TestMethod()]
        public void BatchMoveTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
            string tmpKey = NewKey;
            entryPathPairs[0] = new EntryPathPair(Bucket, LocalKey, tmpKey);
            entryPathPairs[1] = new EntryPathPair(Bucket, tmpKey, LocalKey);

            CallRet actual;
            actual = target.BatchMove(entryPathPairs);
            Assert.IsTrue(actual.OK, "BatchMoveTest Failure");
        }

        /// <summary>
        ///BatchDelete 的测试
        ///</summary>
        [TestMethod()]
        public void BatchDeleteTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            Scope[] keys = new Scope[2]; // TODO: 初始化为适当的值
            keys[0] = new Scope(Bucket, LocalKey);
            keys[1] = new Scope("xxx", "xxx");//error params
            CallRet actual;
            actual = target.BatchDelete(keys);            
            Assert.IsTrue(actual.OK, "BatchStatTest Failure"); ;
        }

        /// <summary>
        ///BatchCopy 的测试
        ///</summary>
        [TestMethod()]
        public void BatchCopyTest()
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值

            EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
            string tmpKey = NewKey;
            entryPathPairs[0] = new EntryPathPair(Bucket, LocalKey, tmpKey);
            entryPathPairs[1] = new EntryPathPair(Bucket, tmpKey, NewKey);            
            CallRet actual;
            actual = target.BatchCopy(entryPathPairs);
            Assert.IsTrue(actual.OK, "BatchStatTest Failure"); ;
        }
    }
}
