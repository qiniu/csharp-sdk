using Qiniu.IO.Resumable;
using Qiniu.RS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace QiniuSDKTest
{
    
    
    /// <summary>
    ///这是 ResumablePutTest 的测试类，旨在
    ///包含所有 ResumablePutTest 单元测试
    ///</summary>
    [TestClass()]
    public class ResumablePutTest:Test
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
        [TestCleanup()]
        public void MyTestCleanup()
        {
            while (!tag)
            {
                System.Threading.Thread.Sleep(500);
            }
        }
        
        #endregion

        /// <summary>
        ///PutFile 的测试
        ///</summary>
        [TestMethod()]
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
