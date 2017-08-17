using Newtonsoft.Json;

namespace Qiniu.Storage
{
    public class PfopInfo 
    {
        [JsonProperty("id")]
        public string Id;
        [JsonProperty("code")]
        public int Code;
        [JsonProperty("desc")]
        public string Desc;
        [JsonProperty("inputKey")]
        public string InputKey;
        [JsonProperty("inputBucket")]
        public string InputBucket;
        [JsonProperty("pipeline")]
        public string Pipeline;
        [JsonProperty("reqid")]
        public string Reqid;
        [JsonProperty("items")]
        public PfopItems[] Items;
    }


    public class PfopItems
    {
        [JsonProperty("cmd")]
        public string Cmd;
        [JsonProperty("code")]
        public string Code;
        [JsonProperty("desc")]
        public string Desc;
        [JsonProperty("Error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error;
        [JsonProperty("keys", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Keys;
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key;
        [JsonProperty("hash", NullValueHandling = NullValueHandling.Ignore)]
        public string Hash;
        [JsonProperty("returnOld", NullValueHandling = NullValueHandling.Ignore)]
        public int? ReturnOld;
    }
}
