using System;
using System.Collections.Specialized;
using System.IO;
using NUnit.Framework;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using Qiniu.RPC;
using Qiniu.Util;
using Qiniu.Test.TestHelper;

namespace Qiniu.Test.IO.Resumable
{
    /// <summary>
    ///这是 ResumablePutTest 的测试类，旨在
    ///包含所有 ResumablePutTest 单元测试
    ///</summary>
    [TestFixture()]
    public class ResumablePutTest:QiniuTestBase
    {
        /// <summary>
        ///PutFile 的测试
        ///</summary>
        [Test]
        public void ResumablePutFileTest()
        {
			Settings putSetting = new Settings(); // TODO: 初始化为适当的值
			string key=NewKey;
            ResumablePutExtra extra = new ResumablePutExtra();
			NameValueCollection nc = new NameValueCollection ();
			nc.Add("x:username","qiniu");
			extra.CallbackParams = nc;
            extra.Notify += new EventHandler<PutNotifyEvent>(extra_Notify);
            extra.NotifyErr += new EventHandler<PutNotifyErrorEvent>(extra_NotifyErr);
            ResumablePut target = new ResumablePut(putSetting, extra); // TODO: 初始化为适当的值
			Console.WriteLine ("extra.Bucket:"+Bucket);
            string upToken = new PutPolicy(Bucket).Token(new Qiniu.Auth.digest.Mac());
            target.Progress += new Action<float>(target_Progress);
			TmpFIle file=new TmpFIle(1024*1024*4);
			target.PutFinished += new EventHandler<CallRet> ((o,e) => {
				file.Del ();
				if (e.OK) {
					RSHelper.RSDel (Bucket, key);
				}
			});
			CallRet ret =target.PutFile (upToken, file.FileName, key);

			//Action a = new Action (() =>
			//{
			//	target.PutFile (upToken, BigFile, NewKey);            
			//});
			//a.BeginInvoke (null,null);

		}

        void extra_NotifyErr(object sender, PutNotifyErrorEvent e)
        {
            
        }

        void extra_Notify(object sender, PutNotifyEvent e)
        {
            PrintLn(e.BlkIdx.ToString());
            PrintLn(e.BlkSize.ToString());
            PrintLn(e.Ret.offset.ToString());
        }
        void target_Progress(float obj)
        {            
            if (obj > 0.999999)
            {
                PrintLn((obj * 100).ToString() + "%");
            }
        }
    }
}
