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
    class BatchDemo
    {
        public static void Main() {
            batchCopy()；
        }

        /// <summary>
        /// 批量复制
        /// </summary>
        /// <param name="entryPathPari"></param>
        /// <param name="force"></param>
        /// <returns></returns>

        /// 接口说明 http://developer.qiniu.com/code/v6/api/kodo-api/rs/batch.html
        /// BatchMove, BatchCopy, BatchDelete, BatchStat处理相同
        public static void batchCopy()
        {
            //初始化AK，SK
            Qiniu.Conf.Config.ACCESS_KEY = "Access_Key";
            Qiniu.Conf.Config.SECRET_KEY = "Secret_Key";

            RSClient target = new RSClient();

            List<EntryPathPair> list = new List<EntryPathPair>();

            EntryPathPair[] pathPairs = new EntryPathPair[2];

            EntryPathPair pathPair1 = new EntryPathPair(bucketSrc, keySrc, bucketDest, keyDest);
            EntryPathPair pathPair2 = new EntryPathPair(bucketSrc2, keySrc2, bucketDest2, keyDest2);

            pathPairs[0] = pathPair1;
            pathPairs[1] = pathPair2;

            CallRet actual = target.BatchCopy(pathPairs, true);
            Console.WriteLine(actual.ToString());
            Console.WriteLine(actual.Response);

        }
    
    }
}
