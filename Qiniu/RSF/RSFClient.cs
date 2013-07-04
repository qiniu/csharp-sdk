using System;
using System.Collections.Generic;
using Qiniu.RPC;
using Qiniu.Conf;
using Qiniu.Auth;
using Newtonsoft.Json;
namespace Qiniu.RSF
{
    /// <summary>
    /// RS Fetch 
    /// </summary>
    public class RSFClient : QiniuAuthClient
    {
        
        private const int MAX_LIMIT = 1000;
        //失败重试次数
        private const int RETRY_TIME = 3;
        private string bucketName;
        /// <summary>
        /// bucket name
        /// </summary>
        public string BucketName { get; private set; }

        private int limit;
        /// <summary>
        /// Fetch返回结果条目数量限制
        /// </summary>
        public int Limit
        {
            get
            {
                return limit;
            }
            set
            {
                limit = value > MAX_LIMIT ? MAX_LIMIT : value;
            }
        }

        private bool end = false;

        private string prefix;
        /// <summary>
        /// 文件前缀
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
            }
        }

        private string marker;
        /// <summary>
        /// Fetch 定位符.
        /// </summary>
        public string Marker
        {
            get
            {
                return marker;
            }
            set
            {
                marker = value;
            }
        }
        /// <summary>
        /// RS Fetch Client
        /// </summary>
        /// <param name="bucketName">七牛云存储空间名称</param>
        public RSFClient(string bucketName)
        {
            this.bucketName = bucketName;
        }
        /// <summary>
        /// The origin Fetch interface,we recomment to use Next().
        /// </summary>
        /// <returns>
        /// Dump
        /// </returns>
        /// <param name='bucketName'>
        /// Bucket name.
        /// </param>
        /// <param name='prefix'>
        /// Prefix.
        /// </param>
        /// <param name='markerIn'>
        /// Marker in.
        /// </param>
        /// <param name='limit'>
        /// Limit.
        /// </param>
        public DumpRet ListPrefix(string bucketName, string prefix="", string markerIn="", int limit = MAX_LIMIT)
        {
            string url = Config.RSF_HOST + string.Format("/list?bucket={0}", bucketName);// + bucketName + 
            if (!string.IsNullOrEmpty(markerIn))
            {
                url += string.Format("&marker={0}", markerIn);
            }
            if (!string.IsNullOrEmpty(prefix))
            {
                url += string.Format("&prefix={0}", prefix);
            }
            if (limit > 0)
            {
                limit = limit > MAX_LIMIT ? MAX_LIMIT : limit;
                url += string.Format("&limit={0}", limit);
            }
            for (int i = 0; i < RETRY_TIME; i++)
            {
                CallRet ret = Call(url);
                if (ret.OK)
                {
                    return JsonConvert.DeserializeObject<DumpRet>(ret.Response);
                }
                else
                {
                    Console.WriteLine("listPrefix fail ===> {0}", ret.Exception.Message);
                }
            }
            return null;
        }
        /// <summary>
        /// call this func before invoke Next()
        /// </summary>
        public void Init()
        {
            end = false;
            this.marker = string.Empty;
        }
        /// <summary>
        /// Next.
        /// <example>
        /// <code>
        /// public static void List (string bucket)
		///{
        ///     RSF.RSFClient rsf = new Qiniu.RSF.RSFClient(bucket);
        ///     rsf.Prefix = "test";
        ///     rsf.Limit = 100;
        ///     List<DumpItem> items;
        ///     while ((items=rsf.Next())!=null)
        ///     {                
        ///      //todo
        ///     }
        ///}s
        /// </code>
        /// </example>
        /// </summary>
        public List<DumpItem> Next()
        {
            if (end)
            {
                return null;
            }
            DumpRet ret = ListPrefix(this.bucketName, this.prefix, this.marker, this.limit);
            if (ret.Items.Count == 0)
            {
                end = true;
                return null;
            }
            this.marker = ret.Marker;
            end = ret.Items.Count < this.limit;
            return ret.Items;
        }
    }
}

