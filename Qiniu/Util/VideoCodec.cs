using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    public abstract class VideoCodec
    {
        private VideoCodec() { }
        public const string a64multi = "a64multi"; // Multicolor charset for Commodore 64 (codec a64_multi)
        public const string a64multi5 = "a64multi5"; // Multicolor charset for Commodore 64, extended with 5th color (colram) (codec a64_multi5)
        public const string amv = "amv"; // AMV Video
        public const string asv1 = "asv1"; // ASUS V1
        public const string asv2 = "asv2"; // ASUS V2
        public const string avrp = "avrp"; // Avid 11 10-bit RGB Packer
        public const string avui = "avui"; // Avid Meridien Uncompressed
        public const string ayuv = "ayuv"; // Uncompressed packed MS 4444
        public const string bmp = "bmp"; // BMP (Windows and OS/2 bitmap)
        public const string cljr = "cljr"; // Cirrus Logic AccuPak
        public const string dnxhd = "dnxhd"; // VC3/DNxHD
        public const string dpx = "dpx"; // DPX image
        public const string dvvideo = "dvvideo"; // DV (Digital Video)
        public const string ffv1 = "ffv1"; // FFmpeg video codec # 1
        public const string ffvhuff = "ffvhuff"; // Huffyuv FFmpeg variant
        public const string flashsv = "flashsv"; // Flash Screen Video
        public const string flashsv2 = "flashsv2"; // Flash Screen Video Version 2
        public const string flv = "flv"; // FLV / Sorenson Spark / Sorenson H.263 (Flash Video) (codec flv1)
        public const string gif = "gif"; // GIF (Graphics Interchange Format)
        public const string h261 = "h261"; // H.261
        public const string h263 = "h263"; // H.263 / H.263-1996
        public const string h263p = "h263p"; // H.263+ / H.263-1998 / H.263 version 2
        public const string libx264 = "libx264"; // libx264 H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h264)
        public const string libx265 = "libx265"; // libx265 H.265 / AVC / MPEG-4 AVC / MPEG-4 part 10 RGB (codec h265)
        public const string huffyuv = "huffyuv"; // Huffyuv / HuffYUV
        public const string jpeg2000 = "jpeg2000"; // JPEG 2000,
        public const string jpegls = "jpegls"; // JPEG-LS
        public const string ljpeg = "ljpeg"; // Lossless JPEG
        public const string mjpeg = "mjpeg"; // MJPEG (Motion JPEG)
        public const string mpeg1video = "mpeg1video"; // MPEG-1 video
        public const string mpeg2video = "mpeg2video"; // MPEG-2 video
        public const string mpeg4 = "mpeg4"; // MPEG-4 part 2
        public const string libxvid = "libxvid"; // libxvidcore MPEG-4 part 2 (codec mpeg4)
        public const string msmpeg4v2 = "msmpeg4v2"; // MPEG-4 part 2 Microsoft variant version 2
        public const string msmpeg4 = "msmpeg4"; // MPEG-4 part 2 Microsoft variant version 3 (codec msmpeg4v3)
        public const string msvideo1 = "msvideo1"; // Microsoft Video-1
        public const string pam = "pam"; // PAM (Portable AnyMap) image
        public const string pbm = "pbm"; // PBM (Portable BitMap) image
        public const string pcx = "pcx"; // PC Paintbrush PCX image
        public const string pgm = "pgm"; // PGM (Portable GrayMap) image
        public const string pgmyuv = "pgmyuv"; // PGMYUV (Portable GrayMap YUV) image
        public const string png = "png"; // PNG (Portable Network Graphics) image
        public const string ppm = "ppm"; // PPM (Portable PixelMap) image
        public const string prores = "prores"; // Apple ProRes
        public const string prores_aw = "prores_aw"; // Apple ProRes (codec prores)
        public const string prores_ks = "prores_ks"; // Apple ProRes (iCodec Pro) (codec prores)
        public const string qtrle = "qtrle"; // QuickTime Animation (RLE) video
        public const string r10k = "r10k"; // AJA Kona 10-bit RGB Codec
        public const string r210 = "r210"; // Uncompressed RGB 10-bit
        public const string rawvideo = "rawvideo"; // raw video
        public const string roqvideo = "roqvideo"; // id RoQ video (codec roq)
        public const string rv10 = "rv10"; // RealVideo 1.0
        public const string rv20 = "rv20"; // RealVideo 2.0
        public const string sgi = "sgi"; // SGI image
        public const string snow = "snow"; // Snow
        public const string sunrast = "sunrast"; // Sun Rasterfile image
        public const string svq1 = "svq1"; // Sorenson Vector Quantizer 1 / Sorenson Video 1 / SVQ1
        public const string targa = "targa"; // Truevision Targa image
        public const string libtheora = "libtheora"; // libtheora Theora (codec theora)
        public const string tiff = "tiff"; // TIFF image
        public const string utvideo = "utvideo"; // Ut Video
        public const string v210 = "v210"; // Uncompressed 422 10-bit
        public const string v308 = "v308"; // Uncompressed packed 444
        public const string v408 = "v408"; // Uncompressed packed QT 4444
        public const string v410 = "v410"; // Uncompressed 444 10-bit
        public const string libvpx = "libvpx"; // libvpx VP8 (codec vp8)
        public const string libvpx_vp9 = "libvpx-vp9"; // libvpx VP9
        public const string wmv1 = "wmv1"; // Windows Media Video 7
        public const string wmv2 = "wmv2"; // Windows Media Video 8
        public const string xbm = "xbm"; // XBM (X BitMap) image
        public const string xface = "xface"; // X-face image
        public const string xwd = "xwd"; // XWD (X Window Dump) image
        public const string y41p = "y41p"; // Uncompressed YUV 411 12-bit
        public const string yuv4 = "yuv4"; // Uncompressed packed 420
        public const string zlib = "zlib"; // LCL (LossLess Codec Library) ZLIB
        public const string zmbv = "zmbv"; // Zip Motion Blocks Video
        public const string copy = "copy"; // 编码与同原始视频
    }
}
