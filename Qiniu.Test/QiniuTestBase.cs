using System;
using Qiniu.Conf;
using System.Collections;

namespace Qiniu.Test
{
	public class QiniuTestBase
	{

		protected static string Bucket = "";
		protected static  string LocalKey = "gogopher.jpg";
		protected static string DOMAIN = "qiniuphotos.qiniudn.com";
		protected static string BigFile = @"";
		protected static string FileOpUrl = "http://qiniuphotos.qiniudn.com/gogopher.jpg";
		protected static string NewKey
		{
			get { return Guid.NewGuid().ToString(); }
		}
		private static bool init = false;
		private void Init()
		{
			if (init)
				return;
				
			//for make test
			Config.ACCESS_KEY = "IT9iP3J9wdXXYsT1p8ns0gWD-CQOdLvIQuyE0FOK";
			Config.SECRET_KEY = "zUCzekBtEqTZ4-WJPCGlBrr2PeyYxsYn98LPaivM";

//			Config.ACCESS_KEY = System.Environment.GetEnvironmentVariable ("QINIU_ACCESS_KEY");  
//			Config.SECRET_KEY = System.Environment.GetEnvironmentVariable ("QINIU_SECRET_KEY");  
//			Bucket =System.Environment.GetEnvironmentVariable ("QINIU_TEST_BUCKET");   
//			DOMAIN =System.Environment.GetEnvironmentVariable ("QINIU_TEST_DOMAIN"); 

			init = true;
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

