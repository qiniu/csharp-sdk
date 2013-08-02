using System;
using Qiniu.FileOp;
using Qiniu.RS;
using NUnit.Framework;

namespace Qiniu.Test.FileOp
{
    
    
    /// <summary>
    ///这是 TextWaterMarkerTest 的测试类，旨在
    ///包含所有 TextWaterMarkerTest 单元测试
    ///</summary>
    [TestFixture]
    public class TextWaterMarkerTest:QiniuTestBase
    {
        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
        [Test]
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
            //System.Diagnostics.Process.Start(actual);
            PrintLn(actual);
            Assert.IsTrue(!string.IsNullOrEmpty(actual), "TextWaterMarkerTest MakeRequestTest Failure");
        }
    }
}
