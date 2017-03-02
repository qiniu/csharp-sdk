namespace Qiniu.IO.Model
{
    /// <summary>
    /// 分片上传的上下文信息
    /// </summary>
    public class ResumeContext
    {
        /// <summary>
        /// 上下文信息
        /// </summary>
        public string Ctx { get; set; }

        /// <summary>
        /// 校验和
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// crc32校验值
        /// </summary>
        public long Crc32 { get; set; }

        /// <summary>
        /// 文件偏移位置
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// 上传目的host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// ctx失效时刻
        /// </summary>
        public long Expired_At { get; set; }
    }
}
