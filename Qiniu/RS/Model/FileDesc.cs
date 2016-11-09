namespace Qiniu.RS.Model
{
    /// <summary>
    /// 文件描述(stat操作返回消息中包含的有效内容)
    /// </summary>
    public class FileDesc
    {
        public string Key { get; set; }

        public string Hash { get; set; }

        public long Fsize { get; set; }

        public string MimeType { get; set; }

        public long PutTime { get; set; }
    }
}
