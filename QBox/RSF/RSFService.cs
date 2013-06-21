using System;
using System.Collections.Generic;
using QBox.RPC;
using QBox.Conf;
using QBox.Auth;
using Newtonsoft.Json;
namespace QBox.RSF
{
	public class RSFService
	{
		public const int MAX_LIMIT = 10;

		//public Client Conn { get return;}
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

		public RSFService (string bucketName)
		{
			this.conn = new QBoxAuthClient();
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
		private DumpRet ListPrefix (string bucketName, string prefix, string markerIn, int limit=MAX_LIMIT)
		{
			string url = Config.RSF_HOST + string.Format ("/list?bucket={0}", bucketName);// + bucketName + 
			if (!string.IsNullOrEmpty (markerIn)) {
				url += string.Format ("&marker={0}", markerIn);
			}
			if (!string.IsNullOrEmpty (prefix)) {
				url += string.Format ("&prefix={0}", prefix);
			}
			if (limit > 0) {
				limit = limit > MAX_LIMIT ? MAX_LIMIT : limit;
				url += string.Format ("&limit={0}", limit);
			}
			try {
				CallRet ret = this.conn.Call (url);
				return JsonConvert.DeserializeObject<DumpRet> (ret.Response);
			} catch (Exception e) {	
				//LOG
				Console.WriteLine (string.Format ("listPrefix error => {0}", e.Message));
				return null;
			}
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

