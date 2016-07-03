using Qiniu.FileOp;
using Qiniu.RS;
using System;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test.FileOp
{
    /// <summary>
    ///这是 ExifTest 的测试类，旨在
    ///包含所有 ExifTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
#endif
	public class ExifTest:QiniuTestBase
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
			string url = GetPolicy.MakeBaseUrl ("qiniuphotos.qiniudn.com", "gogopher.jpg"); // TODO: 初始化为适当的值          
			string actual = Exif.MakeRequest (url);
#if NET20 || NET40
		    ExifRet ret = Exif.Call (actual);
			Assert.IsTrue (ret.OK, "MakeRequestTest Failure");
#else
            ExifRet ret = await Exif.CallAsync(actual);
            Assert.True(ret.OK, "MakeRequestTest Failure");
#endif
        }
    }
}
