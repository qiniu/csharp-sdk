using System;
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
    ///这是 ImageInfoTest 的测试类，旨在
    ///包含所有 ImageInfoTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
#endif
    public class ImageInfoTest:QiniuTestBase
    {
        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void MakeRequestTest()
#else
        [Fact]
        public async Task MakeRequestTest()
#endif
        {
            string actual;            
            actual = ImageInfo.MakeRequest(FileOpUrl);
            //System.Diagnostics.Process.Start(actual);
#if NET20 || NET40
		    ImageInfoRet ret = ImageInfo.Call(actual);
            Assert.IsNotNull(ret, "ImageInfoTest MakeRequestTest Failure");
#else
            ImageInfoRet ret = await ImageInfo.CallAsync(actual);
            Assert.True(ret != null, "ImageInfoTest MakeRequestTest Failure");
#endif
        }
    }
}
