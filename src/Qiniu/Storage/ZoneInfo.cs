using System.Text.Json.Serialization;

namespace Qiniu.Storage
{
    /// <summary>
    /// 从uc.qbox.me返回的消息
    /// </summary>
    internal class ZoneInfo
    {
        [JsonPropertyName("hosts")]
        public ZoneHost[] Hosts { get; set; }
    }

    internal class ZoneHost
    {
        /// <summary>
        /// 过期时间，单位：秒
        /// </summary>
        [JsonPropertyName("ttl")]
        public int Ttl { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("io")]
        public ServiceDomains Io { get; set; }

        [JsonPropertyName("io_src")]
        public ServiceDomains IoSrc { get; set; }

        [JsonPropertyName("up")]
        public ServiceDomains Up { get; set; }
        
        [JsonPropertyName("api")]
        public ServiceDomains Api { get; set; }
        
        [JsonPropertyName("rs")]
        public ServiceDomains Rs { get; set; }
        
        [JsonPropertyName("rsf")]
        public ServiceDomains Rsf { get; set; }
        
        [JsonPropertyName("s3")]
        public ServiceDomains S3 { get; set; }
        
        [JsonPropertyName("uc")]
        public ServiceDomains Uc { get; set; }

        internal class ServiceDomains
        {
            [JsonPropertyName("domains")]
            public string[] Domains { get; set; }

            [JsonPropertyName("old")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string[] Old { get; set; }

            [JsonPropertyName("region_alias")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string RegionAlias { get; set; }
        }
    }
}
