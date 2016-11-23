using System;
namespace Qiniu.Util
{
    /// <summary>
    /// 表示该对象是一个音视频转码 (avthumb)操作。
    /// </summary>
    public interface IPersistentOpsAvthumb : IPersistentOps
    {
        /// <summary>
        /// A/V 封装格式，具体细节请参考支持转换的封装格式。
        /// </summary>
        string Format { get; }
        /// <summary>
        /// A/V 指定视频截取的开始时间，单位：秒，支持精确到毫秒，例如3.345s。用于视频截取，从一段视频中截取一段视频。
        /// </summary>
        double? SeekStart { set; get; }
        /// <summary>
        /// A/V 指定视频截取的长度，单位：秒，支持精确到毫秒，例如1.500s。用于视频截取，从一段视频中截取一段视频。
        /// </summary>
        double? Duration { set; get; }
        /// <summary>
        /// A/V 是否清除文件的metadata，1为清除，0为保留。
        /// </summary>
        int? StripMeta { set; get; }
        /// <summary>
        /// 在数据处理命令后用管道符 | 拼接 saveas/<encodedEntryURI> 指令，指示七牛服务器使用EncodedEntryURI格式中指定的 Bucket 与 Key 来保存处理结果。
        /// </summary>
        string SaveAs { set; get; }
    }
}
