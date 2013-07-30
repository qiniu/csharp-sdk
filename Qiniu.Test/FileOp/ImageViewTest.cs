using System;
using Qiniu.FileOp;
using Qiniu.RS;
using NUnit.Framework;

namespace Qiniu.Test.FileOp
{
    
    
    /// <summary>
    ///这是 ImageViewTest 的测试类，旨在
    ///包含所有 ImageViewTest 单元测试
    ///</summary>
    [TestFixture]
    public class ImageViewTest:QiniuTestBase
    {
        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
        [Test]
        public void MakeRequestTest()
        {
            ImageView target = new ImageView { Mode = 0, Width = 200, Height = 200, Quality = 90, Format = "gif" }; // TODO: 初始化为适当的值
            string url = FileOpUrl; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = target.MakeRequest(url);
            System.Diagnostics.Process.Start(actual);
            Assert.IsTrue(!string.IsNullOrEmpty(actual), "ImageViewTest MakeRequestTest Failure");
           
        }
    }
}
