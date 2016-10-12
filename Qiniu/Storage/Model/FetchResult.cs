using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FetchResult : HttpResult
    {
        [JsonProperty("hash")]
        public string Hash { set; get; }
        [JsonProperty("key")]
        public string Key { set; get; }
    }
}
