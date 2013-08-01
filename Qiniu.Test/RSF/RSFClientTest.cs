using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qiniu.RSF;
using Qiniu.Conf;
using Qiniu.Test.TestHelper;

namespace Qiniu.Test.RSF
{
	/// <summary>
	///这是 RSFClientTest 的测试类，旨在
	///包含所有 RSFClientTest 单元测试
	///</summary>
	[TestFixture]
	public class RSFClientTest:QiniuTestBase
	{
		private List<string> tmpKeys=new List<string>();

		public RSFClientTest()
		{

		}
		[TestFixtureSetUp]
		public void BeforeTest()
		{
			#region before test
			tmpKeys = RSHelper.RSPut(Bucket,3);
			#endregion
		}
		[TestFixtureTearDown]
		public void AfterTest()
		{
			foreach (string k in tmpKeys) {
				RSHelper.RSDel (Bucket, k);
			}
		}
		/// <summary>
		///Next 的测试
		///</summary>
		[Test]
		public void NextTest()
		{
			RSFClient target = new RSFClient(Bucket); // TODO: 初始化为适当的值
			target.Init();
			target.Marker = string.Empty;
			target.Prefix = string.Empty;
			target.Limit = 1000;
			List<DumpItem> actual;
			int count = 0;  
			actual = target.Next();
			while (actual != null)
			{
				count += actual.Count;
				actual = target.Next();
			}
			Assert.IsTrue(count == 3, "ListPrefixTest Failure");
		}

		/// <summary>
		///ListPrefix 的测试
		///</summary>
		[Test]
		public void ListPrefixTest()
		{
			RSFClient target = new RSFClient(Bucket); // TODO: 初始化为适当的值
			target.Marker = string.Empty;
			target.Prefix = string.Empty;
			target.Limit = 100;
			DumpRet actual;
			actual = target.ListPrefix(Bucket);
			foreach (DumpItem item in actual.Items)
			{
				Console.WriteLine("Key:{0},Hash:{1},Mime:{2},PutTime:{3},EndUser:{4}", item.Key, item.Hash, item.Mime, item.PutTime, item.EndUser);
			}

			//error params
			Assert.IsTrue(actual.Items.Count==3, "ListPrefixTest Failure");

		}
	}
}

