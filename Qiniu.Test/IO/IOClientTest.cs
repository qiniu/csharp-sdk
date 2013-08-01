using System;
using NUnit.Framework;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.Util;
using Qiniu.Test.TestHelper;

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
			string key = NewKey;
			PrintLn (key);
			PutExtra extra = new PutExtra (); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string> ();
			extra.Scope = Bucket;
			PutPolicy put = new PutPolicy (extra.Scope);
			TmpFIle file = new TmpFIle (1024 * 10);
			target.PutFinished += new EventHandler<PutRet> ((o,e) => {
				file.Del ();
				if (e.OK) {
					RSHelper.RSDel (Bucket, file.FileName);
				}
			});

			PutRet ret = target.PutFile (put.Token (), file.FileName, file.FileName, extra);

			//error params
			//target.PutFile("error", "error", "error", null);
			Assert.IsTrue (ret.OK, "PutFileTest Failure");

		}
		[Test]
		public void PutTest()
		{
			IOClient target = new IOClient(); 
			string key = NewKey;
			PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string>();
			extra.Scope = Bucket;
			PutPolicy put = new PutPolicy(extra.Scope);
			target.PutFinished += new EventHandler<PutRet> ((o,e) => {
				if (e.OK) {
					RSHelper.RSDel (Bucket, key);
				}
			});
			PutRet ret = target.Put(put.Token(), key, "Hello, Qiniu Cloud!".ToStream(), extra);
		
			Assert.IsTrue(ret.OK, "PutFileTest Failure");

		}
	}
}

