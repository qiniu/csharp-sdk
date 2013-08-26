using Qiniu.FileOp;
using Qiniu.RS;
using NUnit.Framework;
using System;

namespace Qiniu.Test.FileOp
{
	/// <summary>
	///这是 ExifTest 的测试类，旨在
	///包含所有 ExifTest 单元测试
	///</summary>
	[TestFixture]
	public class ExifTest:QiniuTestBase
	{
		/// <summary>
		///MakeRequest 的测试
		///</summary>
		[Test]
		public void MakeRequestTest ()
		{
			string url = GetPolicy.MakeBaseUrl ("qiniuphotos.qiniudn.com", "gogopher.jpg"); // TODO: 初始化为适当的值          
			string actual = Exif.MakeRequest (url);
			ExifRet ret = Exif.Call (actual);
			Assert.IsTrue (ret.OK, "MakeRequestTest Failure");
		}
	}
}
