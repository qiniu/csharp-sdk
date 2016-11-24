using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    public abstract class AudioCodec
    {
        private AudioCodec() { }
        public const string aac = "aac"; // AAC (Advanced Audio Coding)
        public const string libfaac = "libfaac"; // 
        public const string ac3 = "ac3"; // ATSC A/52A (AC-3)
        public const string ac3_fixed = "ac3_fixed"; // ATSC A/52A (AC-3) (codec ac3)
        public const string adpcm_adx = "adpcm_adx"; // SEGA CRI ADX ADPCM
        public const string g722 = "g722"; // G.722 ADPCM (codec adpcm_g722)
        public const string g726 = "g726"; // G.726 ADPCM (codec adpcm_g726)
        public const string adpcm_ima_qt = "adpcm_ima_qt"; // ADPCM IMA QuickTime
        public const string adpcm_ima_wav = "adpcm_ima_wav"; // ADPCM IMA WAV
        public const string adpcm_ms = "adpcm_ms"; // ADPCM Microsoft
        public const string adpcm_swf = "adpcm_swf"; // ADPCM Shockwave Flash
        public const string adpcm_yamaha = "adpcm_yamaha"; // ADPCM Yamaha
        public const string alac = "alac"; // ALAC (Apple Lossless Audio Codec)
        public const string libopencore_amrnb = "libopencore_amrnb"; // OpenCORE AMR-NB (Adaptive Multi-Rate Narrow-Band) (codec amr_nb)
        public const string comfortnoise = "comfortnoise"; // RFC 3389 comfort noise generator
        public const string dca = "dca"; // DCA (DTS Coherent Acoustics) (codec dts)
        public const string eac3 = "eac3"; // ATSC A/52 E-AC-3
        public const string flac = "flac"; // FLAC (Free Lossless Audio Codec)
        public const string g723_1 = "g723_1"; // G.723.1
        public const string libilbc = "libilbc"; // iLBC (Internet Low Bitrate Codec) (codec ilbc)
        public const string mp2 = "mp2"; // MP2 (MPEG audio layer 2)
        public const string libmp3lame = "libmp3lame"; // libmp3lame MP3 (MPEG audio layer 3) (codec mp3)
        public const string nellymoser = "nellymoser"; // Nellymoser Asao
        public const string pcm_alaw = "pcm_alaw"; // PCM A-law / G.711 A-law
        public const string pcm_f32be = "pcm_f32be"; // PCM 32-bit floating point big-endian
        public const string pcm_f32le = "pcm_f32le"; // PCM 32-bit floating point little-endian
        public const string pcm_f64be = "pcm_f64be"; // PCM 64-bit floating point big-endian
        public const string pcm_f64le = "pcm_f64le"; // PCM 64-bit floating point little-endian
        public const string pcm_mulaw = "pcm_mulaw"; // PCM mu-law / G.711 mu-law
        public const string pcm_s16be = "pcm_s16be"; // PCM signed 16-bit big-endian
        public const string pcm_s16be_planar = "pcm_s16be_planar"; // PCM signed 16-bit big-endian planar
        public const string pcm_s16le = "pcm_s16le"; // PCM signed 16-bit little-endian
        public const string pcm_s16le_planar = "pcm_s16le_planar"; // PCM signed 16-bit little-endian planar
        public const string pcm_s24be = "pcm_s24be"; // PCM signed 24-bit big-endian
        public const string pcm_s24daud = "pcm_s24daud"; // PCM D-Cinema audio signed 24-bit
        public const string pcm_s24le = "pcm_s24le"; // PCM signed 24-bit little-endian
        public const string pcm_s24le_planar = "pcm_s24le_planar"; // PCM signed 24-bit little-endian planar
        public const string pcm_s32be = "pcm_s32be"; // PCM signed 32-bit big-endian
        public const string pcm_s32le = "pcm_s32le"; // PCM signed 32-bit little-endian
        public const string pcm_s32le_planar = "pcm_s32le_planar"; // PCM signed 32-bit little-endian planar
        public const string pcm_s8 = "pcm_s8"; // CM signed 8-bit
        public const string pcm_s8_planar = "pcm_s8_planar"; // PCM signed 8-bit planar
        public const string pcm_u16be = "pcm_u16be"; // PCM unsigned 16-bit big-endian
        public const string pcm_u16le = "pcm_u16le"; // PCM unsigned 16-bit little-endian
        public const string pcm_u24be = "pcm_u24be"; // PCM unsigned 24-bit big-endian
        public const string pcm_u24le = "pcm_u24le"; // PCM unsigned 24-bit little-endian
        public const string pcm_u32be = "pcm_u32be"; // PCM unsigned 32-bit big-endian
        public const string pcm_u32le = "pcm_u32le"; // PCM unsigned 32-bit little-endian
        public const string pcm_u8 = "pcm_u8"; // PCM unsigned 8-bit
        public const string real_144 = "real_144"; // RealAudio 1.0 (14.4K) (codec ra_144)
        public const string roq_dpcm = "roq_dpcm"; // id RoQ DPCM
        public const string s302m = "s302m"; // SMPTE 302M
        public const string sonic = "sonic"; // Sonic
        public const string sonicls = "sonicls"; // Sonic lossless
        public const string libspeex = "libspeex"; // libspeex Speex (codec speex)
        public const string tta = "tta"; // TTA (True Audio)
        public const string vorbis = "vorbis"; // Vorbis
        public const string libvorbis = "libvorbis"; // ibvorbis (codec vorbis)
        public const string wavpack = "wavpack"; // 无损音频压缩格式
        public const string wmav1 = "wmav1"; // Windows Media Audio 1
        public const string wmav2 = "wmav2"; // Windows Media Audio 2
        public const string copy = "copy"; // 编码同原始音频
    }
}
