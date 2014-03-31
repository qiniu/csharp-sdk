using System;
using System.Threading;
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

		[Test]
		public void PutPolicyTest(){
			PutPolicy policy = new PutPolicy ("lskjd:lskd");
			policy.CallBackUrl = "callbackUrl.com";
			policy.ReturnUrl = "returnUrl.com";
			bool exp = false;
			try{
				policy.Token ();
			}catch{
				exp = true;
				Assert.IsTrue (true, "PutPolicyTest Failure");
			}
			Assert.IsTrue (exp, "PutPolicyTest Failure");
			exp = false;


			policy = new PutPolicy ("bucket");
			policy.CallBackUrl = "callbackUrl.com";
			try{
				policy.Token ();
			}catch{
				exp = true;
				Assert.IsTrue (true, "PutPolicyTest Failure");
			}
			Assert.IsTrue (exp, "PutPolicyTest Failure");
			exp = false;


			policy = new PutPolicy("bucket");
			policy.AsyncOps="";
			policy.CallBackBody="uid=123";
			policy.CallBackUrl="www.qiniu.com";
			policy.DetectMime = 1;
			policy.FsizeLimit=4096;
			policy.InsertOnly = 1;
			policy.PersistentNotifyUrl="www.yourdomain.com/persistentNotifyUrl";
			policy.PersistentOps = "avthumb/m3u8/preset/video_16x9_440k";
			try {
				string result = policy.ToString();
				string expect = "{\"scope\":\"bucket\",\"callBackUrl\":\"www.qiniu.com\",\"callBackBody\":\"uid=123\",\"asyncOps\":\"\",\"deadline\":0,\"insertOnly\":1,\"detectMime\":1,\"fsizeLimit\":4096,\"persistentNotifyUrl\":\"www.yourdomain.com/persistentNotifyUrl\",\"persistentOps\":\"avthumb/m3u8/preset/video_16x9_440k\"}";
				Assert.IsTrue(result==expect,"PutPolicyTest Failure");
			} catch (Exception ee){
				Assert.IsTrue (false, "PutPolicyTest Failure");
			}	

		}

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
			PutPolicy put = new PutPolicy (Bucket);
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
		public void AsyncPutFileTest()
		{

			ManualResetEvent allDone = new ManualResetEvent (false);
			IOClient target = new IOClient(); 
			string key = NewKey;
			PrintLn (key);
			PutExtra extra = new PutExtra (); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string> ();
			PutPolicy put = new PutPolicy (Bucket);
			TmpFIle file = new TmpFIle (1024 * 10);

			bool result = false;
			target.PutProgressChanged+= (object sender, PutProgressEventArgs e) => {
				Console.WriteLine(e.Percentage);
			};
			target.PutFailed+= (object sender, PutFailedEventArgs e) => {
				Console.Write(e.Error.ToString());
				file.Del ();
				allDone.Set();
			};
			target.PutFinished += (object sender, PutRet e) => {
				result = true;
				file.Del ();
				RSHelper.RSDel (Bucket, file.FileName);
				allDone.Set();
			};

			target.AsyncPutFile(put.Token (), file.FileName, file.FileName, extra);

			allDone.WaitOne ();

			//error params
			Assert.IsTrue (result, "PutFileTest Failure");

		}

		/// <summary>
		///PutFile 的测试
		///</summary>
		[Test]
		public void PutFileWithoutKeyTest()
		{

			IOClient target = new IOClient(); 
			string key = NewKey;
			PrintLn (key);
			PutExtra extra = new PutExtra (); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string> ();
			PutPolicy put = new PutPolicy (Bucket);
			TmpFIle file = new TmpFIle (1024 * 10);
			target.PutFinished += new EventHandler<PutRet> ((o,e) => {
				file.Del ();
				if (e.OK) {
					RSHelper.RSDel (Bucket, file.FileName);
				}
			});

			PutRet ret = target.PutFileWithoutKey (put.Token (),file.FileName, extra);

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
            PutPolicy put = new PutPolicy(Bucket);
            target.PutFinished += new EventHandler<PutRet>((o, e) =>
            {
                if (e.OK)
                {
                    RSHelper.RSDel(Bucket, key);
                }
            });
            string token = put.Token();
            PutRet ret = target.Put(put.Token(), key, StreamEx.ToStream("Hello, Qiniu Cloud!"), extra);

            Assert.IsTrue(ret.OK, "PutFileTest Failure");

        }
		[Test]
		public void PutWithoutKeyTest()
		{

		}
	}
}

