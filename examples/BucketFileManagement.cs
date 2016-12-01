using Qiniu.Util;
using Qiniu.Storage;
using Qiniu.Storage.Model;
using Qiniu.Http;
using System.Collections.Generic;

namespace CSharpSDKExamples
{
    /// <summary>
    /// 空间及空间文件管理
    /// </summary>
    public class BucketFileManagement
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

        /// <summary>
        /// 获取空间文件列表
        /// 
        /// BucketManager.listFiles(bucket, prefix, marker, limit, delimiter)
        /// 
        /// bucket:    目标空间名称
        /// 
        /// prefix:    返回指定文件名前缀的文件列表(prefix可设为null)
        /// 
        /// marker:    考虑到设置limit后返回的文件列表可能不全(需要重复执行listFiles操作)
        ///            执行listFiles操作时使用marker标记来追加新的结果
        ///            特别注意首次执行listFiles操作时marker为null   
        ///            
        /// limit:     每次返回结果所包含的文件总数限制(limit<=1000，建议值100) 
        /// 
        /// delimiter: 分隔符，比如-或者/等等，可以模拟作为目录结构(参考下述示例)
        ///            假设指定空间中有2个文件 fakepath/1.txt fakepath/2.txt
        ///            现设置分隔符delimiter = / 得到返回结果items =[]，commonPrefixes = [fakepath/]
        ///            然后调整prefix = fakepath/ delimiter = null 得到所需结果items = [1.txt,2.txt]
        ///            于是可以在本地先创建一个目录fakepath,然后在该目录下写入items中的文件  
        ///            
        /// </summary>
        public static void listFiles()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            string bucket = "BUCKET";
            string marker = ""; // 首次请求时marker必须为空
            string prefix = null; // 按文件名前缀保留搜索结果
            string delimiter = null; // 目录分割字符(比如"/")
            int limit = 100; // 最大值1000

            BucketManager bm = new BucketManager(mac);
            List<FileDesc> items = new List<FileDesc>();
            List<string> commonPrefixes = new List<string>();

            do
            {
                ListFilesResult result = bm.listFiles(bucket, prefix, marker, limit, delimiter);
                
                marker = result.Marker;
                
                if (result.Items != null)
                {
                    items.AddRange(result.Items);
                }

                if (result.CommonPrefixes != null)
                {
                    commonPrefixes.AddRange(result.CommonPrefixes);
                }

            } while (!string.IsNullOrEmpty(marker));

            foreach (string cp in commonPrefixes)
            {
                System.Console.WriteLine(cp);
            }

            foreach(var item in items)
            {
                System.Console.WriteLine(item.Key);
            }
        }
    }
}