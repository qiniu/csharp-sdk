using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 分片上传的上下文信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ResumeContext
    {
        [JsonProperty("ctx")]
        public string Ctx { get; set; }

        [JsonProperty("checksum")]
        public string Checksum { get; set; }

        [JsonProperty("crc32")]
        public long Crc32 { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("expired_at")]
        public long ExpiredAt { get; set; }
    }
}
