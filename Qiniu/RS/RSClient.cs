using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Auth.digest;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.Util;
#if ABOVE45
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif

namespace Qiniu.RS
{
	/// <summary>
	/// 文件管理操作
	/// </summary>
	public enum FileHandle
	{
		/// <summary>
		/// 查看
		/// </summary>
		STAT = 0,
		/// <summary>
		/// 移动move
		/// </summary>
		MOVE,
		/// <summary>
		/// 复制copy
		/// </summary>
		COPY,
		/// <summary>
		/// 删除delete
		/// </summary>
		DELETE,
		/// <summary>
		/// 抓取资源fetch
		/// </summary>
		FETCH
	}

	/// <summary>
	/// 资源存储客户端，提供对文件的查看（stat），移动(move)，复制（copy）,删除（delete）, 抓取资源（fetch） 操作
	/// 以及与这些操作对应的批量操作
	/// </summary>
	public class RSClient : QiniuAuthClient
	{
		private static string[] OPS = new string[] { "stat", "move", "copy", "delete", "fetch" };

		public RSClient(Mac mac = null)
			: base(mac)
		{
		}

#if !ABOVE45
        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        private CallRet op (FileHandle op, EntryPath scope)
		{
			string url = string.Format ("{0}/{1}/{2}",
			                            Config.RS_HOST,
			                            OPS [(int)op],
			                            Base64URLSafe.Encode (scope.URI));
			return Call (url);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="op"></param>
		/// <param name="pair"></param>
		/// <returns></returns>
		private CallRet op2 (FileHandle op, EntryPathPair pair)
		{
			string url = string.Format ("{0}/{1}/{2}/{3}",
			                            Config.RS_HOST,
			                            OPS [(int)op],
			                            Base64URLSafe.Encode (pair.URISrc),
			                            Base64URLSafe.Encode (pair.URIDest));
			return Call(url);
		}


		/// <summary>
        /// </summary>
        /// <param name="op"></param>
        /// <param name="pair"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        private CallRet op2(FileHandle op, EntryPathPair pair, bool force)
        {

            string url = string.Format("{0}/{1}/{2}/{3}/force/{4}",
                                        Config.RS_HOST,
                                        OPS[(int)op],
                                        Base64URLSafe.Encode(pair.URISrc),
                                        Base64URLSafe.Encode(pair.URIDest), force);
            return Call(url);
        }

		private CallRet opFetch(FileHandle op, string fromUrl, EntryPath entryPath)
		{
			string url = string.Format("{0}/{1}/{2}/to/{3}",
										Config.PREFETCH_HOST,
										OPS[(int)op],
										Base64URLSafe.Encode(fromUrl),
										Base64URLSafe.Encode(entryPath.URI));
			return Call (url);
		}

		/// <summary>
		/// 文件信息查看
		/// </summary>
		/// <param name="scope"></param>
		/// <returns>文件的基本信息，见<see cref="Entry">Entry</see></returns>
		public Entry Stat (EntryPath scope)
		{
			CallRet callRet = op (FileHandle.STAT, scope);
			return new Entry (callRet);
		}

		/// <summary>
		/// 删除文件
		/// </summary>
		/// <param name="bucket">七牛云存储空间名称</param>
		/// <param name="key">需要删除的文件key</param>
		/// <returns></returns>
		public CallRet Delete (EntryPath scope)
		{
			CallRet callRet = op (FileHandle.DELETE, scope);
			return new Entry (callRet);
		}

		/// <summary>
		/// 移动文件
		/// </summary>
		/// <param name="bucketSrc">文件所属的源空间名称</param>
		/// <param name="keySrc">源key</param>
		/// <param name="bucketDest">目标空间名称</param>
		/// <param name="keyDest">目标key</param>
		/// <returns>见<see cref="CallRet">CallRet</see></returns>
		public CallRet Move (EntryPathPair pathPair)
		{
			return op2 (FileHandle.MOVE, pathPair);
		}


		/// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="bucketSrc">文件所属的源空间名称</param>
        /// <param name="keySrc">源key</param>
        /// <param name="bucketDest">目标空间名称</param>
        /// <param name="keyDest">目标key</param>
        /// <param name="force">强制覆盖</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public CallRet Move(EntryPathPair pathPair, bool force)
        {
            return op2(FileHandle.MOVE, pathPair, force);
        }

		/// <summary>
		/// 复制
		/// </summary>
		/// <param name="bucketSrc">文件所属的空间名称</param>
		/// <param name="keySrc">需要复制的文件key</param>
		/// <param name="bucketDest">复制至目标空间</param>
		/// <param name="keyDest">复制的副本文件key</param>
		/// <returns>见<see cref="CallRet">CallRet</see></returns>
		public CallRet Copy (EntryPathPair pathPair)
		{
			return op2 (FileHandle.COPY, pathPair);
		}

		/// <summary>
        /// 复制
        /// </summary>
        /// <param name="bucketSrc">文件所属的空间名称</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">复制至目标空间</param>
        /// <param name="keyDest">复制的副本文件key</param>
        /// <param name="force">复制是否强制覆盖目标文件</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public CallRet Copy(EntryPathPair pathPair, bool force)
        {

            return op2(FileHandle.COPY, pathPair, force);

        }

		/// <summary>
		/// 抓取资源
		/// </summary>
		/// <param name="fromUrl">需要抓取的文件URL</param>
		/// <param name="entryPath">目标entryPath</param>
		/// <returns>见<see cref="CallRet">CallRet</see></returns>
		public CallRet Fetch(string fromUrl, EntryPath entryPath)
		{
			return opFetch(FileHandle.FETCH, fromUrl, entryPath);
		}
#else
        private async Task<CallRet> opAsync(FileHandle op, EntryPath scope)
        {
            string url = string.Format("{0}/{1}/{2}",
                                        Config.RS_HOST,
                                        OPS[(int)op],
                                        Base64URLSafe.Encode(scope.URI));
            return await CallAsync(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        private async Task<CallRet> op2Async(FileHandle op, EntryPathPair pair)
        {
            string url = string.Format("{0}/{1}/{2}/{3}",
                                        Config.RS_HOST,
                                        OPS[(int)op],
                                        Base64URLSafe.Encode(pair.URISrc),
                                        Base64URLSafe.Encode(pair.URIDest));
            return await CallAsync(url);
        }


        /// <summary>
        /// </summary>
        /// <param name="op"></param>
        /// <param name="pair"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        private async Task<CallRet> op2Async(FileHandle op, EntryPathPair pair, bool force)
        {

            string url = string.Format("{0}/{1}/{2}/{3}/force/{4}",
                                        Config.RS_HOST,
                                        OPS[(int)op],
                                        Base64URLSafe.Encode(pair.URISrc),
                                        Base64URLSafe.Encode(pair.URIDest), force);
            return await CallAsync(url);
        }

        private async Task<CallRet> opFetchAsync(FileHandle op, string fromUrl, EntryPath entryPath)
        {
            string url = string.Format("{0}/{1}/{2}/to/{3}",
                                        Config.PREFETCH_HOST,
                                        OPS[(int)op],
                                        Base64URLSafe.Encode(fromUrl),
                                        Base64URLSafe.Encode(entryPath.URI));
            return await CallAsync(url);
        }

        /// <summary>
        /// 文件信息查看
        /// </summary>
        /// <param name="scope"></param>
        /// <returns>文件的基本信息，见<see cref="Entry">Entry</see></returns>
        public async Task<Entry> StatAsync(EntryPath scope)
        {
            CallRet callRet = await opAsync(FileHandle.STAT, scope);
            return new Entry(callRet);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="bucket">七牛云存储空间名称</param>
        /// <param name="key">需要删除的文件key</param>
        /// <returns></returns>
        public async Task<CallRet> DeleteAsync(EntryPath scope)
        {
            CallRet callRet = await opAsync(FileHandle.DELETE, scope);
            return new Entry(callRet);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="bucketSrc">文件所属的源空间名称</param>
        /// <param name="keySrc">源key</param>
        /// <param name="bucketDest">目标空间名称</param>
        /// <param name="keyDest">目标key</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public async Task<CallRet> MoveAsync(EntryPathPair pathPair)
        {
            return await op2Async(FileHandle.MOVE, pathPair);
        }


        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="bucketSrc">文件所属的源空间名称</param>
        /// <param name="keySrc">源key</param>
        /// <param name="bucketDest">目标空间名称</param>
        /// <param name="keyDest">目标key</param>
        /// <param name="force">强制覆盖</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public async Task<CallRet> MoveAsync(EntryPathPair pathPair, bool force)
        {
            return await op2Async(FileHandle.MOVE, pathPair, force);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="bucketSrc">文件所属的空间名称</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">复制至目标空间</param>
        /// <param name="keyDest">复制的副本文件key</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public async Task<CallRet> CopyAsync(EntryPathPair pathPair)
        {
            return await op2Async(FileHandle.COPY, pathPair);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="bucketSrc">文件所属的空间名称</param>
        /// <param name="keySrc">需要复制的文件key</param>
        /// <param name="bucketDest">复制至目标空间</param>
        /// <param name="keyDest">复制的副本文件key</param>
        /// <param name="force">复制是否强制覆盖目标文件</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public async Task<CallRet> CopyAsync(EntryPathPair pathPair, bool force)
        {

            return await op2Async(FileHandle.COPY, pathPair, force);

        }

        /// <summary>
        /// 抓取资源
        /// </summary>
        /// <param name="fromUrl">需要抓取的文件URL</param>
        /// <param name="entryPath">目标entryPath</param>
        /// <returns>见<see cref="CallRet">CallRet</see></returns>
        public async Task<CallRet> FetchAsync(string fromUrl, EntryPath entryPath)
        {
            return await opFetchAsync(FileHandle.FETCH, fromUrl, entryPath);
        }
#endif


        /// <summary>
        /// 获取一元批操作http request Body
        /// </summary>
        /// <param name="opName">操作名</param>
        /// <param name="keys">操作对象keys</param>
        /// <returns>Request Body</returns>
        private string getBatchOp_1 (FileHandle op, EntryPath[] keys)
		{
			if (keys.Length < 1)
				return string.Empty;
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < keys.Length - 1; i++) {
				string item = string.Format ("op=/{0}/{1}&",
				                             OPS [(int)op], 
				                             Base64URLSafe.Encode (keys [i].URI));
				sb.Append (item);
			}
			string litem = string.Format ("op=/{0}/{1}", OPS [(int)op], Base64URLSafe.Encode (keys [keys.Length - 1].URI));
			return sb.Append (litem).ToString ();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="opName"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		private string getBatchOp_2 (FileHandle op, EntryPathPair[] keys)
		{
			if (keys.Length < 1)
				return string.Empty;
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < keys.Length - 1; i++) {
				string item = string.Format ("op=/{0}/{1}/{2}&", 
				                             OPS [(int)op],
				                             Base64URLSafe.Encode (keys [i].URISrc),
				                             Base64URLSafe.Encode (keys [i].URIDest));
				sb.Append (item);
			}
			string litem = string.Format ("op=/{0}/{1}/{2}", OPS [(int)op],
			                              Base64URLSafe.Encode (keys [keys.Length - 1].URISrc),
			                              Base64URLSafe.Encode (keys [keys.Length - 1].URIDest));
			return sb.Append (litem).ToString ();
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <param name="keys"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        private string getBatchOp_2(FileHandle op, EntryPathPair[] keys, bool force)
        {
            if (keys.Length < 1)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < keys.Length - 1; i++)
            {
                string item = string.Format("op=/{0}/{1}/{2}/force/{3}/&",
                                             OPS[(int)op],
                                             Base64URLSafe.Encode(keys[i].URISrc),
                                             Base64URLSafe.Encode(keys[i].URIDest), force);
                sb.Append(item);
            }
            string litem = string.Format("op=/{0}/{1}/{2}/force/{3}", OPS[(int)op],
                                          Base64URLSafe.Encode(keys[keys.Length - 1].URISrc),
                                          Base64URLSafe.Encode(keys[keys.Length - 1].URIDest), force);
            return sb.Append(litem).ToString();
        }

#if !ABOVE45
        private CallRet batch(string requestBody)
        {
            return CallWithBinary(Conf.Config.RS_HOST + "/batch", "application/x-www-form-urlencoded", StreamEx.ToStream(requestBody), requestBody.Length);
        }

        /// <summary>
        /// 批操作：文件信息查看
        /// <example>
        /// <code>
        /// public static void BatchStat(string bucket, string[] keys)
        ///{
        ///    RSClient client = new RSClient();
        ///    List&lt;Scope> scopes= new List&lt;Scope>();
        ///    foreach(string key in keys)
        ///    {
        ///        Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
        ///        scopes.Add(new Scope(bucket,key));
        ///    }
        ///    client.BatchStat(scopes.ToArray()); 
        ///}
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="keys">文件bucket+key,see<see cref="Scope"/></param>
        /// <returns></returns>
        public List<BatchRetItem> BatchStat (EntryPath[] keys)
		{
			string requestBody = getBatchOp_1 (FileHandle.STAT, keys);
			CallRet ret = batch (requestBody);
			if (ret.OK) {
				List<BatchRetItem> items = JsonConvert.DeserializeObject<List<BatchRetItem>> (ret.Response);
				return items;
			}
			return null;
		}

        /// <summary>
        /// 批操作：文件移动
        /// </summary>
        /// <param name="entryPathPair"><see cref="EntryPathPair"/></param>
        public CallRet BatchMove (EntryPathPair[] entryPathPairs)
		{
			string requestBody = getBatchOp_2 (FileHandle.MOVE, entryPathPairs);
			return batch (requestBody);
		}

		/// <summary>
        /// batch
        /// </summary>
        /// <param name="entryPathPairs"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public CallRet BatchMove(EntryPathPair[] entryPathPairs, bool force)
        {
            string requestBody = getBatchOp_2(FileHandle.MOVE, entryPathPairs, force);
            return batch(requestBody);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bucket"></param>
		/// <param name="entryPathPari"></param>
		public CallRet BatchCopy (EntryPathPair[] entryPathPari)
		{
			string requestBody = getBatchOp_2 (FileHandle.COPY, entryPathPari);
			return batch (requestBody);
		}

		/// <summary>
        /// 批量复制
        /// </summary>
        /// <param name="entryPathPari"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public CallRet BatchCopy(EntryPathPair[] entryPathPari, bool force)
        {
            string requestBody = getBatchOp_2(FileHandle.COPY, entryPathPari, force);
            return batch(requestBody);
        }
		
		/// <summary>
		/// 批量删除
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public CallRet BatchDelete (EntryPath[] keys)
		{
			string requestBody = getBatchOp_1 (FileHandle.DELETE, keys);
			return batch (requestBody);
		}
#else
        private async Task<CallRet> batchAsync(string requestBody)
        {
            var content = new StringContent(requestBody);
            // StringContent 的 ContentType 默认是 text/plain；用 FormUrlEncodedContent 也是可以，但这样就要改动的地方较多。
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return await CallWithBinaryAsync(Conf.Config.RS_HOST + "/batch", content);
        }

        /// <summary>
        /// 批操作：文件信息查看
        /// <example>
        /// <code>
        /// public static async void BatchStat(string bucket, string[] keys)
        ///{
        ///    RSClient client = new RSClient();
        ///    List<Scope> scopes= new List<Scope>();
        ///    foreach(string key in keys)
        ///    {
        ///        Console.WriteLine("\n===> Stat {0}:{1}", bucket, key);
        ///        scopes.Add(new Scope(bucket,key));
        ///    }
        ///    await client.BatchStat(scopes.ToArray()); 
        ///}
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="keys">文件bucket+key,see<see cref="Scope"/></param>
        /// <returns></returns>
        public async Task<List<BatchRetItem>> BatchStatAsync(EntryPath[] keys)
        {
            string requestBody = getBatchOp_1(FileHandle.STAT, keys);
            CallRet ret = await batchAsync(requestBody);
            if (ret.OK)
            {
                List<BatchRetItem> items = JsonConvert.DeserializeObject<List<BatchRetItem>>(ret.Response);
                return items;
            }
            return null;
        }

        /// <summary>
        /// 批操作：文件移动
        /// </summary>
        /// <param name="entryPathPair"><see cref="">EntryPathPair</see></param>
        public async Task<CallRet> BatchMoveAsync(EntryPathPair[] entryPathPairs)
        {
            string requestBody = getBatchOp_2(FileHandle.MOVE, entryPathPairs);
            return await batchAsync(requestBody);
        }

        /// <summary>
        /// batch
        /// </summary>
        /// <param name="entryPathPairs"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public async Task<CallRet> BatchMoveAsync(EntryPathPair[] entryPathPairs, bool force)
        {
            string requestBody = getBatchOp_2(FileHandle.MOVE, entryPathPairs, force);
            return await batchAsync(requestBody);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="entryPathPari"></param>
        public async Task<CallRet> BatchCopyAsync(EntryPathPair[] entryPathPari)
        {
            string requestBody = getBatchOp_2(FileHandle.COPY, entryPathPari);
            return await batchAsync(requestBody);
        }

        /// <summary>
        /// 批量复制
        /// </summary>
        /// <param name="entryPathPari"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        public async Task<CallRet> BatchCopyAsync(EntryPathPair[] entryPathPari, bool force)
        {
            string requestBody = getBatchOp_2(FileHandle.COPY, entryPathPari, force);
            return await batchAsync(requestBody);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<CallRet> BatchDeleteAsync(EntryPath[] keys)
        {
            string requestBody = getBatchOp_1(FileHandle.DELETE, keys);
            return await batchAsync(requestBody);
        }
#endif
    }
}
