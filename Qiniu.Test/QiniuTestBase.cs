using System;
using Qiniu.Conf;

namespace Qiniu.Test
{
	public class QiniuTestBase
	{

		protected string Bucket = "icattlecoder3";
		protected string LocalKey = "gogopher.jpg";
		protected string DOMAIN = "qiniuphotos.qiniudn.com";
		protected string LocalFile = @"~/.profile";
		protected string BigFile = @"C:\Users\floyd\Downloads\ChromeSetup.exe";
		protected string FileOpUrl = "http://qiniuphotos.qiniudn.com/gogopher.jpg";
		protected string NewKey
		{
			get { return Guid.NewGuid().ToString(); }
		}

		private void Init()
		{
			Config.ACCESS_KEY = "gPhMyVzzbQ_LOjboaVsy7dbCB4JHgyVPonmhT3Dp";
			Config.SECRET_KEY = "OjY7IMysXu1erRRuWe7gkaiHcD6-JMJ4hXeRPZ1B";
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

