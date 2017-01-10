namespace Qiniu.RS.Model
{
    /// <summary>
    /// 文件描述(stat操作返回消息中包含的有效内容)
    /// 与StatInfo一致
    /// </summary>
    public class FileDesc
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 文件hash(ETAG)
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 文件大小(字节)
        /// </summary>
        public long Fsize { get; set; }

        /// <summary>
        /// 文件MIME类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public long PutTime { get; set; }
    }
}
