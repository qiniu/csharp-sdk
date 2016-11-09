using System;
using Newtonsoft.Json;
using Qiniu.Util;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 上传策略
    /// 参阅http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PutPolicy
    {
        [JsonProperty("scope")]
        public string Scope { set; get; }

        [JsonProperty("deadline")]
        public int Deadline { set; get; }

        [JsonProperty("insertOnly")]
        public int? InsertOnly { set; get; }

        [JsonProperty("saveKey")]
        public string SaveKey { set; get; }

        [JsonProperty("endUser")]
        public string EndUser { set; get; }

        [JsonProperty("returnUrl")]
        public string ReturnUrl { set; get; }

        [JsonProperty("returnBody")]
        public string ReturnBody { set; get; }

        [JsonProperty("callbackUrl")]
        public string CallbackUrl { set; get; }

        [JsonProperty("callbackBody")]
        public string CallbackBody { set; get; }

        [JsonProperty("callbackBodyType")]
        public string CallbackBodyType { set; get; }

        [JsonProperty("callbackHost")]
        public string CallbackHost { set; get; }

        [JsonProperty("callbackFetchKey")]
        public int? CallbackFetchKey { set; get; }

        [JsonProperty("persistentOps")]
        public string PersistentOps { set; get; }

        [JsonProperty("persistentNotifyUrl")]
        public string PersistentNotifyUrl { set; get; }

        [JsonProperty("persistentPipeline")]
        public string PersistentPipeline { set; get; }

        [JsonProperty("fsizeLimit")]
        public int? FsizeLimit { set; get; }

        [JsonProperty("detectMime")]
        public int? DetectMime { set; get; }

        [JsonProperty("mimeLimit")]
        public string MimeLimit { set; get; }

        [JsonProperty("deleteAfterDays")]
        public int? DeleteAfterDays { set; get; }

        public void SetExpires(int expireInSeconds)
        {
            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            this.Deadline = (int)ts.TotalSeconds + expireInSeconds;
        }

        public string ToJsonString()
        {
            return StringHelper.JsonEncode(this);
        }
    }
}
