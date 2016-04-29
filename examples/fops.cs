using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo
{
    class Pfop
    {  

        static void Main(string[] args)
        {
            //初始化AK，SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";

            //设置要转码的空间和key，并且这个key在你空间中存在
            String bucket = "Bucket_Name";
            String key = "Bucket_key";

            //实例化一个entry对象
            EntryPath entry = new EntryPath(bucket, key);

            //设置转码操作参数
            String fops = "avthumb/mp4/s/640x360/vb/1.25m";
            //设置转码的队列
            String pipeline = "yourpipelinename";

            //可以对转码后的文件进行使用saveas参数自定义命名，当然也可以不指定文件会默认命名并保存在当前空间。
            String urlbase64 = Qiniu.Util.Base64URLSafe.Encode("保存的空间:保存的key");
            String pfops = fops + "|saveas/"+urlbase64;

            //实例化一个fop对象主要进行后续转码操作
            Qiniu.RS.Pfop fop = new Qiniu.RS.Pfop();

            Uri uri = null;

            string s = fop.Do(entry, pfops, uri, pipeline, 1);
            Console.WriteLine(s);
            Console.ReadLine();
        }


    }
}