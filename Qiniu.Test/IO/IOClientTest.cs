using System;
using NUnit.Framework;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.Util;

namespace Qiniu.Test.IO
{
	[TestFixture]
	public class IOClientTest:QiniuTestBase
	{

		/// <summary>
		///PutFile 的测试
		///</summary>
		[Test]
		public void PutFileTest()
		{
			IOClient target = new IOClient(); 
			string upToken = string.Empty;
			string key = LocalKey;
			PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string>();
			extra.Scope = Bucket;
			PutPolicy put = new PutPolicy(extra.Scope);
			PutRet ret = target.PutFile(put.Token(), key, LocalFile, extra);
			//error params
			target.PutFile("error", "error", "error", null);
			Assert.IsTrue(ret.OK, "PutFileTest Failure");

		}
		[Test]
		public void PutTest()
		{
			IOClient target = new IOClient(); 
			string upToken = string.Empty;
			string key = LocalKey;
			PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string>();
			extra.Scope = Bucket;
			PutPolicy put = new PutPolicy(extra.Scope);

			PutRet ret = target.Put(put.Token(), key, "Hello, Qiniu Cloud!".ToStream(), extra);
		
			Assert.IsTrue(ret.OK, "PutFileTest Failure");

		}
	}
}

