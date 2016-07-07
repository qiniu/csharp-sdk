using System;
using Qiniu.FileOp;
using Qiniu.RS;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
#endif

namespace Qiniu.Test.FileOp
{


    /// <summary>
    ///这是 ImageViewTest 的测试类，旨在
    ///包含所有 ImageViewTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
#endif
    public class ImageViewTest:QiniuTestBase
    {
        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
#if NET20 || NET40
        [Test]
#else
        [Fact]
#endif
        public void MakeRequestTest()
        {
            ImageView target = new ImageView { Mode = 0, Width = 200, Height = 200, Quality = 90, Format = "gif" }; // TODO: 初始化为适当的值
            string url = FileOpUrl; // TODO: 初始化为适当的值
            string actual;
            actual = target.MakeRequest(url);
            //System.Diagnostics.Process.Start(actual);
#if NET20 || NET40
            Assert.IsTrue(!string.IsNullOrEmpty(actual), "ImageViewTest MakeRequestTest Failure");
#else
            Assert.True(!string.IsNullOrEmpty(actual), "ImageViewTest MakeRequestTest Failure");
#endif

        }
    }
}
