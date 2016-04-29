using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.RS;

namespace ConsoleDemo
{
    class Download
    {
        public static void download()
        {
            //设置需要操作的账号的AK和SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";
            //构造私有空间的需要生成的下载的链接
            string baseUrl = "http://bucketdomain/key";
            //调用MakeRequest方法生成私有下载链接
            string private_url = GetPolicy.MakeRequest(baseUrl);
            Console.WriteLine(private_url);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            Download.download();
        }
    }
}