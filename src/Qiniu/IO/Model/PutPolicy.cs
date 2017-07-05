using Qiniu.JSON;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 上传策略
    /// 另请参阅 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    public class PutPolicy
    {
        /// <summary>
        /// [必需]bucket或者bucket:key
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// [可选]若为 1，表示允许用户上传以 scope 的 keyPrefix 为前缀的文件。
        /// </summary>
        [JsonProperty("isPrefixalScope")]
        public int? isPrefixalScope { get; set; }

        /// <summary>
        /// [必需]上传策略失效时刻，请使用SetExpire来设置它
        /// </summary>
        [JsonProperty("deadline")]
        public int Deadline { get; private set; }

        /// <summary>
        /// [可选]"仅新增"模式
        /// </summary>
        [JsonProperty("insertOnly")]
        public int? InsertOnly { get; set; }

        /// <summary>
        /// [可选]保存文件的key
        /// </summary>
        [JsonProperty("saveKey")]
        public string SaveKey { get; set; }

        /// <summary>
        /// [可选]终端用户
        /// </summary>
        [JsonProperty("endUser")]
        public string EndUser { get; set; }

        /// <summary>
        /// [可选]返回URL
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// [可选]返回内容
        /// </summary>
        [JsonProperty("returnBody")]
        public string ReturnBody { get; set; }

        /// <summary>
        /// [可选]回调URL
        /// </summary>
        [JsonProperty("callBackUrl")]
        public string CallbackUrl { get; set; }

        /// <summary>
        /// [可选]回调内容
        /// </summary>
        [JsonProperty("callbackBody")]
        public string CallbackBody { get; set; }

        /// <summary>
        /// [可选]回调内容类型
        /// </summary>
        [JsonProperty("callbackBodyType")]
        public string CallbackBodyType { get; set; }

        /// <summary>
        /// [可选]回调host
        /// </summary>
        [JsonProperty("callbackHost")]
        public string CallbackHost { get; set; }

        /// <summary>
        /// [可选]回调fetchkey
        /// </summary>
        [JsonProperty("callbackFetchKey")]
        public int? CallbackFetchKey { get; set; }

        /// <summary>
        /// [可选]上传预转持久化
        /// </summary>
        [JsonProperty("persistentOps")]
        public string PersistentOps { get; set; }

        /// <summary>
        /// [可选]持久化结果通知
        /// </summary>
        [JsonProperty("persistentNotifyUrl")]
        public string PersistentNotifyUrl { get; set; }

        /// <summary>
        /// [可选]私有队列
        /// </summary>
        [JsonProperty("persistentPipeline")]
        public string PersistentPipeline { get; set; }

        /// <summary>
        /// [可选]上传文件大小限制：最小值
        /// </summary>
        [JsonProperty("fsizeMin")]
        public int? FsizeMin { get; set; }

        /// <summary>
        /// [可选]上传文件大小限制：最大值
        /// </summary>
        [JsonProperty("fsizeLimit")]
        public int? FsizeLimit { get; set; }

        /// <summary>
        /// [可选]上传时是否自动检测MIME
        /// </summary>
        [JsonProperty("detectMime")]
        public int? DetectMime { get; set; }

        /// <summary>
        /// [可选]上传文件MIME限制
        /// </summary>
        [JsonProperty("mimeLimit")]
        public string MimeLimit { get; set; }

        /// <summary>
        /// [可选]文件上传后多少天后自动删除
        /// </summary>
        [JsonProperty("deleteAfterDays")]
        public int? DeleteAfterDays { get; set; }

        /// <summary>
        /// [可选]文件的存储类型，默认为普通存储，设置为1为低频存储
        /// </summary>
        [JsonProperty("fileType")]
        public int? FileType { get; set; }

        /// <summary>
        /// 设置上传凭证有效期（配置Deadline属性）
        /// </summary>
        /// <param name="expireInSeconds"></param>
        public void SetExpires(int expireInSeconds)
        {
            Deadline = (int)Util.UnixTimestamp.GetUnixTimestamp(expireInSeconds);
        }

        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <returns>JSON字符串</returns>
        public string ToJsonString()
        {
            return JsonHelper.Serialize(this);
        }

    }
}
