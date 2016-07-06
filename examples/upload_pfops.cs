using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qiniu.Auth;
using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;

namespace ConsoleDemo
{
    class UploadDemo
    {

        private void upload()
        {
            //设置账号的AK和SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";
            IOClient target = new IOClient();
            PutExtra extra = new PutExtra();
            //设置上传的空间
            String bucket = "bucket_name";
            //设置上传的文件的key值
            String key = "yourdefinekey";

            //设置转码操作参数
            String fops = "avthumb/mp4/s/640x360b/1.25m";
            //设置转码的队列
            String pipeline = "yourpipelinename";

            //可以对转码后的文件进行使用saveas参数自定义命名，当然也可以不指定文件会默认命名并保存在当前空间。
            String urlbase64 = Qiniu.Util.Base64URLSafe.Encode("目标Bucket_Name:自定义文件key");
            String pfops = fops + "|saveas/" + urlbase64;

            //普通上传,只需要设置上传的空间名就可以了,第二个参数可以设定token过期时间
            PutPolicy put = new PutPolicy(bucket, 3600);

            //设置PersistentOps以及PersistentPipeline
            put.PersistentOps = fops;
            put.PersistentPipeline = pipeline;

            //调用Token()方法生成上传的Token
            string upToken = put.Token();
            //上传文件的路径
            String filePath = "/.../...";

            PutRet ret = target.PutFile(upToken, key, filePath, extra);
            //打印出相应的信息
            Console.WriteLine(ret.Response.ToString());
            Console.WriteLine(ret.key);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //实例化UploadDemo对象并调用设置的upload方法
            UploadDemo Upload = new UploadDemo();
            Upload.upload();
        }

    }
}