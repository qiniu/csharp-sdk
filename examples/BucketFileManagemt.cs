using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Storage.Model;
using Qiniu.Http;

namespace QiniuDemo
{
    public class BucketFileManagemt
    {
        public static void stat()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";

            BucketManager bm = new BucketManager(mac);
            StatResult result = bm.stat(bucket, key);
        }

        public static void delete()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.delete(bucket, key);
        }

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

        public static void chgm()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string key = "KEY";
            string mimeType = "MIME_TYPE";

            BucketManager bm = new BucketManager(mac);
            HttpResult result = bm.chgm(bucket, key, mimeType);
        }

        public static void fetch()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "test";//TARGET_BUCKET";
            string saveKey = "hello.txt";// "SAVE_KEY";
            string localFile = "LOCAL_FILE";

            string remoteUrl = "REMOTE_URI";

            BucketManager bm = new BucketManager(mac);

            bm.fetch(remoteUrl, bucket, saveKey);

        }

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
    }
}