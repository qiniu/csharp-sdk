using System;
namespace Qiniu.Util
{
    /// <summary>
    /// 表示该对象是一个音频转码 (avthumb)操作。
    /// </summary>
    public interface IPersistentOpsAvthumbForAudio : IPersistentOpsAvthumb
    {
        /// <summary>
        /// A 音频码率，单位：比特每秒（bit/s），常用码率：64k，128k，192k，256k，320k等。若指定码率大于原音频码率，则使用原音频码率进行转码。
        /// </summary>
        string BitRate { set; get; }
        /// <summary>
        /// A 音频质量，取值范围为0-9（mp3），10-500（aac），仅支持mp3和aac，值越小越高。不能与上述码率参数共用。
        /// </summary>
        int? AudioQuality { set; get; }
        /// <summary>
        /// A 音频采样频率，单位：赫兹（Hz），常用采样频率：8000，12050，22050，44100等。
        /// </summary>
        int? SamplingRate { set; get; }
        /// <summary>
        /// A 音频编码格式，具体细节请参考支持转换的音频编码格式。
        /// </summary>
        string AudioCodec { set; get; }
        /// <summary>
        /// A 设置音频的profile等级，支持：aac_he。注：需配合 libfdk_aac 编码方案使用，如 avthumb/m4a/acodec/libfdk_aac/audioProfile/aac_he。
        /// </summary>
        string Profile { set; get; }
        /// <summary>
        /// A 转码成mp3时是否写入xing header，默认1写入，写入会导致 file，afinfo 等命令识别出错误的码率。好处是在需要音频时长、帧数的时候只需要获取header。
        /// </summary>
        string Xing { set; get; }
        /// <summary>
        /// A 是否去除音频流，0为保留，1为去除。
        /// 默认值为0。
        /// </summary>
        int? AudioNo { set; get; }
    }
}
