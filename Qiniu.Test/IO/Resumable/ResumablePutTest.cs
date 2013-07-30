using System;
using NUnit.Framework;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using Qiniu.Util;

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
        public void PutFileTest()
        {
            Settings putSetting = new Settings(); // TODO: 初始化为适当的值
            ResumablePutExtra extra = new ResumablePutExtra();
            extra.Notify += new EventHandler<PutNotifyEvent>(extra_Notify);
            extra.NotifyErr += new EventHandler<PutNotifyErrorEvent>(extra_NotifyErr);
            extra.Bucket = Bucket;
            ResumablePut target = new ResumablePut(putSetting, extra); // TODO: 初始化为适当的值
            string upToken = new PutPolicy(extra.Bucket).Token();
            target.Progress += new Action<float>(target_Progress);
            target.PutFile(upToken, BigFile, NewKey);            

        }

        void extra_NotifyErr(object sender, PutNotifyErrorEvent e)
        {
            //throw new NotImplementedException();
        }

        void extra_Notify(object sender, PutNotifyEvent e)
        {
            PrintLn(e.BlkIdx.ToString());
            PrintLn(e.BlkSize.ToString());
            PrintLn(e.Ret.offset.ToString());
            //throw new NotImplementedException();
        }
        bool tag = false;
        void target_Progress(float obj)
        {            
            if (obj > 0.999999)
            {
                PrintLn((obj * 100).ToString() + "%");
                tag = true;
            }
        }
    }
}
