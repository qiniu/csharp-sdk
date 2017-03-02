using System;
using System.Collections.Generic;
using Qiniu.Util;
using Qiniu.RS;
using Qiniu.RS.Model;
using Qiniu.Http;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 空间及空间文件管理
    /// </summary>
    public class BucketDemo
    {

        /// <summary>
        /// 空间文件的stat(获取文件基本信息)操作
        /// </summary>
        public static void stat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.mp4";

            BucketManager bm = new BucketManager(mac);

            StatResult result = bm.Stat(bucket, key);

            Console.WriteLine(result);
        }

        public static void batchStat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            int N = 5;

            // 待查询的N个文件keys
            string[] keys = new string[N];
            for (int i = 0; i < N; ++i)
            {
                keys[i] = string.Format("{0:D3}.txt", i + 1);
            }

            BucketManager bm = new BucketManager(mac);

            var result = bm.BatchStat(bucket, keys);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 删除空间中指定文件
        /// </summary>
        public static void delete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.txt";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Delete(bucket, key);

            Console.WriteLine(result);
        }

        public static void batchDelete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            int N = 5;

            // 待删除的N个文件keys
            string[] keys = new string[N];
            for(int i=0;i<N;++i)
            {
                keys[i] = string.Format("{0:D3}.txt", i + 1);
            }

            BucketManager bm = new BucketManager(mac);

            var result = bm.BatchDelete(bucket, keys);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        public static void copy()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "test";
            string srcKey = "1.txt";
            string dstBucket = "test";
            string dstKey = "2.txt";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Copy(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //var result = bm.Copy(srcBucket, srcKey, dstBucket, dstKey, force);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        public static void move()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "test";
            string srcKey = "1.txt";
            string dstBucket = "test";
            string dstKey = "2.txt";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Move(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //var result = bm.Move(srcBucket, srcKey, dstBucket, dstKey, force);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        public static void rename()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string srcKey = "1.txt";
            string dstKey = "2.txt";

            BucketManager bm = new BucketManager(mac);
            
            var result= bm.Rename(bucket, srcKey, dstKey);

            Console.WriteLine(result);
        }
        /// <summary>
        /// 修改文件的MIME_TYPE
        /// </summary>
        public static void chgm()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.txt";
            string mimeType = "text/html";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Chgm(bucket, key, mimeType);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 拉取资源到空间
        /// </summary>
        public static void fetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string saveKey = "1.jpg";
            string remoteUrl = "http://remote-url.com/file-name";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Fetch(remoteUrl, bucket, saveKey);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 对于设置了镜像存储的空间，从镜像源站抓取指定名称的资源并存储到该空间中
        /// </summary>
        public static void prefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.jpg";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Prefetch(bucket, key);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        public static void batch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            // 批量操作类似于
            // op=<op1>&op=<op2>&op=<op3>...
            string batchOps = "op=OP1&op=OP2";
            BucketManager bm = new BucketManager(mac);
            var result = bm.Batch(batchOps);
            // 或者
            //string[] batch_ops={"<op1>","<op2>","<op3>",...};
            //bm.Batch(batch_ops);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 列举所有的bucket
        /// </summary>
        public static void buckets()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager bm = new BucketManager(mac);

            var result = bm.Buckets();

            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取bucket信息
        /// </summary>
        public static void bucket()
        {
            string bkt = "test";

            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager bm = new BucketManager(mac);

            var result = bm.Bucket(bkt);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取指定bucket对应的域名(可能不止一个),类似于abcxx.bkt.clouddn.com这样
        /// </summary>
        public static void domains()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";

            BucketManager bm = new BucketManager(mac);

            var result = bm.Domains(bucket);

            Console.WriteLine(result);
        }

        /// <summary>
        /// 获取空间文件列表          
        /// </summary>
        public static void listFiles()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string marker = ""; // 首次请求时marker必须为空
            string prefix = null; // 按文件名前缀保留搜索结果
            string delimiter = null; // 目录分割字符(比如"/")
            int limit = 100; // 单次列举数量限制(最大值为1000)

            BucketManager bm = new BucketManager(mac);

            List<FileDesc> items = new List<FileDesc>();
            List<string> commonPrefixes = new List<string>();

            do
            {
                var result = bm.ListFiles(bucket, prefix, marker, limit, delimiter);

                Console.WriteLine(result);
                marker = result.Result.Marker;

            } while (!string.IsNullOrEmpty(marker));

            //foreach (string cp in commonPrefixes)
            //{
            //    Console.WriteLine(cp);
            //}

            //foreach(var item in items)
            //{
            //    Console.WriteLine(item.Key);
            //}
        }

        /// <summary>
        /// 更新文件的lifecycle
        /// </summary>
        public static void updateLifecycle()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";
            string key = "1.txt";

            // 新的deleteAfterDays，设置为0表示取消lifecycle
            int deleteAfterDays = 1;

            BucketManager bm = new BucketManager(mac);

            var result = bm.UpdateLifecycle(bucket, key, deleteAfterDays);

            Console.WriteLine(result);
        }
    }
}