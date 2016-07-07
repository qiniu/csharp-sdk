﻿using System;
using Qiniu.FileOp;
using Qiniu.RS;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test.FileOp
{


    /// <summary>
    ///这是 ImageMogrifyTest 的测试类，旨在
    ///包含所有 ImageMogrifyTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
#endif
    public class ImageMogrifyTest:QiniuTestBase
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
            ImageMogrify target = new ImageMogrify
            {
                Thumbnail = "!50x50r",
                Gravity = "center",
                Rotate = 90,
                Crop = "!50x50",
                Quality = 80,
                AutoOrient = true
            };
            string mogrUrl = target.MakeRequest(FileOpUrl);
            //System.Diagnostics.Process.Start(mogrUrl);
            PrintLn(mogrUrl);
#if NET20 || NET40
		    Assert.IsTrue(!string.IsNullOrEmpty(mogrUrl), "ImageMogrifyTest MakeRequestTest Failure");
#else
            Assert.True(!string.IsNullOrEmpty(mogrUrl), "ImageMogrifyTest MakeRequestTest Failure");
#endif
        }
}
}
