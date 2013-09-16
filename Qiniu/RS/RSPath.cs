namespace Qiniu.RS
{
	/// <summary>
	/// bucket+   ":"+ key
	/// </summary>
	public class EntryPath
	{
		private string bucket;

		/// <summary>
		/// 七年云存储空间名
		/// </summary>
		public string Bucket {
			get { return bucket; }
		}

		private string key;

		/// <summary>
		/// 文件key
		/// </summary>
		public string Key {
			get { return key; }
		}

		private string uri;

		/// <summary>
		/// bucket+ ":"+ key
		/// </summary>
		public string URI { get { return this.uri; } }

		public string Base64EncodedURI { get { return Qiniu.Util.Base64URLSafe.Encode (this.uri); } }

		public EntryPath (string bucket, string key)
		{
			this.bucket = bucket;
			this.key = key;
			this.uri = this.bucket + ":" + Key;
		}
	}

	/// <summary>
	/// 二元操作路径
	/// </summary>
	public class EntryPathPair
	{
		private EntryPath src;
		private EntryPath dest;

		private void _entryPathPair (string bucketSrc, string keySrc, string bucketDest, string keyDest)
		{
			this.src = new EntryPath (bucketSrc, keySrc);
			this.dest = new EntryPath (bucketDest, keyDest);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.RS.EntryPathPair"/> class.
		/// </summary>
		public EntryPathPair (EntryPath src, EntryPath des)
		{
			this.src = src;
			this.dest = des;
		}

		/// <summary>
		/// 二元操作路径构造函数
		/// </summary>
		/// <param name="bucketSrc">源空间名称</param>
		/// <param name="keySrc">源文件key</param>
		/// <param name="bucketDest">目标空间名称</param>
		/// <param name="keyDest">目文件key</param>
		public EntryPathPair (string bucketSrc, string keySrc, string bucketDest, string keyDest)
		{
			_entryPathPair (bucketSrc, keySrc, bucketDest, keyDest);
		}

		/// <summary>
		/// 二元操作路径构造函数
		/// </summary>
		/// <param name="bucket">源空间名称，目标空间名称</param>
		/// <param name="keySrc">源文件key</param>
		/// <param name="keyDest">目文件key</param>
		public EntryPathPair (string bucket, string keySrc, string keyDest)
		{
			_entryPathPair (bucket, keySrc, bucket, keyDest);
		}

		/// <summary>
		/// bucketSrc+":"+keySrc
		/// </summary>
		public string URISrc { get { return src.URI; } }

		/// <summary>
		/// bucketDest+":"+keyDest
		/// </summary>
		public string URIDest { get { return dest.URI; } }
	}
}
