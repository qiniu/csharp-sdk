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
		/// <summary>
		///Stat 的测试
		///</summary>
		[Test]
		public void StatTest()
		{
			RSClient target = new RSClient(); 
			//YES
			EntryPath scope = new EntryPath(Bucket, LocalKey); 
			Entry actual;
			actual = target.Stat(scope);
			Assert.IsTrue(!string.IsNullOrEmpty(actual.Hash), "StatTest Failure");
		}

		/// <summary>
		///Move 的测试
		///</summary>
		[Test]
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
		[Test]
		public void DeleteTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath scope = new EntryPath(Bucket,LocalKey); // TODO: 初始化为适当的值       
			CallRet actual;
			actual = target.Delete(scope);
			Assert.IsTrue(actual.OK, "DeleteTest Failure");            
		}

		/// <summary>
		///Copy 的测试
		///</summary>
		[Test]
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
		[Test]
		public void BatchStatTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			keys[0] = new EntryPath(Bucket, LocalKey);
			keys[1] = new EntryPath("xxx", "xxx");//error params
			List<BatchRetItem> actual;
			actual = target.BatchStat(keys);
			Assert.IsTrue(actual.Count > 0, "BatchStatTest Failure");
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
			entryPathPairs[0] = new EntryPathPair(Bucket, LocalKey, tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKey, LocalKey);

			CallRet actual;
			actual = target.BatchMove(entryPathPairs);
			Assert.IsTrue(actual.OK, "BatchMoveTest Failure");
		}

		/// <summary>
		///BatchDelete 的测试
		///</summary>
		[Test]
		public void BatchDeleteTest()
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath[] keys = new EntryPath[2]; // TODO: 初始化为适当的值
			keys[0] = new EntryPath(Bucket, LocalKey);
			keys[1] = new EntryPath("xxx", "xxx");//error params
			CallRet actual;
			actual = target.BatchDelete(keys);            
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
			entryPathPairs[0] = new EntryPathPair(Bucket, LocalKey, tmpKey);
			entryPathPairs[1] = new EntryPathPair(Bucket, tmpKey, NewKey);            
			CallRet actual;
			actual = target.BatchCopy(entryPathPairs);
			Assert.IsTrue(actual.OK, "BatchStatTest Failure"); ;
		}
	}
}
