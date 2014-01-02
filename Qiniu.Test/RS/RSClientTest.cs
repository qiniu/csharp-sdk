using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qiniu.RS;
using Qiniu.RPC;

namespace Qiniu.Test.RS
{
	/// <summary>
	///这是 RSClientTest 的测试类，旨在
	///包含所有 RSClientTest 单元测试
	///</summary>
	[TestFixture]
	public class RSClientTest:QiniuTestBase
	{
		private List<string> tmpKeys=new List<string>();


		[TestFixtureSetUp]
		public void BeforeTest()
		{
			#region before test
			tmpKeys = RSHelper.RSPut(Bucket,4);
			#endregion
		}
		[TestFixtureTearDown]
		public void AfterTest()
		{
			foreach (string k in tmpKeys) {
				RSHelper.RSDel (Bucket, k);
			}
		}
		/// <summary>
		///Stat 的测试
		///</summary>
		[Test]
		public void StatTest()
		{
			RSClient target = new RSClient();
			//YES
		
//			EntryPath scope = new EntryPath(Bucket, tmpKeys[0]); 
			EntryPath scope = new EntryPath ("wangming", "bucketMgr.md");
			Entry actual;
			actual = target.Stat(scope);
			Assert.IsTrue(actual.OK, "StatTest Failure");
		}

		/// <summary>
		///Move 的测试
		///</summary>
		[Test]
		public void MoveTest()
		{

			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			string key = NewKey;
			EntryPathPair pathPair = new EntryPathPair(Bucket, tmpKeys[0], key); ; // TODO: 初始化为适当的值
			CallRet actual;
			//YES
			actual = target.Move(pathPair);
			if (actual.OK) {
				tmpKeys [0] = key;
			}
			Assert.IsTrue(actual.OK, "MoveTest Failure");          
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
		[Test]
		public void CopyTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			string key = NewKey;
			EntryPathPair pathPair = new EntryPathPair(Bucket, tmpKeys[0], key); // TODO: 初始化为适当的值
			CallRet actual;
			actual = target.Copy(pathPair);
			if (actual.OK) {
				RSHelper.RSDel (Bucket, key);
			}
			Assert.IsTrue(actual.OK, "CopyTest Failure");   
		}

		/// <summary>
		///BatchStat 的测试
		///</summary>
		[Test]
		public void BatchStatTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			keys[0] = new EntryPath(Bucket, tmpKeys[0]);
			keys[1] = new EntryPath(Bucket, tmpKeys[1]);//error params
			List<BatchRetItem> actual;
			actual = target.BatchStat(keys);
			Assert.IsTrue(actual.Count == 2, "BatchStatTest Failure");
		}

		/// <summary>
		///BatchMove 的测试
		///</summary>
		[Test]
		public void BatchMoveTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
			string tmpKey = NewKey;
			string tmpKey2 = NewKey;
			entryPathPairs[0] = new EntryPathPair(Bucket, tmpKeys[0], tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKeys[1], tmpKey2);

			CallRet actual;
			actual = target.BatchMove(entryPathPairs);
			if (actual.OK) {
				tmpKeys [0] = tmpKey;
				tmpKeys [1] = tmpKey2;
			}
			Assert.IsTrue(actual.OK, "BatchMoveTest Failure");
		}

		/// <summary>
		///BatchDelete 的测试
		///</summary>
		[Test]
		public void BatchDeleteTest()
		{
			List<string> tmps = RSHelper.RSPut(Bucket,2);

			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			int i = 0;
			foreach (string k in tmps) {
				keys [i++] = new EntryPath (Bucket, k);
			}
				
			CallRet actual;
			actual = target.BatchDelete(keys);  
			if (actual.OK) {
				foreach (string k in tmps) {
					RSHelper.RSDel(Bucket,k);
				}
			}
			Assert.IsTrue(actual.OK, "BatchStatTest Failure"); ;
		}

		/// <summary>
		///BatchCopy 的测试
		///</summary>
		[Test]
		public void BatchCopyTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值

			EntryPathPair[] entryPathPairs = new EntryPathPair[2]; // TODO: 初始化为适当的值
			string tmpKey = NewKey;
			string tmpKey2 = NewKey;
			entryPathPairs[0] = new EntryPathPair(Bucket, tmpKeys[0], tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKeys[1], tmpKey2);            
			CallRet actual;
			actual = target.BatchCopy(entryPathPairs);
			if (actual.OK) {
				RSHelper.RSDel (Bucket, tmpKey);
				RSHelper.RSDel (Bucket, tmpKey2);
			}
			Assert.IsTrue(actual.OK, "BatchStatTest Failure"); ;
		}
	}
}
