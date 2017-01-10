using Newtonsoft.Json;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 分片上传的上下文信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ResumeContext
    {
        /// <summary>
        /// 上下文信息
        /// </summary>
        [JsonProperty("ctx")]
        public string Ctx { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        /// <summary>
        /// crc32校验值
        /// </summary>
        [JsonProperty("crc32")]
        public long Crc32 { get; set; }

        /// <summary>
        /// 文件偏移位置
        /// </summary>
        [JsonProperty("offset")]
        public long Offset { get; set; }

        /// <summary>
        /// 上传目的host
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }

        /// <summary>
        /// ctx失效时刻
        /// </summary>
        [JsonProperty("expired_at")]
        public long ExpiredAt { get; set; }
    }
}
