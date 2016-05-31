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
		private string saveKey;
		private int insertOnly;
		private int detectMime;
        private string mimeLimit;
		private long fsizeLimit;
		private long fsizeMin;
		private string persistentOps;
		private string persistentNotifyUrl;
		private string persistentPipeline;
		private string endUser;
		private UInt64 expires = 3600;
		private UInt64 deadline = 0;
		private string callbackHost;
		private string callbackBodyType;
		private int callbackFetchKey;
    	private int deleteAfterDays;

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
        /// 限定用户上传的文件类型
        /// 指定本字段值，七牛服务器会侦测文件内容以判断MimeType，再用判断值跟指定值进行匹配，匹配成功则允许上传，匹配失败返回400状态码
        /// 示例:
        ///1. “image/*“表示只允许上传图片类型
        ///2. “image/jpeg;image/png”表示只允许上传jpg和png类型的图片
        /// </summary>
        /// <value>The detect MIME.</value>
        [JsonProperty("mimeLimit")]
        public string MimeLimit
        {
            get
            {
                return mimeLimit;
            }
            set
            {
                mimeLimit = value;
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
		/// 可选, Gets or sets the fsize limit.
		/// </summary>
		/// <value>限定上传文件大小最小值.</value>
		[JsonProperty("fsizeMin")]
		public long FsizeMin {
			get {
				return fsizeMin;
			}
			set{
				fsizeMin = value;
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
		/// 可指定音视频文件上传完成后，需要进行的转码持久化操作。persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
		/// </summary>
		[JsonProperty("persistentOps")]
		public string PersistentOps {
			get { return persistentOps;  }
			set { persistentOps = value; }
		}

		// <summary>
		/// 可指定音视频文件上传后处理的队列，不指定时在公共队列中。persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
		/// </summary>
		[JsonProperty("persistentPipeline")]
		public string PersistentPipeline {
			get { return persistentPipeline;  }
			set { persistentPipeline = value; }
		}

		// <summary>
		///上传成功后，七牛云向App-Server发送回调通知时的 Host 值，仅当同时设置了 callbackUrl 时有效。
		/// </summary>
		[JsonProperty("callbackHost")]
		public string CallbackHost
		{
			get { return callbackHost; }
			set { callbackHost = value; }
		}

		// <summary>
		///上传成功后，七牛云向App-Server发送回调通知callbackBody的Content-Type，默认为application/x-www-form-urlencoded，也可设置为application/json。
		/// </summary>
		[JsonProperty("callbackBodyType")]
		public string CallbackBodyType
		{
			get { return callbackBodyType; }
			set { callbackBodyType = value; }
		}

		// <summary>
		///是否启用fetchKey上传模式，0为关闭，1为启用；具体见fetchKey上传模式。
		/// </summary>
		[JsonProperty("callbackFetchKey")]
		public int CallbackFetchKey
		{
			get { return callbackFetchKey; }
			set { callbackFetchKey = value; }
		}


		/// <summary>
        /// 文件在多少天后被删除，七牛将文件上传时间与指定的deleteAfterDays天数相加，得到的时间入到后一天的午夜(CST,中国标准时间)，从而得到文件删除开始时间。例如文件在2015年1月1日上午10:00 CST上传，指定deleteAfterDays为3天，那么会在2015年1月5日00:00 CST之后当天内删除文件
        /// </summary>
        public int DeleteAfterDays
        {
            get { return deleteAfterDays; }
            set { deleteAfterDays = value; }
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
