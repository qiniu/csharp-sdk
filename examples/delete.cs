using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qiniu.RS;
using Qiniu.RPC;

namespace ConsoleDemo
{
    class BucketManager
    {


        public static void Delete(string bucket, string key)
        {
            //实例化一个RSClient对象，用于操作BucketManager里面的方法
            RSClient client = new RSClient();
            CallRet ret = client.Delete(new EntryPath(bucket, key));
            if (ret.OK)
            {
                Console.WriteLine("Delete OK");
            }
            else
            {
                Console.WriteLine("Failed to delete");
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
            //调用Delete方法
            BucketManager.Delete(bucket,key);

        }
    }
}