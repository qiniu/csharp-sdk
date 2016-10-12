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

            //覆盖上传,<bucket>:<key>，表示只允许用户上传指定key的文件。在这种格式下文件默认允许“修改”，已存在同名资源则会被本次覆盖。
            PutPolicy put = new PutPolicy(bucket+":"+key, 3600);

            //设置callbackUrl以及callbackBody,七牛将文件名和文件大小回调给业务服务器
            put.CallBackUrl = "http://your.domain.com/callback";
            put.CallBackBody = "filename=$(fname)&filesize=$(fsize)";

            //调用Token()方法生成上传的Token
            string upToken = put.Token();
            //上传文件的路径
            String filePath = "/.../...";

            //调用PutFile()方法上传
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