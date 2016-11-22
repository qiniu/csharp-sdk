using Newtonsoft.Json;
using System.Collections.Generic;

namespace Qiniu.Storage.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CdnRefreshRequest
    {
        [JsonProperty("urls")]
        public List<string> Urls { set; get; }
        [JsonProperty("dirs")]
        public List<string> Dirs { set; get; }
    }
}
