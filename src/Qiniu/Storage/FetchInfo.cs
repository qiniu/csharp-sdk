using Newtonsoft.Json;
namespace Qiniu.Storage
{
    public class FetchInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [JsonProperty("key")]
        public string Key { set; get; }

        /// <summary>
        /// 文件大小(字节)
        /// </summary>
        [JsonProperty("fsize")]
        public long Fsize { set; get; }

        /// <summary>
        /// 文件hash(ETAG)
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { set; get; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        [JsonProperty("mimeType")]
        public string MimeType { set; get; }
    }
}
