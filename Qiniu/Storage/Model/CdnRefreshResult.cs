using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CdnRefreshResult : HttpResult
    {
        [JsonProperty("code")]
        public int Code { set; get; }
        [JsonProperty("error")]
        public string Error { set; get; }
        [JsonProperty("requestId")]
        public string RequestId { set; get; }
        [JsonProperty("invalidUrls")]
        public string[] InvalidUrls { set; get; }
        [JsonProperty("invalidDirs")]
        public string[] InvalidDirs { set; get; }
    }
}
