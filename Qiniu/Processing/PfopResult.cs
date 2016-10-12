using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Processing
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PfopResult : HttpResult
    {
        [JsonProperty("persistentId")]
        public string PersistentId { set; get; }
        public PfopResult() { }
    }
}
