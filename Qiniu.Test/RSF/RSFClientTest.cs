using System;
using System.Collections.Generic;
using Qiniu.RSF;
using Qiniu.Conf;
using Qiniu.Test.TestHelper;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test.RSF
{
    /// <summary>
    ///这是 RSFClientTest 的测试类，旨在
    ///包含所有 RSFClientTest 单元测试
    ///</summary>
#if NET20 || NET40
    [TestFixture]
	public class RSFClientTest:QiniuTestBase
#else
    public class RSFClientTest : QiniuTestBase, IDisposable
#endif
    {
		private List<string> tmpKeys = new List<string> ();

		public RSFClientTest ()
		{
#if !NET20 && !NET40
            Task.Run(async () => await BeforeTest()).Wait();
#endif
        }

#if !NET20 && !NET40
        public async void Dispose()
        {
            await AfterTest();
        }
#endif

#if NET20 || NET40
		[TestFixtureSetUp]
        public void BeforeTest ()
        {
        #region before test
			tmpKeys = RSHelper.RSPut (Bucket, 3);
        #endregion
		}
#else
        public async Task BeforeTest()
        {
            #region before test
            tmpKeys = await RSHelper.RSPut(Bucket, 3);
            #endregion
        }
#endif

#if NET20 || NET40
		[TestFixtureTearDown]
        public void AfterTest ()
#else
        public async Task AfterTest()
#endif
		{
			foreach (string k in tmpKeys) {
#if NET20 || NET40
				RSHelper.RSDel (Bucket, k);
#else
                await RSHelper.RSDel(Bucket, k);
#endif
            }
		}

        /// <summary>
        ///ListPrefix 的测试
        ///</summary>
#if NET20 || NET40
		[Test]
        public void ListPrefixTest()
#else
        [Fact]
        public async Task ListPrefixTest()
#endif
		{
            RSFClient target = new RSFClient (Bucket); // TODO: 初始化为适当的值
			target.Marker = string.Empty;
			target.Prefix = "csharp";
			target.Limit = 3;
			DumpRet actual;
#if NET20 || NET40
            actual = target.ListPrefix (Bucket);
			foreach (DumpItem item in actual.Items) {
				Console.WriteLine ("Key:{0},Hash:{1},Mime:{2},PutTime:{3},EndUser:{4}", item.Key, item.Hash, item.Mime, item.PutTime, item.EndUser);
			}

			//error params
			Assert.IsTrue (actual.Items.Count >= 3, "ListPrefixTest Failure");
#else
            actual = await target.ListPrefixAsync(Bucket);
            foreach (DumpItem item in actual.Items)
            {
                Console.WriteLine("Key:{0},Hash:{1},Mime:{2},PutTime:{3},EndUser:{4}", item.Key, item.Hash, item.Mime, item.PutTime, item.EndUser);
            }

            //error params
            Assert.True(actual.Items.Count >= 3, "ListPrefixTest Failure");
#endif

        }
	}
}

