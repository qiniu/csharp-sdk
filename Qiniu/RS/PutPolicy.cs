using System;
using Newtonsoft.Json;
using Qiniu.Auth.digest;
using Qiniu.Conf;
using Qiniu.Util;

namespace Qiniu.RS
{
	/// <summary>
	/// PutPolicy
	/// </summary>
	[JsonObject(MemberSerialization.OptIn)]
	public class PutPolicy
	{
		private string scope;
		private string callBackUrl;
		private string callBackBody;
		private string returnUrl;
		private string returnBody;
		private string asyncOps;
		private string saveKey;
		private int insertOnly;
		private int detectMime;
		private long fsizeLimit;
		private string persistentOps;
		private string persistentNotifyUrl;
		private string endUser;
		private UInt64 expires = 3600;
		private UInt64 deadline = 0;

		/// <summary>
		/// 一般指文件要上传到的目标存储空间（Bucket）。若为”Bucket”，表示限定只能传到该Bucket（仅限于新增文件）；若为”Bucket:Key”，表示限定特定的文件，可修改该文件。
		/// </summary>
		[JsonProperty("scope")]
		public string Scope {
			get { return scope; }
			set { scope = value; }
		}

		/// <summary>
		/// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的URL，必须是公网上可以正常进行POST请求并能响应 HTTP Status 200 OK 的有效 URL
		/// </summary>
		[JsonProperty("callBackUrl")]
		public string CallBackUrl {
			get { return  callBackUrl; }
			set { callBackUrl = value; }
		}

		/// <summary>
		/// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的数据。支持 魔法变量 和 自定义变量，不可与 returnBody 同时使用。
		/// </summary>
		[JsonProperty("callBackBody")]
		public string CallBackBody {
			get { return callBackBody; }
			set { callBackBody = value; }
		}

		/// <summary>
		/// 设置用于浏览器端文件上传成功后，浏览器执行301跳转的URL，一般为 HTML Form 上传时使用。文件上传成功后会跳转到 returnUrl?query_string, query_string 会包含 returnBody 内容。returnUrl 不可与 callbackUrl 同时使用
		/// </summary>
		[JsonProperty("returnUrl")]
		public string ReturnUrl {
			get { return returnUrl;  }
			set { returnUrl = value; }
		}

		/// <summary>
		/// 文件上传成功后，自定义从 Qiniu-Cloud-Server 最终返回給终端 App-Client 的数据。支持 魔法变量，不可与 callbackBody 同时使用。
		/// </summary>    
		[JsonProperty("returnBody")]
		public string ReturnBody {
			get { return returnBody;  }
			set { returnBody = value; }
		}

		/// <summary>
		/// 指定文件（图片/音频/视频）上传成功后异步地执行指定的预转操作。每个预转指令是一个API规格字符串，多个预转指令可以使用分号“;”隔开
		/// </summary>
		[JsonProperty("asyncOps")]
		public string AsyncOps {
			get { return asyncOps; }
			set { asyncOps = value; }
		}

		/// <summary>
		/// 给上传的文件添加唯一属主标识，特殊场景下非常有用，比如根据终端用户标识给图片或视频打水印
		/// </summary>
		[JsonProperty("endUser")]
		public string EndUser {
			get { return endUser; }
			set { endUser = value; }
		}

		/// <summary>
		/// 定义 uploadToken 的失效时间，Unix时间戳，精确到秒，缺省为 3600 秒
		/// </summary>
		[JsonProperty("deadline")]
		public UInt64 Deadline {
			get { return deadline;  }
		}

		/// <summary>
		/// 可选, Gets or sets the save key.
		/// </summary>
		/// <value>The save key.</value>
		[JsonProperty("saveKey")]
		public string SaveKey {
			get {
				return saveKey;
			}
			set{
				saveKey = value; 
			}
		}

		/// <summary>
		/// 可选。 若非0, 即使Scope为 Bucket:Key 的形式也是insert only.
		/// </summary>
		/// <value>The insert only.</value>
		[JsonProperty("insertOnly")]
		public int InsertOnly {
			get {
				return insertOnly;
			}
			set{ 
				insertOnly = value;
			}
		}

		/// <summary>
		/// 可选。若非0, 则服务端根据内容自动确定 MimeType */
		/// </summary>
		/// <value>The detect MIME.</value>
		[JsonProperty("detectMime")]
		public int DetectMime {
			get {
				return detectMime;
			}
			set{
				detectMime = value; 
			}
		}

		/// <summary>
		/// 可选, Gets or sets the fsize limit.
		/// </summary>
		/// <value>The fsize limit.</value>
		[JsonProperty("fsizeLimit")]
		public long FsizeLimit {
			get {
				return fsizeLimit;
			}
			set{ 
				fsizeLimit = value;
			}
		}

		/// <summary>
		/// 音视频转码持久化完成后，七牛的服务器会向用户发送处理结果通知。这里指定的url就是用于接收通知的接口。设置了`persistentOps`,则需要同时设置此字段
		/// </summary>
		[JsonProperty("persistentNotifyUrl")]
		public string PersistentNotifyUrl {
			get { return persistentNotifyUrl;  }
			set { persistentNotifyUrl = value; }
		}

		/// <summary>
		/// 可指定音视频文件上传完成后，需要进行的转码持久化操作。asyncOps的处理结果保存在缓存当中，有可能失效。而persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
		/// </summary>
		[JsonProperty("persistentOps")]
		public string PersistentOps {
			get { return persistentOps;  }
			set { persistentOps = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.RS.PutPolicy"/> class.
		/// </summary>
		/// <param name="scope">Scope.</param>
		/// <param name="expires">Expires.</param>
		public PutPolicy (string scope, UInt32 expires=3600)
		{
			Scope = scope;
			this.expires = expires;
		}

		/// <summary>
		/// 生成上传Token
		/// </summary>
		/// <returns></returns>
		public string Token (Mac mac=null)
		{
			if (string.IsNullOrEmpty (persistentOps) ^ string.IsNullOrEmpty (persistentNotifyUrl)) {
				throw new Exception ("PersistentOps and PersistentNotifyUrl error");
			}
			if (string.IsNullOrEmpty (callBackUrl) ^ string.IsNullOrEmpty (callBackBody)) {
				throw new Exception ("CallBackUrl and CallBackBody error");
			} 
			if (string.IsNullOrEmpty (returnUrl) ^ string.IsNullOrEmpty (returnBody)) {
				throw new Exception ("returnUrl and returnBody error");
			} 
			if (!string.IsNullOrEmpty (returnUrl) && !string.IsNullOrEmpty (callBackUrl)) {
				throw new Exception ("returnUrl and callBackUrl error");
			}
			if (mac == null) {
				mac = new Mac (Config.ACCESS_KEY, Config.Encoding.GetBytes (Config.SECRET_KEY));
			}
			this.deadline = (UInt32)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 + (long)expires);
			string flag = this.ToString (); 
			return mac.SignWithData (Config.Encoding.GetBytes (flag));
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Qiniu.RS.PutPolicy"/> in json formmat.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Qiniu.RS.PutPolicy"/>.</returns>
		public override string ToString ()
		{
			return QiniuJsonHelper.JsonEncode (this);
		}
	}
}
