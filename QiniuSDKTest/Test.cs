using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Conf;

namespace QiniuSDKTest
{
    public class Test
    {
        protected string Bucket = "<Bucket,eg.qiniuphotos>";
        protected string LocalKey = "<LocalKey,eg.gogopher.jpg>";
        protected string DOMAIN = "<DOMAIN,eg.qiniuphotos.qiniudn.com>";
        protected string LocalFile = @"<LocalFilePath>";
        protected string BigFile = @"<BigFilePath>";
        protected string FileOpUrl = "http://qiniuphotos.qiniudn.com/gogopher.jpg";
        protected string NewKey
        {
            get { return Guid.NewGuid().ToString(); }
        }

        private void Init()
        {
            Config.ACCESS_KEY = "<ACCESS_KEY>";
            Config.SECRET_KEY = "<SECRET_KEY>";
        }

        public Test()
        {
            Init();
        }
        protected void PrintLn(string str)
        {
            Console.WriteLine(str);
        }
    }
}
