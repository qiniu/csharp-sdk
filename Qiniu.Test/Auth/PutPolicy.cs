using System;
using NUnit.Framework;
using Qiniu.RS;
using Qiniu.Util;
using Qiniu.Auth.digest;

namespace Qiniu.Test.FileOp
{


	/// <summary>
	///这是 GetPolicyTest 的测试类，旨在
	///包含所有 GetPolicyTest 单元测试
	///</summary>
	[TestFixture]
	public class GetPolicyTest:QiniuTestBase
	{
		/// <summary>
		///MakeRequest 的测试
		///</summary>
		[Test]
		public void MakeRequestTest()
		{
			string actual;
			FileOpUrl = "http://icattlecoder-private.qiniudn.com/img.jpg?download/avialkjdf" + "橛苛要工苛".ToUrlEncode() ;

			actual = GetPolicy.MakeRequest(FileOpUrl);
			//System.Diagnostics.Process.Start(actual);
			PrintLn(actual);
			Assert.IsTrue(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeRequestTest Failure");
		}

		/// <summary>
		///MakeBaseUrl 的测试
		///</summary>
		[Test]
		public void MakeBaseUrlTest()
		{
			string actual;
			actual = GetPolicy.MakeBaseUrl(Bucket+".qiniudn.com", LocalKey);
			//System.Diagnostics.Process.Start(actual);
			PrintLn(actual);
			Assert.IsTrue(!string.IsNullOrEmpty(actual), "GetPolicyTest MakeBaseUrlTest Failure");
		}
	}
}
