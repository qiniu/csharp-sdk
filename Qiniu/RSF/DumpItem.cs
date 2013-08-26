using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Qiniu.RSF
{
	/// <summary>
	/// Dump item文件信息.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class DumpItem
	{
		Int64 fSize;

		/// <summary>
		/// 文件大小.
		/// </summary>
		[JsonProperty("fsize")]
		public Int64 FSize {
			get {
				return fSize;
			}
			set {
				fSize = value;
			}
		}

		Int64 putTime;

		/// <summary>
		/// 修改时间.
		/// </summary>
		[JsonProperty("putTime")]
		public Int64 PutTime {
			get {
				return putTime;
			}
			set {
				putTime = value;
			}
		}

		string key;

		/// <summary>
		/// 文件名.
		/// </summary>
		[JsonProperty("key")]
		public string Key {
			get {
				return key;
			}
			set {
				key = value;
			}
		}

		string hash;

		/// <summary>
		/// Gets a value indicating whether this instance hash.
		/// </summary>
		[JsonProperty("hash")]
		public string Hash {
			get {
				return hash;
			}
			set {
				hash = value;
			}
		}

		string mime;

		/// <summary>
		/// Gets the MIME.
		/// </summary>
		[JsonProperty("mimeType")]
		public string Mime {
			get {
				return mime;
			}
			set {
				mime = value;
			}
		}

		string endUser;

		public string EndUser {
			get {
				return endUser;
			}
			set {
				endUser = value;
			}
		}
	}

	/// <summary>
	/// Fetch 返回结果.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class DumpRet
	{
		string marker;

		/// <summary>
		/// fetch 定位符
		/// </summary>
		[JsonProperty("marker")]
		public string Marker {
			get {
				return marker;
			}
			set {
				marker = value;
			}
		}

		List<DumpItem> items;

		/// <summary>
		/// The items.
		/// </summary>
		[JsonProperty("items")]
		public List<DumpItem> Items {
			get {
				return items;
			}
			set {
				items = value;
			}
		}
	}
}

