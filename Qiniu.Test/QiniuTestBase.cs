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
		protected static string LocalFile = @"~/.profile";
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
			/*
			Config.ACCESS_KEY = System.Environment.GetEnvironmentVariable ("QINIU_ACCESS_KEY");  
			Config.SECRET_KEY = System.Environment.GetEnvironmentVariable ("QINIU_SECRET_KEY");  
			Bucket =System.Environment.GetEnvironmentVariable ("QINIU_TEST_BUCKET");   
			DOMAIN =System.Environment.GetEnvironmentVariable ("QINIU_TEST_DOMAIN"); 
			*/

			Config.ACCESS_KEY = "gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
			Config.SECRET_KEY = "OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";
			Bucket = "icattlecoder3";
			DOMAIN = "qiniuphotos.qiniudn.com";

			//for MonoDevelop Nunit 
			//Config.ACCESS_KEY = System.Configuration.ConfigurationManager.AppSettings ["QINIU_ACCESS_KEY"];
			//Config.SECRET_KEY = System.Configuration.ConfigurationManager.AppSettings ["QINIU_SECRET_KEY"];
			//Bucket = System.Configuration.ConfigurationManager.AppSettings ["QINIU_TEST_BUCKET"];
			//DOMAIN = System.Configuration.ConfigurationManager.AppSettings ["QINIU_TEST_DOMAIN"];
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

