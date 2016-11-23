using System;
namespace Qiniu.Util
{
    /// <summary>
    /// 表示该对象是一个视频转码 (avthumb)操作。
    /// </summary>
    public interface IPersistentOpsAvthumbForVideo : IPersistentOpsAvthumb
    {
        /// <summary>
        /// V 视频帧率，每秒显示的帧数，单位：赫兹（Hz），常用帧率：24，25，30等，一般用默认值。
        /// </summary>
        int? FrameRate { set; get; }
        /// <summary>
        /// V 视频码率，单位：比特每秒（bit/s），常用视频码率：128k，1.25m，5m等。若指定码率大于原视频码率，则使用原视频码率进行转码。
        /// </summary>
        string VideoBitRate { set; get; }
        /// <summary>
        /// V 视频编码格式，具体细节请参考支持转换的视频编码格式。
        /// </summary>
        string VideoCodec { set; get; }
        /// <summary>
        /// V 字幕编码方案，支持方案：mov_text。该参数仅用于修改带字幕视频的字幕编码。
        /// </summary>
        string SubtitleCodec { set; get; }
        /// <summary>
        /// V 添加字幕，支持：srt格式字幕（uft-8编码和和utf-8 BOM编码）、带有字幕的mkv文件、embed（将原视频的字幕流嵌入目标视频）。基于URL安全的base64编码。
        /// </summary>
        string SubtitleURL { set; get; }
        /// <summary>
        /// V 指定视频分辨率，格式为<width>x<height>或者预定义值，width 取值范围 [20,3840]，height 取值范围 [20,2160]。
        /// </summary>
        string Resolution { set; get; }
        /// <summary>
        /// V 配合参数/s/使用，指定为1时，把视频按原始比例缩放到/s/指定的矩形框内，0或者不指定会强制缩放到对应分辨率，可能造成视频变形。
        /// </summary>
        int? Autoscale { set; get; }
        /// <summary>
        /// V 该参数为视频在播放器中显示的宽高比，格式为<width>:<height>。例如：取值3:4表示视频在播放器中播放是宽:高=3:4（注：此处取值仅为体现演示效果）。
        /// </summary>
        string Aspect { set; get; }
        /// <summary>
        /// V 设置h264的crf值，整数，取值范围[18,28]，值越小，画质更清晰。注意：不可与vb共用
        /// </summary>
        int? H264Crf { set; get; }
        /// <summary>
        /// V 指定顺时针旋转的度数，可取值为90、180、270、auto，默认为不旋转。
        /// </summary>
        int? Degree { set; get; }
        /// <summary>
        /// V 水印图片的源路径，目前仅支持远程路径，需要经过urlsafe_base64_encode。水印具体介绍见视频水印。
        /// </summary>
        string EncodedRemoteImageUrl { set; get; }
        /// <summary>
        /// V 视频图片水印位置，存在/wmImage/时生效。
        /// </summary>
        string Gravity { set; get; }
        /// <summary>
        /// V 水印文本内容,需要经过urlsafe_base64_encode。
        /// </summary>
        string EncodedText { set; get; }
        /// <summary>
        /// V 文本位置（默认NorthEast）
        /// </summary>
        string GravityText { set; get; }
        /// <summary>
        ///  V 文本字体，需要经过urlsafe_base64_encode，默认为黑体,注意：中文水印必须指定中文字体。
        /// </summary>
        string Font { set; get; }
        /// <summary>
        /// V 水印文字颜色，需要经过urlsafe_base64_encode，RGB格式，可以是颜色名称（例如红色）或十六进制（例如#FF0000），参考RGB颜色编码表，默认为黑色。
        /// </summary>
        string FontColor { set; get; }
        /// <summary>
        ///  V 水印文字大小，单位: 缇，等于1/20磅，默认值0（默认大小）
        /// </summary>
        int? FontSize { set; get; }
        /// <summary>
        /// V 是否去除视频流，0为保留，1为去除。
        /// 默认值为0。
        /// </summary>
        int? VideoNo { set; get; }
    }
}
