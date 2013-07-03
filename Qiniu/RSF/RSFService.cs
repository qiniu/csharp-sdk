using System;
using System.Collections.Generic;
using QBox.RPC;
using QBox.Conf;
using QBox.Auth;
using Newtonsoft.Json;
namespace QBox.RSF
{
	public class RSFClient:QBoxAuthClient
	{
        //
		public const int MAX_LIMIT = 10;
        //失败重试次数
        public const int RETRY_TIME = 3;
		private Client conn;
		private string bucketName;
        public string BucketName { get; private set; }

		private int limit;
		public int Limit {
			get {
				return limit;
			}
			set {
				limit = value>MAX_LIMIT?MAX_LIMIT:value;
			}
		}

		private bool end=false;

		private string prefix;
		/// <summary>
		/// 文件前缀
		/// </summary>
		/// <value>
		/// The prefix.
		/// </value>
		public string Prefix {
			get {
				return prefix;
			}
			set {
				prefix = value;
			}
		}

		private string marker;
		/// <summary>
		/// Fetch 定位符.
		/// </summary>
		public string Marker {
			get {
				return marker;
			}
			set {
				marker = value;
			}
		}

		public RSFClient (string bucketName)
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
        public DumpRet ListPrefix(string bucketName, string prefix, string markerIn, int limit = MAX_LIMIT)
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
        /// call this func before you Invoke Next()
        /// </summary>
        public void Init()
        {
            end = false;
            this.marker = string.Empty; 
        }
		/// <summary>
		/// Next this instance.
		/// </summary>
		public List<DumpItem> Next()
		{
			if (end) {
				return null;
			}
			DumpRet ret = ListPrefix (this.bucketName, this.prefix, this.marker,this.limit);
			if (ret.Items.Count == 0) {
				end = true;
				return null;
			}				
			this.marker = ret.Marker;
			end = ret.Items.Count < MAX_LIMIT;
			return ret.Items;
		}

	}
}

