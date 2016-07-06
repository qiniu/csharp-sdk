using System;
using System.Collections.Generic;
using Qiniu.Conf;
using Qiniu.RS;
using Qiniu.RPC;
using Qiniu.IO;
using Qiniu.RSF;

namespace Qiniu.Test
{
    class ListDemo
    {
        public static void Main() {
            list();
        }

        /// <summary>
        /// The origin Fetch interface,we recomment to use Next().
        /// </summary>
        /// <returns>
        /// Dump
        /// </returns>
        /// <param name='bucketName'>
        /// 空间名
        /// </param>
        /// <param name='prefix'>
        /// 匹配前缀字符
        /// </param>
        /// <param name='markerIn'>
        /// 上一次列举返回的位置标记，作为本次列举的起点信息,默认值为空字符串。
        /// </param>
        /// <param name='limit'>
        /// 本次列举的条目数，范围为1-1000,缺省值为1000。
        /// </param>
        /// 接口说明：http://developer.qiniu.com/code/v6/api/kodo-api/rs/list.html
        public static void list() {
            //初始化AK，SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";

            String bucket = "Bucket_Name";
            String prefix = "prefix";
            String markerIn = "markerIn";
            String limit = "limit";

            RSFClient target = new RSFClient(bucket);
            DumpRet actual;
            actual = target.ListPrefix(bucket, prefix, markerIn, limit);
            foreach (DumpItem item in actual.Items)
            {
                Console.WriteLine("Key:{0},Hash:{1},Mime:{2},PutTime:{3},EndUser:{4}", item.Key, item.Hash, item.Mime, item.PutTime, item.EndUser);
            }
        }
    }
}
