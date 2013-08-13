using System;
using System.Text;

namespace Qiniu.Conf
{
	public class Config
	{
		public static string USER_AGENT = "qiniu csharp-sdk v6.0.0";
		public static string ACCESS_KEY = "<Please apply your access key>";
		public static string SECRET_KEY = "<Dont send your secret key to anyone>";
		// 不要在客户端初始化该变量
		public static string RS_HOST = "http://rs.Qbox.me";
		public static string UP_HOST = "http://up.qiniu.com";
		public static string RSF_HOST = "http://rsf.Qbox.me";
		public static Encoding Encoding = Encoding.UTF8;
	}
}
