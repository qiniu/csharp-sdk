using System;
using Qiniu.IO;
using Qiniu.RS;
using Qiniu.Util;
using Qiniu.Conf;

namespace csharptest
{
	class MainClass
	{
		
		static string Bucket = "icattlecoder3";
		static string LocalKey = "gogopher.jpg";
		static string DOMAIN = "qiniuphotos.qiniudn.com";
		static string LocalFile = @"~/.profile";
		static string BigFile = @"C:\Users\floyd\Downloads\ChromeSetup.exe";
		static string FileOpUrl = "http://qiniuphotos.qiniudn.com/gogopher.jpg";
		static string NewKey
		{
			get { return Guid.NewGuid().ToString(); }
		}

		public static void Main (string[] args)
		{
			Config.ACCESS_KEY = "gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
			Config.SECRET_KEY = "OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";

			PutTest ();

			Console.WriteLine ("Hello World!");
			Console.Write ("");
		}

		public static void PutTest()
		{
			IOClient target = new IOClient(); 
			string upToken = string.Empty;
			string key = LocalKey;
			//PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Crc32 = 123;
			extra.CheckCrc = CheckCrcType.CHECK;
			extra.Params = new System.Collections.Generic.Dictionary<string, string>();
			extra.Scope = Bucket+":"+key;
			PutPolicy put = new PutPolicy(extra.Scope);

			PutRet ret = target.Put(put.Token(), key, "hello Qiniu Cloud!".ToStream(), extra);

			//Assert.IsTrue(ret.OK, "PutFileTest Failure");

		}
	}
}


