using System;
using Qiniu.Conf;
using System.Collections;

namespace Qiniu.Test
{
	public class QiniuTestBase
	{

		protected string Bucket = "";
		protected string LocalKey = "gogopher.jpg";
		protected string DOMAIN = "qiniuphotos.qiniudn.com";
		protected string LocalFile = @"~/.profile";
		protected string BigFile = @"";
		protected string FileOpUrl = "http://qiniuphotos.qiniudn.com/gogopher.jpg";
		protected string NewKey
		{
			get { return Guid.NewGuid().ToString(); }
		}

		private void Init()
		{

			Config.ACCESS_KEY = System.Configuration.ConfigurationManager.AppSettings ["QINIU_ACCESS_KEY"];
			Config.SECRET_KEY = System.Configuration.ConfigurationManager.AppSettings ["QINIU_SECRET_KEY"];
			Bucket = System.Configuration.ConfigurationManager.AppSettings ["QINIU_TEST_BUCKET"];
			DOMAIN = System.Configuration.ConfigurationManager.AppSettings ["QINIU_TEST_DOMAIN"];
		}

		public QiniuTestBase()
		{
			Init();
		}
		protected void PrintLn(string str)
		{
			Console.WriteLine(str);
		}
	}
}

