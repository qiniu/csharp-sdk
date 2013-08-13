using System;
using Newtonsoft.Json;

namespace Qiniu.RS
{
	public class BatchRetItem
	{
		public int code;
		public BatchRetData data;
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class BatchRetData
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

		string error;

		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("error")]
		public string Error {
			get {
				return error;
			}
			set {
				error = value;
			}
 
		}
	}
}
