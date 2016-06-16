using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Conf;
using Qiniu.RS;
using Qiniu.RPC;
using Qiniu.IO;
using Qiniu.RSF;

namespace Qiniu.Test
{
    class FetchDemo
    {
        public static void Main() {
            
            fetch();
        }

        /// <summary>
        /// 抓取资源
        /// </summary>
        /// <param name="fromUrl">需要抓取的文件URL</param>
        /// <param name="entryPath">目标entryPath</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>

        ///接口说明：http://developer.qiniu.com/code/v6/api/kodo-api/rs/fetch.html
        public static void fetch() {
            //初始化AK，SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";

            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPath pathPath = new EntryPath("bucketName", "saveKey"); // TODO: 初始化为适当的值
            CallRet actual = target.Fetch(fromUrl, pathPath);
            Console.WriteLine(actual.ToString());
            Console.WriteLine(actual.Response);
        }
    }
}
