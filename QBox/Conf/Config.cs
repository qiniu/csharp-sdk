using System;
using System.Text;

namespace QBox.Conf
{
    public class Config
    {

        public static string USER_AGENT = "<User-Agent,eg.qiniu csharp-sdk v6.0.0>";

        public static string ACCESS_KEY = "<Please apply your access key>";
        public static string SECRET_KEY = "<Dont send your secret key to anyone>";

        public static string RS_HOST = "http://rs.qbox.me";
        public static string UP_HOST = "http://up.qbox.me";
		public static string RSF_HOST = "http://rsf.qbox.me";

        public static Encoding Encoding = Encoding.UTF8;
    }
}
