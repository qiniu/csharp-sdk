using System;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.Util;
using Qiniu.Test.TestHelper;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test.IO
{
#if NET20 || NET40
    [TestFixture]
#endif
    public class IOClientTest:QiniuTestBase
	{

#if NET20 || NET40
        [Test]
#else
        [Fact]
#endif
        public void PutPolicyTest(){
			PutPolicy policy = new PutPolicy ("lskjd:lskd");
			policy.CallBackUrl = "callbackUrl.com";
			policy.ReturnUrl = "returnUrl.com";
			bool exp = false;
			try{
				policy.Token ();
			}catch{
				exp = true;
#if NET20 || NET40
				Assert.IsTrue (true, "PutPolicyTest Failure1");
#else
                Assert.True(true, "PutPolicyTest Failure1");
#endif
            }
#if NET20 || NET40
			Assert.IsTrue (exp, "PutPolicyTest Failure2");
#else
            Assert.True(exp, "PutPolicyTest Failure2");
#endif
            exp = false;


			policy = new PutPolicy ("bucket");
			policy.CallBackUrl = "callbackUrl.com";
			try{
				policy.Token ();
			}catch{
				exp = true;
#if NET20 || NET40
				Assert.IsTrue (true, "PutPolicyTest Failure3");
#else
                Assert.True(true, "PutPolicyTest Failure3");
#endif
            }
#if NET20 || NET40
			Assert.IsTrue (exp, "PutPolicyTest Failure4");
#else
            Assert.True(exp, "PutPolicyTest Failure4");
#endif
            exp = false;


			policy = new PutPolicy("bucket");
			policy.CallBackBody="uid=123";
			policy.CallBackUrl="www.qiniu.com";
			policy.DetectMime = 1;
			policy.FsizeLimit=4096;
			policy.FsizeMin=0;
			policy.InsertOnly = 1;
			policy.PersistentNotifyUrl="www.yourdomain.com/persistentNotifyUrl";
			policy.PersistentOps = "avthumb/m3u8/preset/video_16x9_440k";
			policy.CallbackHost = "180.97.211.38";
            policy.CallbackFetchKey = 0;
            policy.CallbackBodyType = "application/json";
			try {
				string result = policy.ToString();
				string expect = "{\"scope\":\"bucket\",\"callBackUrl\":\"www.qiniu.com\",\"callBackBody\":\"uid=123\",\"deadline\":0,\"insertOnly\":1,\"detectMime\":1,\"fsizeLimit\":4096,\"fsizeMin\":0,\"persistentNotifyUrl\":\"www.yourdomain.com/persistentNotifyUrl\",\"persistentOps\":\"avthumb/m3u8/preset/video_16x9_440k\",\"callbackHost\":\"180.97.211.38\",\"callbackBodyType\":\"application/json\",\"callbackFetchKey\":0}";
                //Assert.IsTrue(result==expect,"PutPolicyTest Failure5");
#if NET20 || NET40
				Assert.AreEqual(result, expect);
#else
                Assert.Equal(result, expect);
#endif
            } catch (Exception ee){
#if NET20 || NET40
				Assert.IsTrue (false, ee.Message.ToString());
#else
                Assert.True(false, ee.Message);
#endif
            }

		}

        /// <summary>
        ///PutFile 的测试
        ///</summary>
#if NET20 || NET40
        [Test]
		public void PutFileTest()
#else
        [Fact]
        public async Task PutFileTest()
#endif
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
#if NET20 || NET40
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
#else
            target.PutFinished += new EventHandler<PutRet>(async (o, e) => {
                file.Del();
                if (e.OK)
                {
                    await RSHelper.RSDel(Bucket, file.FileName);
                }
            });
            PutRet ret = await target.PutFileAsync(put.Token(), file.FileName, file.FileName, extra);

            //error params
            //target.PutFile("error", "error", "error", null);
            Assert.True(ret.OK, "PutFileTest Failure");
#endif

        }
        /// <summary>
        ///PutFile 的测试
        ///</summary>
#if NET20 || NET40
        [Test]
		public void PutFileWithoutKeyTest()
#else
        [Fact]
        public async Task PutFileWithoutKeyTest()
#endif
		{

			IOClient target = new IOClient();

			PutExtra extra = new PutExtra (); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string> ();
			PutPolicy put = new PutPolicy (Bucket);
			TmpFIle file = new TmpFIle (1024 * 10);
#if NET20 || NET40
			target.PutFinished += new EventHandler<PutRet> ((o,e) => {
				file.Del ();
				if (e.OK) {
					RSHelper.RSDel (Bucket, file.FileName);
				}
			});
			PutRet ret = target.PutFileWithoutKey (put.Token (),file.FileName, extra);

			Assert.AreEqual (ret.Hash, ret.key, "expected key equal to hash");
			//error params
			//target.PutFile("error", "error", "error", null);
			Assert.IsTrue (ret.OK, "PutFileTest Failure");
#else
            target.PutFinished += new EventHandler<PutRet>(async (o, e) => {
                file.Del();
                if (e.OK)
                {
                    await RSHelper.RSDel(Bucket, file.FileName);
                }
            });
            PutRet ret = await target.PutFileWithoutKeyAsync(put.Token(), file.FileName, extra);

            Assert.True(ret.Hash == ret.key, "expected key equal to hash");
            //error params
            //target.PutFile("error", "error", "error", null);
            Assert.True(ret.OK, "PutFileTest Failure");
#endif

        }

#if NET20 || NET40
        [Test]
		public void PutTest()
#else
        [Fact]
        public async Task PutTest()
#endif
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
#if NET20 || NET40
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
#else
            target.PutFinished += new EventHandler<PutRet>(async (o, e) =>
            {
                if (e.OK)
                {
                    await RSHelper.RSDel(Bucket, key);
                }
            });
            string token = put.Token();
            PutRet ret = await target.PutAsync(put.Token(), key, StreamEx.ToStream("Hello, Qiniu Cloud!"), extra);

            Assert.True(ret.OK, "PutFileTest Failure");
#endif

        }

#if NET20 || NET40
		[Test]
#else
        [Fact]
#endif
        public void PutWithoutKeyTest()
		{


		}
	}
}

