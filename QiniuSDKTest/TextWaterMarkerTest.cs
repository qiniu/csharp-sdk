using Qiniu.FileOp;
using Qiniu.RS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace QiniuSDKTest
{
    
    
    /// <summary>
    ///这是 TextWaterMarkerTest 的测试类，旨在
    ///包含所有 TextWaterMarkerTest 单元测试
    ///</summary>
    [TestClass()]
    public class TextWaterMarkerTest:Test
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
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
        [TestMethod()]
        public void MakeRequestTest()
        {
            string text = "Qiniu"; // TODO: 初始化为适当的值
            string fontname = string.Empty; // TODO: 初始化为适当的值
            string color = "#123abc"; // TODO: 初始化为适当的值
            int fontsize = 2000; // TODO: 初始化为适当的值
            int dissolve = 50; // TODO: 初始化为适当的值            
            int dx = 0; // TODO: 初始化为适当的值
            int dy = 0; // TODO: 初始化为适当的值
            TextWaterMarker target = new TextWaterMarker(text, fontname, color, fontsize, dissolve, MarkerGravity.South, dx, dy); // TODO: 初始化为适当的值
                      
            string actual;
            actual = target.MakeRequest(FileOpUrl);
            // 如果是私有空间，添加下面一句
            actual= GetPolicy.MakeRequest(actual);
            System.Diagnostics.Process.Start(actual);
            PrintLn(actual);
            Assert.IsTrue(!string.IsNullOrEmpty(actual), "TextWaterMarkerTest MakeRequestTest Failure");
        }
    }
}
