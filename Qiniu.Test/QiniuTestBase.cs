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
#if NET20 || NET40
            Config.ACCESS_KEY = System.Environment.GetEnvironmentVariable ("QINIU_ACCESS_KEY");  
			Config.SECRET_KEY = System.Environment.GetEnvironmentVariable ("QINIU_SECRET_KEY");  
			Bucket =System.Environment.GetEnvironmentVariable ("QINIU_TEST_BUCKET");   
			DOMAIN =System.Environment.GetEnvironmentVariable ("QINIU_TEST_DOMAIN"); 
#else
            Config.ACCESS_KEY = "QWYn5TFQsLLU1pL5MFEmX3s5DmHdUThav9WyOWOm";
		    Config.SECRET_KEY = "Bxckh6FA-Fbs9Yt3i3cbKVK22UPBmAOHJcL95pGz";
            Bucket = "csharpsdk";
            DOMAIN = "csharpsdk.qiniudn.com";
#endif

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

