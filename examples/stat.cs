using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.RS;
using Qiniu.RPC;

namespace ConsoleDemo
{
    class BucketManager
    {

        public static void Stat(string bucket, string key)
        {   

            //实例化一个RSClient对象，用于操作BucketManager里面的方法
            RSClient client = new RSClient();
            //调用Stat方法获取文件的信息
            Entry entry = client.Stat(new EntryPath(bucket, key));
            if (entry.OK)
            {   
                //打印文件的hash、fsize等信息
                Console.WriteLine("Hash: " + entry.Hash);
                Console.WriteLine("Fsize: " + entry.Fsize);
                Console.WriteLine("PutTime: " + entry.PutTime);
                Console.WriteLine("MimeType: " + entry.MimeType);
                Console.ReadLine();
            }
            else
            {   
                Console.WriteLine("Failed to Stat");
            }
        }

        static void Main(string[] args)
        {
            //初始化AK，SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";
            //要测试的空间和key，并且这个key在你空间中存在
            String bucket = "Bucket_Name";
            String key = "Bucket_key";
            //调用Stat方法
            BucketManager.Stat(bucket,key);

        }
    }
}