namespace Qiniu.Common
{
    /// <summary>
    /// 从uc.qbox.me返回的消息
    /// </summary>
    internal class ZoneInfo
    {
        public string TTL { get; set; }

        public OBulk HTTP { get; set; }

        public OBulk HTTPS { get; set; }
    }

    /// <summary>
    /// 包含uploadHost和iovip等
    /// </summary>
    internal class OBulk
    {
        public string[] UP { get; set; }
        public string[] IO { get; set; }
    }
}
