using Newtonsoft.Json;

namespace Qiniu.Storage
{
    /// <summary>
    /// 从uc.qbox.me返回的消息
    /// </summary>
    internal class ZoneInfo
    {
        /// <summary>
        /// 过期时间，单位：秒
        /// </summary>
        [JsonProperty("ttl")]
        public int Ttl { get; set; }
        
        [JsonProperty("io")]
        public Io Io { set; get; }
        
        [JsonProperty("up")]
        public Up Up { set; get; }
        
        [JsonProperty("api")]
        public Api Api { set; get; }
        
        [JsonProperty("rs")]
        public Rs Rs { set; get; }
        
        [JsonProperty("rsf")]
        public Rsf Rsf { set; get; }
    }

    internal class Io
    {
        [JsonProperty("src")]
        public Src Src { set; get; }
    }

    internal class Src
    {
        [JsonProperty("main")]
        public string[] Main { set; get; }
    }

    internal class Up
    {
        [JsonProperty("acc")]
        public DomainInfo Acc { set; get; }
        
        [JsonProperty("old_acc")]
        public DomainInfo OldAcc { set; get; }
        
        [JsonProperty("src")]
        public DomainInfo Src { set; get; }
        
        [JsonProperty("old_src")]
        public DomainInfo OldSrc { set; get; }
    }
    internal class DomainInfo
    {
        [JsonProperty("main")]
        public string[] Main { set; get; }
        
        [JsonProperty("backup", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Backup { set; get; }
        
        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
        public string Info { set; get; }
    }

    internal class Api
    {
        [JsonProperty("acc")]
        public DomainInfo Acc { set; get; }
    }
    
    internal class Rs
    {
        [JsonProperty("acc")]
        public DomainInfo Acc { set; get; }
    }
    
    internal class Rsf
    {
        [JsonProperty("acc")]
        public DomainInfo Acc { set; get; }
    }
}
