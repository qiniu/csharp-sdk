using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StatResult : HttpResult
    {
        [JsonProperty("fsize")]
        public long Fsize { set; get; }
        [JsonProperty("hash")]
        public string Hash { set; get; }
        [JsonProperty("mimeType")]
        public string MimeType { set; get; }
        [JsonProperty("putTime")]
        public long PutTime { set; get; }
        public StatResult() { }
    }
}