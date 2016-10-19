using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Storage.Model;
using Qiniu.Http;

namespace QiniuDemo
{
    /// <summary>
    /// 空间及空间文件管理
    /// </summary>
    public class BucketFileManagemt
    {

        /// <summary>
        /// 空间文件的stat(获取文件基本信息)操作
        /// </summary>
        public static void stat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";

            BucketManager bm = new BucketManager(mac);
            StatResult result = bm.stat(bucket, key);
        }

        /// <summary>
        /// 删除空间中指定文件
        /// </summary>
        public static void delete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.delete(bucket, key);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        public static void copy()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "SRC_BUCKET";
            string srcKey = "SRC_KEY";
            string dstBucket = "SRC_BUCKET";
            string dstKey = "DST_BUCKET";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.copy(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //HttpResult result = bm.copy(srcBucket, srcKey, dstBucket, dstKey, force);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        public static void move()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string srcBucket = "SRC_BUCKET";
            string srcKey = "SRC_KEY";
            string dstBucket = "SRC_BUCKET";
            string dstKey = "DST_BUCKET";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.move(srcBucket, srcKey, dstBucket, dstKey);

            //支持force参数, bool force = true/false
            //HttpResult result = bm.move(srcBucket, srcKey, dstBucket, dstKey, force);
        }

        /// <summary>
        /// 修改文件的MIME_TYPE
        /// </summary>
        public static void chgm()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";
            string mimeType = "MIME_TYPE";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.chgm(bucket, key, mimeType);
        }

        /// <summary>
        /// 拉取资源到空间
        /// </summary>
        public static void fetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string saveKey = "SAVE_KEY";
            string remoteUrl = "REMOTE_URI";

            BucketManager bm = new BucketManager(mac);
            bm.fetch(remoteUrl, bucket, saveKey);

        }

        /// <summary>
        /// 对于设置了镜像存储的空间，从镜像源站抓取指定名称的资源并存储到该空间中
        /// </summary>
        public static void prefetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";

            BucketManager bm = new BucketManager(mac);
            bm.prefetch(bucket, key);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        public static void batch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            // 批量操作类似于
            // op=<op1>&op=<op2>&op=<op3>...
            string batchOps = "BATCH_OPS";
            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.batch(batchOps);
            // 或者
            //string[] batch_ops={"<op1>","<op2>","<op3>",...};
            //bm.batch(batch_ops);

            System.Console.WriteLine(result.Response);
        }

        /// <summary>
        /// 列举所有的bucket
        /// </summary>
        public static void buckets()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            BucketManager bm = new BucketManager(mac);
            BucketsResult result = bm.buckets();

            foreach(string bucket in result.Buckets)
            {
                System.Console.WriteLine(bucket);
            }
        }

        /// <summary>
        /// 获取指定bucket对应的域名(可能不止一个),类似于abcxx.bkt.clouddn.com这样
        /// </summary>
        public static void domains()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";

            BucketManager bm = new BucketManager(mac);
            DomainsResult result = bm.domains(bucket);

            foreach(string domain in result.Domains)
            {
                System.Console.WriteLine(domain);
            }
        }
    }
}