using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    /// <summary>
    /// 音视频转码 (avthumb)操作。
    /// </summary>
    public class PersistentOpsAvthumb : PersistentOps, IPersistentOps, IPersistentOpsAvthumb, IPersistentOpsAvthumbForAudio, IPersistentOpsAvthumbForVideo
    {
        /// <summary>
        /// 创建一个音视频转码 (avthumb)操作
        /// </summary>
        /// <param name="format">音视频转码 (avthumb)格式。</param>
        protected internal PersistentOpsAvthumb(string format)
        {
            this.Format = format;
        }
        /// <summary>
        /// A/V 封装格式，具体细节请参考支持转换的封装格式。
        /// </summary>
        public string Format { private set; get; }
        /// <summary>
        /// A 音频码率，单位：比特每秒（bit/s），常用码率：64k，128k，192k，256k，320k等。若指定码率大于原音频码率，则使用原音频码率进行转码。
        /// </summary>
        public string BitRate { set; get; }
        /// <summary>
        /// A 音频质量，取值范围为0-9（mp3），10-500（aac），仅支持mp3和aac，值越小越高。不能与上述码率参数共用。
        /// </summary>
        public int? AudioQuality { set; get; }
        /// <summary>
        /// A 音频采样频率，单位：赫兹（Hz），常用采样频率：8000，12050，22050，44100等。
        /// </summary>
        public int? SamplingRate { set; get; }
        /// <summary>
        /// V 视频帧率，每秒显示的帧数，单位：赫兹（Hz），常用帧率：24，25，30等，一般用默认值。
        /// </summary>
        public int? FrameRate { set; get; }
        /// <summary>
        /// V 视频码率，单位：比特每秒（bit/s），常用视频码率：128k，1.25m，5m等。若指定码率大于原视频码率，则使用原视频码率进行转码。
        /// </summary>
        public string VideoBitRate { set; get; }
        /// <summary>
        /// V 视频编码格式，具体细节请参考支持转换的视频编码格式。
        /// </summary>
        public string VideoCodec { set; get; }
        /// <summary>
        /// A 音频编码格式，具体细节请参考支持转换的音频编码格式。
        /// </summary>
        public string AudioCodec { set; get; }
        /// <summary>
        /// A 设置音频的profile等级，支持：aac_he。注：需配合 libfdk_aac 编码方案使用，如 avthumb/m4a/acodec/libfdk_aac/audioProfile/aac_he。
        /// </summary>
        public string Profile { set; get; }
        /// <summary>
        /// V 字幕编码方案，支持方案：mov_text。该参数仅用于修改带字幕视频的字幕编码。
        /// </summary>
        public string SubtitleCodec { set; get; }
        /// <summary>
        /// V 添加字幕，支持：srt格式字幕（uft-8编码和和utf-8 BOM编码）、带有字幕的mkv文件、embed（将原视频的字幕流嵌入目标视频）。基于URL安全的base64编码。
        /// </summary>
        public string SubtitleURL { set; get; }
        /// <summary>
        /// A/V 指定视频截取的开始时间，单位：秒，支持精确到毫秒，例如3.345s。用于视频截取，从一段视频中截取一段视频。
        /// </summary>
        public double? SeekStart { set; get; }
        /// <summary>
        /// A/V 指定视频截取的长度，单位：秒，支持精确到毫秒，例如1.500s。用于视频截取，从一段视频中截取一段视频。
        /// </summary>
        public double? Duration { set; get; }
        /// <summary>
        /// V 指定视频分辨率，格式为<width>x<height>或者预定义值，width 取值范围 [20,3840]，height 取值范围 [20,2160]。
        /// </summary>
        public string Resolution { set; get; }
        /// <summary>
        /// V 配合参数/s/使用，指定为1时，把视频按原始比例缩放到/s/指定的矩形框内，0或者不指定会强制缩放到对应分辨率，可能造成视频变形。
        /// </summary>
        public int? Autoscale { set; get; }
        /// <summary>
        /// V 该参数为视频在播放器中显示的宽高比，格式为<width>:<height>。例如：取值3:4表示视频在播放器中播放是宽:高=3:4（注：此处取值仅为体现演示效果）。
        /// </summary>
        public string Aspect { set; get; }
        /// <summary>
        /// A/V 是否清除文件的metadata，1为清除，0为保留。
        /// </summary>
        public int? StripMeta { set; get; }
        /// <summary>
        /// V 设置h264的crf值，整数，取值范围[18,28]，值越小，画质更清晰。注意：不可与vb共用
        /// </summary>
        public int? H264Crf { set; get; }
        /// <summary>
        /// V 指定顺时针旋转的度数，可取值为90、180、270、auto，默认为不旋转。
        /// </summary>
        public int? Degree { set; get; }
        /// <summary>
        /// V 水印图片的源路径，目前仅支持远程路径，需要经过urlsafe_base64_encode。水印具体介绍见视频水印。
        /// </summary>
        public string EncodedRemoteImageUrl { set; get; }
        /// <summary>
        /// V 视频图片水印位置，存在/wmImage/时生效。
        /// </summary>
        public string Gravity { set; get; }
        /// <summary>
        /// V 水印文本内容,需要经过urlsafe_base64_encode。
        /// </summary>
        public string EncodedText { set; get; }
        /// <summary>
        /// V 文本位置（默认NorthEast）
        /// </summary>
        public string GravityText { set; get; }
        /// <summary>
        ///  V 文本字体，需要经过urlsafe_base64_encode，默认为黑体,注意：中文水印必须指定中文字体。
        /// </summary>
        public string Font { set; get; }
        /// <summary>
        /// V 水印文字颜色，需要经过urlsafe_base64_encode，RGB格式，可以是颜色名称（例如红色）或十六进制（例如#FF0000），参考RGB颜色编码表，默认为黑色。
        /// </summary>
        public string FontColor { set; get; }
        /// <summary>
        ///  V 水印文字大小，单位: 缇，等于1/20磅，默认值0（默认大小）
        /// </summary>
        public int? FontSize { set; get; }
        /// <summary>
        /// A 转码成mp3时是否写入xing header，默认1写入，写入会导致 file，afinfo 等命令识别出错误的码率。好处是在需要音频时长、帧数的时候只需要获取header。
        /// </summary>
        public string Xing { set; get; }
        /// <summary>
        /// A 是否去除音频流，0为保留，1为去除。
        /// 默认值为0。
        /// </summary>
        public int? AudioNo { set; get; }
        /// <summary>
        /// V 是否去除视频流，0为保留，1为去除。
        /// 默认值为0。
        /// </summary>
        public int? VideoNo { set; get; }
        /// <summary>
        /// 在数据处理命令后用管道符 | 拼接 saveas/<encodedEntryURI> 指令，指示七牛服务器使用EncodedEntryURI格式中指定的 Bucket 与 Key 来保存处理结果。
        /// </summary>
        public string SaveAs { set; get; }
        /// <summary>
        /// 获取操作字符串。
        /// </summary>
        /// <returns>操作字符串。</returns>
        public override string GetOpsString()
        {
            var sbOps = new StringBuilder("avthumb/");
            sbOps.Append(this.Format);
            if (!string.IsNullOrWhiteSpace(this.BitRate))
            {
                sbOps.Append("/ab/");
                sbOps.Append(this.BitRate);
            }
            if (this.AudioQuality.HasValue)
            {
                sbOps.Append("/aq/");
                sbOps.Append(this.AudioQuality);
            }
            if (this.SamplingRate.HasValue)
            {
                sbOps.Append("/ar/");
                sbOps.Append(this.SamplingRate);
            }
            if (this.FrameRate.HasValue)
            {
                sbOps.Append("/r/");
                sbOps.Append(this.FrameRate);
            }
            if (!string.IsNullOrWhiteSpace(this.VideoBitRate))
            {
                sbOps.Append("/vb/");
                sbOps.Append(this.VideoBitRate);
            }
            if (!string.IsNullOrWhiteSpace(this.VideoCodec))
            {
                sbOps.Append("/vcodec/");
                sbOps.Append(this.VideoCodec);
            }
            if (!string.IsNullOrWhiteSpace(this.AudioCodec))
            {
                sbOps.Append("/acodec/");
                sbOps.Append(this.AudioCodec);
            }
            if (!string.IsNullOrWhiteSpace(this.SubtitleCodec))
            {
                sbOps.Append("/scodec/");
                sbOps.Append(this.SubtitleCodec);
            }
            if (!string.IsNullOrWhiteSpace(this.SubtitleURL))
            {
                sbOps.Append("/subtitle/");
                sbOps.Append(this.SubtitleURL);
            }
            if (this.SeekStart.HasValue)
            {
                sbOps.Append("/ss/");
                sbOps.Append(this.SeekStart);
            }
            if (this.Duration.HasValue)
            {
                sbOps.Append("/d/");
                sbOps.Append(this.Duration);
            }
            if (!string.IsNullOrWhiteSpace(this.Resolution))
            {
                sbOps.Append("/s/");
                sbOps.Append(this.Resolution);
            }
            if (this.Autoscale.HasValue)
            {
                sbOps.Append("/autoscale/");
                sbOps.Append(this.Autoscale);
            }
            if (!string.IsNullOrWhiteSpace(this.Aspect))
            {
                sbOps.Append("/aspect/");
                sbOps.Append(this.Aspect);
            }
            if (this.StripMeta.HasValue)
            {
                sbOps.Append("/stripmeta/");
                sbOps.Append(this.StripMeta);
            }
            if (this.H264Crf.HasValue)
            {
                sbOps.Append("/h264Crf/");
                sbOps.Append(this.H264Crf);
            }
            if (this.Degree.HasValue)
            {
                sbOps.Append("/rotate/");
                sbOps.Append(this.Degree);
            }
            if (!string.IsNullOrWhiteSpace(this.EncodedRemoteImageUrl))
            {
                sbOps.Append("/wmImage/");
                sbOps.Append(this.EncodedRemoteImageUrl);
            }
            if (!string.IsNullOrWhiteSpace(this.Gravity))
            {
                sbOps.Append("/wmGravity/");
                sbOps.Append(this.Gravity);
            }
            if (!string.IsNullOrWhiteSpace(this.EncodedText))
            {
                sbOps.Append("/wmText/");
                sbOps.Append(this.EncodedText);
            }
            if (!string.IsNullOrWhiteSpace(this.GravityText))
            {
                sbOps.Append("/wmGravityText/");
                sbOps.Append(this.GravityText);
            }
            if (!string.IsNullOrWhiteSpace(this.Font))
            {
                sbOps.Append("/wmFont/");
                sbOps.Append(this.Font);
            }
            if (!string.IsNullOrWhiteSpace(this.FontColor))
            {
                sbOps.Append("/wmFontColor/");
                sbOps.Append(this.FontColor);
            }
            if (this.FontSize.HasValue)
            {
                sbOps.Append("/wmFontSize/");
                sbOps.Append(this.FontSize);
            }
            if (!string.IsNullOrWhiteSpace(this.Xing))
            {
                sbOps.Append("/writeXing/");
                sbOps.Append(this.Xing);
            }
            if (this.AudioNo.HasValue)
            {
                sbOps.Append("/an/");
                sbOps.Append(this.AudioNo);
            }
            if (this.VideoNo.HasValue)
            {
                sbOps.Append("/vn/");
                sbOps.Append(this.VideoNo);
            }
            if (!string.IsNullOrWhiteSpace(this.SaveAs))
            {
                sbOps.Append("|saveas/");
                sbOps.Append(this.SaveAs);
            }
            return sbOps.ToString();
        }
    }
}
