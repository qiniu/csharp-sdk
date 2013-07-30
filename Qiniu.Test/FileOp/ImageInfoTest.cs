using System;
using Qiniu.FileOp;
using Qiniu.RS;
using NUnit.Framework;

namespace Qiniu.Test.FileOp
{
    
    
    /// <summary>
    ///这是 ImageInfoTest 的测试类，旨在
    ///包含所有 ImageInfoTest 单元测试
    ///</summary>
    [TestFixture]
	public class ImageInfoTest:QiniuTestBase
    {
        /// <summary>
        ///MakeRequest 的测试
        ///</summary>
        [Test]
        public void MakeRequestTest()
        {
            string url = string.Empty; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;            
            actual = ImageInfo.MakeRequest(FileOpUrl);
            System.Diagnostics.Process.Start(actual);
            ImageInfoRet ret= ImageInfo.Call(actual);
            Assert.IsNotNull(ret, "ImageInfoTest MakeRequestTest Failure");
        }
    }
}
