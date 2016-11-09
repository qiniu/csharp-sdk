using Newtonsoft.Json;

namespace Qiniu.RS.Model
{
    /// <summary>
    /// 获取空间文件信息(stat操作)的有效内容
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class StatInfo
    {
        [JsonProperty("fsize")]
        public long Fsize { set; get; }

        [JsonProperty("hash")]
        public string Hash { set; get; }

        [JsonProperty("mimeType")]
        public string MimeType { set; get; }

        [JsonProperty("putTime")]
        public long PutTime { set; get; }
    }
}
