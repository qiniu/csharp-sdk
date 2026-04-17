using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Qiniu.Storage
{
    /// <summary>
    /// 分片上传的上下文信息
    /// </summary>
    public class ResumeContext
    {
        /// <summary>
        /// 上下文信息
        /// </summary>
        [JsonPropertyName("ctx")]
        public string Ctx { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        [JsonPropertyName("checksum")]
        public string Checksum { get; set; }

        /// <summary>
        /// crc32校验值
        /// </summary>
        [JsonPropertyName("crc32")]
        public uint Crc32 { get; set; }

        /// <summary>
        /// 文件偏移位置
        /// </summary>
        [JsonPropertyName("offset")]
        public long Offset { get; set; }

        /// <summary>
        /// 上传目的host
        /// </summary>
        [JsonPropertyName("host")]
        public string Host { get; set; }

        /// <summary>
        /// ctx失效时刻
        /// </summary>
        [JsonPropertyName("expired_at")]
        public long ExpiredAt { get; set; }

        /// <summary>
        /// 新版分片上传上下文etag
        /// </summary>
        [JsonPropertyName("etag")]
        public Dictionary<string, object> Etag { get; set; }

        /// <summary>
        /// 新版分片上传md5校验值
        /// </summary>
        [JsonPropertyName("md5")]
        public string Md5 { get; set; }
    }
}
