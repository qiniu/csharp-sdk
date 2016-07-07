using System;
using System.Collections.Generic;
using Qiniu.RS;
using Qiniu.RPC;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test.RS
{
    /// <summary>
    ///这是 RSClientTest 的测试类，旨在
    ///包含所有 RSClientTest 单元测试
    ///</summary>

#if NET20 || NET40
    [TestFixture]
    public class RSClientTest:QiniuTestBase
#else
    public class RSClientTest : QiniuTestBase, IDisposable
#endif
    {
		private List<string> tmpKeys=new List<string>();

#if !NET20 && !NET40
        public RSClientTest()
        {
            Task.Run(async () => await BeforeTest()).Wait();
        }

        public void Dispose()
        {
            AfterTest();
        }
#endif

#if NET20 || NET40
		[TestFixtureSetUp]
        public void BeforeTest()
		{
        #region before test
			tmpKeys = RSHelper.RSPut(Bucket,4);
        #endregion
		}
#else
        public async Task BeforeTest()
        {
            #region before test
            tmpKeys = await RSHelper.RSPut(Bucket, 4);
            #endregion
        }
#endif

#if NET20 || NET40
		[TestFixtureTearDown]
        public void AfterTest()
		{
			foreach (string k in tmpKeys) {
				RSHelper.RSDel (Bucket, k);
			}
		}
#else
        public async void AfterTest()
        {
            foreach (string k in tmpKeys)
            {
                await RSHelper.RSDel(Bucket, k);
            }
        }
#endif

        /// <summary>
        ///Stat 的测试
        ///</summary>

#if NET20 || NET40
		[Test]
        public void StatTest()
#else
        [Fact]
        public async Task StatTest()
#endif
		{
            RSClient target = new RSClient();
			//YES
		
			EntryPath scope = new EntryPath(Bucket, tmpKeys[0]); 
			Entry actual;
#if NET20 || NET40
		    actual = target.Stat(scope);
            Assert.IsTrue(actual.OK, "StatTest Failure");
#else
            actual = await target.StatAsync(scope);
            Assert.True(actual.OK, "StatTest Failure");
#endif

        }

		/// <summary>
		///Move 的测试
		///</summary>
#if NET20 || NET40
		[Test]
        public void MoveTest()
#else
        [Fact]
        public async Task MoveTest()
#endif
		{
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
			string key = NewKey;
			EntryPathPair pathPair = new EntryPathPair(Bucket, tmpKeys[0], key); ; // TODO: 初始化为适当的值
			CallRet actual;
            //YES
#if NET20 || NET40
		    actual = target.Move(pathPair);
			if (actual.OK) {
				tmpKeys [0] = key;
			}
			Assert.IsTrue(actual.OK, "MoveTest Failure");
#else
            actual = await target.MoveAsync(pathPair);
            if (actual.OK)
            {
                tmpKeys[0] = key;
            }
            Assert.True(actual.OK, "MoveTest Failure");
#endif        
		}

        /// <summary>
        ///Delete 的测试
        ///</summary>
        //[Test]
        //public void DeleteTest()
        //{
        /*
        RSClient target = new RSClient(); // TODO: 初始化为适当的值
        EntryPath scope = new EntryPath(Bucket,LocalKey); // TODO: 初始化为适当的值       
        CallRet actual;
        actual = target.Delete(scope);
        Assert.IsTrue(actual.OK, "DeleteTest Failure");            
        */
        //}

        /// <summary>
        ///Copy 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void CopyTest()
#else
        [Fact]
        public async Task CopyTest()
#endif
		{
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
			string key = NewKey;
			EntryPathPair pathPair = new EntryPathPair(Bucket, tmpKeys[0], key); // TODO: 初始化为适当的值
			CallRet actual;
#if NET20 || NET40
		    actual = target.Copy(pathPair);
			if (actual.OK) {
				RSHelper.RSDel (Bucket, key);
			}
			Assert.IsTrue(actual.OK, "CopyTest Failure");
#else
            actual = await target.CopyAsync(pathPair);
            if (actual.OK)
            {
                await RSHelper.RSDel(Bucket, key);
            }
            Assert.True(actual.OK, "CopyTest Failure");
#endif 
		}

        /// <summary>
        ///BatchStat 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void BatchStatTest()
#else
        [Fact]
        public async Task BatchStatTest()
#endif
		{
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			keys[0] = new EntryPath(Bucket, tmpKeys[0]);
			keys[1] = new EntryPath(Bucket, tmpKeys[1]);//error params
			List<BatchRetItem> actual;
#if NET20 || NET40
		    actual = target.BatchStat(keys);
			Assert.IsTrue(actual.Count == 2, "BatchStatTest Failure");
#else
            actual = await target.BatchStatAsync(keys);
            Assert.True(actual.Count == 2, "BatchStatTest Failure");
#endif
		}

        /// <summary>
        ///BatchMove 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void BatchMoveTest()
#else
        [Fact]
        public async Task BatchMoveTest()
#endif
		{
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
			string tmpKey = NewKey;
			string tmpKey2 = NewKey;
			entryPathPairs[0] = new EntryPathPair(Bucket, tmpKeys[0], tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKeys[1], tmpKey2);

			CallRet actual;
#if NET20 || NET40
		    actual = target.BatchMove(entryPathPairs);
			if (actual.OK) {
				tmpKeys [0] = tmpKey;
				tmpKeys [1] = tmpKey2;
			}
			Assert.IsTrue(actual.OK, "BatchMoveTest Failure");
#else
            actual = await target.BatchMoveAsync(entryPathPairs);
            if (actual.OK)
            {
                tmpKeys[0] = tmpKey;
                tmpKeys[1] = tmpKey2;
            }
            Assert.True(actual.OK, "BatchMoveTest Failure");
#endif
		}

        /// <summary>
        ///BatchDelete 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void BatchDeleteTest()
#else
        [Fact]
        public async Task BatchDeleteTest()
#endif
		{
#if NET20 || NET40
            List<string> tmps = RSHelper.RSPut(Bucket,2);
#else
            List<string> tmps = await RSHelper.RSPut(Bucket,2);
#endif

            RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			int i = 0;
			foreach (string k in tmps) {
				keys [i++] = new EntryPath (Bucket, k);
			}
				
			CallRet actual;
#if NET20 || NET40
		    actual = target.BatchDelete(keys);  
			if (actual.OK) {
				foreach (string k in tmps) {
					RSHelper.RSDel(Bucket,k);
				}
			}
			Assert.IsTrue(actual.OK, "BatchStatTest Failure");
#else
            actual = await target.BatchDeleteAsync(keys);
            if (actual.OK)
            {
                foreach (string k in tmps)
                {
                    await RSHelper.RSDel(Bucket, k);
                }
            }
            Assert.True(actual.OK, "BatchStatTest Failure");
#endif
        }

        /// <summary>
        ///BatchCopy 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void BatchCopyTest()
#else
        [Fact]
        public async Task BatchCopyTest()
#endif
		{
            RSClient target = new RSClient(); // TODO: 初始化为适当的值

			EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
			string tmpKey = NewKey;
			string tmpKey2 = NewKey;
			entryPathPairs[0] = new EntryPathPair(Bucket, tmpKeys[0], tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKeys[1], tmpKey2);            
			CallRet actual;
#if NET20 || NET40
		    actual = target.BatchCopy(entryPathPairs);
			if (actual.OK) {
				RSHelper.RSDel (Bucket, tmpKey);
				RSHelper.RSDel (Bucket, tmpKey2);
			}
			Assert.IsTrue(actual.OK, "BatchStatTest Failure");
#else
            actual = await target.BatchCopyAsync(entryPathPairs);
            if (actual.OK)
            {
                await RSHelper.RSDel(Bucket, tmpKey);
                await RSHelper.RSDel(Bucket, tmpKey2);
            }
            Assert.True(actual.OK, "BatchStatTest Failure");
#endif
		}
	}
}
