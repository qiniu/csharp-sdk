using System;
using Newtonsoft.Json;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 上传策略
    /// 另请参阅 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PutPolicy
    {
        /// <summary>
        /// bucket或者bucket:key
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { set; get; }

        /// <summary>
        /// 上传策略失效时刻
        /// </summary>
        [JsonProperty("deadline")]
        public int Deadline { set; get; }

        /// <summary>
        /// "仅新增"模式
        /// </summary>
        [JsonProperty("insertOnly")]
        public int? InsertOnly { set; get; }

        /// <summary>
        /// 保存文件的key
        /// </summary>
        [JsonProperty("saveKey")]
        public string SaveKey { set; get; }

        /// <summary>
        /// 终端用户
        /// </summary>
        [JsonProperty("endUser")]
        public string EndUser { set; get; }

        /// <summary>
        /// 返回URL
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl { set; get; }

        /// <summary>
        /// 返回内容
        /// </summary>
        [JsonProperty("returnBody")]
        public string ReturnBody { set; get; }

        /// <summary>
        /// 回调URL
        /// </summary>
        [JsonProperty("callbackUrl")]
        public string CallbackUrl { set; get; }

        /// <summary>
        /// 回调内容
        /// </summary>
        [JsonProperty("callbackBody")]
        public string CallbackBody { set; get; }

        /// <summary>
        /// 回调内容类型
        /// </summary>
        [JsonProperty("callbackBodyType")]
        public string CallbackBodyType { set; get; }

        /// <summary>
        /// 回调host
        /// </summary>
        [JsonProperty("callbackHost")]
        public string CallbackHost { set; get; }

        /// <summary>
        /// 回调fetchkey
        /// </summary>
        [JsonProperty("callbackFetchKey")]
        public int? CallbackFetchKey { set; get; }

        /// <summary>
        /// 上传预转持久化
        /// </summary>
        [JsonProperty("persistentOps")]
        public string PersistentOps { set; get; }

        /// <summary>
        /// 持久化结果通知
        /// </summary>
        [JsonProperty("persistentNotifyUrl")]
        public string PersistentNotifyUrl { set; get; }

        /// <summary>
        /// 私有队列
        /// </summary>
        [JsonProperty("persistentPipeline")]
        public string PersistentPipeline { set; get; }

        /// <summary>
        /// 上传文件大小限制
        /// </summary>
        [JsonProperty("fsizeLimit")]
        public int? FsizeLimit { set; get; }

        /// <summary>
        /// 上传时是否自动检测MIME
        /// </summary>
        [JsonProperty("detectMime")]
        public int? DetectMime { set; get; }

        /// <summary>
        /// 上传文件MIME限制
        /// </summary>
        [JsonProperty("mimeLimit")]
        public string MimeLimit { set; get; }

        /// <summary>
        /// 文件上传后多少天后自动删除
        /// </summary>
        [JsonProperty("deleteAfterDays")]
        public int? DeleteAfterDays { set; get; }

        /// <summary>
        /// 设置上传凭证有效期
        /// </summary>
        /// <param name="expireInSeconds"></param>
        public void SetExpires(int expireInSeconds)
        {
            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            this.Deadline = (int)ts.TotalSeconds + expireInSeconds;
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <returns>JSON字符串</returns>
        public string ToJsonString()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, setting);
        }
    }
}
