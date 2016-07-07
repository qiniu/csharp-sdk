using System;
using Qiniu.RS;
using Qiniu.Util;
using Qiniu.Auth.digest;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
#endif

namespace Qiniu.Test.FileOp
{


    /// <summary>
    ///这是 GetPolicyTest 的测试类，旨在
    ///包含所有 GetPolicyTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
#else
#endif
    public class GetPolicyTest:QiniuTestBase
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
			string actual;
			FileOpUrl = "http://private-res.qiniudn.com/gogopher.jpg??download/avialkjdf" + StringEx.ToUrlEncode("橛苛要工苛") ;

			actual = GetPolicy.MakeRequest(FileOpUrl);
			//System.Diagnostics.Process.Start(actual);
			PrintLn(actual);
#if NET20 || NET40
			Assert.IsTrue(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeRequestTest Failure");
#else
            Assert.True(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeRequestTest Failure");
#endif
        }

        /// <summary>
        ///MakeBaseUrl 的测试
        ///</summary>
#if NET20 || NET40
        [Test]
#else
        [Fact]
#endif
        public void MakeBaseUrlTest()
		{
			string actual;
			actual = GetPolicy.MakeBaseUrl(Bucket+".qiniudn.com", LocalKey);
			//System.Diagnostics.Process.Start(actual);
			PrintLn(actual);
#if NET20 || NET40
			Assert.IsTrue(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeBaseUrlTest Failure");
#else
            Assert.True(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeBaseUrlTest Failure");
#endif
        }
    }
}
