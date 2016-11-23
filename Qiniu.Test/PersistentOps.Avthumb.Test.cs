using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Qiniu.Test.Utils;

namespace Qiniu.Test
{
    [TestClass]
    public class PersistentOpsAvthumbTest
    {
        [TestMethod]
        public void TestPersistentOpsAvthumb()
        {
            var sbOps = new StringBuilder("avthumb");

            var avthumb = new Models.PersistentOpsAvthumbTest(new string[] { Util.AVThumbFormats.mp2, Util.AVThumbFormats.mp3, Util.AVThumbFormats.mp4 }.RandomGetFromArray());
            sbOps.AppendFormat("/{0}", avthumb.Format);

            avthumb.BitRate = new string[] { "64k", "128k", "192k", "256k", "320k" }.RandomGetFromArray();
            sbOps.AppendFormat("/ab/{0}", avthumb.BitRate);

            avthumb.AudioQuality = new Random().Next(0, 9);
            sbOps.AppendFormat("/aq/{0}", avthumb.AudioQuality);

            avthumb.SamplingRate = new int[] { 8000, 12050, 22050, 44100 }.RandomGetFromArray();
            sbOps.AppendFormat("/ar/{0}", avthumb.SamplingRate);

            avthumb.FrameRate = new int[] { 24, 25, 30 }.RandomGetFromArray();
            sbOps.AppendFormat("/r/{0}", avthumb.FrameRate);

            avthumb.VideoBitRate = new string[] { "128k", "1.25m", "1.5m" }.RandomGetFromArray();
            sbOps.AppendFormat("/vb/{0}", avthumb.VideoBitRate);

            avthumb.VideoCodec = new string[] { Util.VideoCodec.a64multi, Util.VideoCodec.a64multi5, Util.VideoCodec.amv, Util.VideoCodec.asv1, Util.VideoCodec.asv2 }.RandomGetFromArray();
            sbOps.AppendFormat("/vcodec/{0}", avthumb.VideoCodec);

            avthumb.AudioCodec = new string[] { Util.AudioCodec.aac, Util.AudioCodec.ac3, Util.AudioCodec.ac3_fixed, Util.AudioCodec.adpcm_adx, Util.AudioCodec.adpcm_ima_qt }.RandomGetFromArray();
            sbOps.AppendFormat("/acodec/{0}", avthumb.AudioCodec);

            avthumb.Profile = new string[] { "aac_he" }.RandomGetFromArray();
            sbOps.AppendFormat("/audioProfile/{0}", avthumb.Profile);

            avthumb.SubtitleCodec = new string[] { "mov_text" }.RandomGetFromArray();
            sbOps.AppendFormat("/scodec/{0}", avthumb.SubtitleCodec);

            avthumb.SubtitleURL = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/subtitle/{0}", avthumb.SubtitleURL);

            avthumb.SeekStart = new Random().Next(1, 10000) / 100d;
            sbOps.AppendFormat("/ss/{0}s", avthumb.SeekStart);

            avthumb.Duration = new Random().Next(1, 10000) / 100d;
            sbOps.AppendFormat("/t/{0}s", avthumb.Duration);

            avthumb.Resolution = string.Format("{0}x{1}", new Random().Next(20, 3840), new Random().Next(20, 2160));
            sbOps.AppendFormat("/s/{0}", avthumb.Resolution);

            avthumb.Autoscale = new int[] { 0, 1 }.RandomGetFromArray();
            sbOps.AppendFormat("/autoscale/{0}", avthumb.Autoscale);

            avthumb.Aspect = new string[] { "4:3", "16:9" }.RandomGetFromArray();
            sbOps.AppendFormat("/aspect/{0}", avthumb.Aspect);

            avthumb.StripMeta = new int[] { 0, 1 }.RandomGetFromArray();
            sbOps.AppendFormat("/stripmeta/{0}", avthumb.StripMeta);

            avthumb.H264Crf = new Random().Next(18, 28);
            sbOps.AppendFormat("/h264Crf/{0}", avthumb.H264Crf);

            avthumb.Degree = new int[] { 90, 180, 270 }.RandomGetFromArray();
            sbOps.AppendFormat("/rotate/{0}", avthumb.Degree);

            avthumb.EncodedRemoteImageUrl = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmImage/{0}", avthumb.EncodedRemoteImageUrl);

            avthumb.Gravity = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmGravity/{0}", avthumb.Gravity);

            avthumb.EncodedText = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmText/{0}", avthumb.EncodedText);

            avthumb.GravityText = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmGravityText/{0}", avthumb.GravityText);

            avthumb.Font = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmFont/{0}", avthumb.Font);

            avthumb.FontColor = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/wmFontColor/{0}", avthumb.FontColor);

            avthumb.FontSize = new Random().Next(1, 64);
            sbOps.AppendFormat("/wmFontSize/{0}", avthumb.FontSize);

            avthumb.Xing = Guid.NewGuid().ToString();
            sbOps.AppendFormat("/writeXing/{0}", avthumb.Xing);

            avthumb.AudioNo = new int[] { 0, 1 }.RandomGetFromArray();
            sbOps.AppendFormat("/an/{0}", avthumb.AudioNo);

            avthumb.VideoNo = new int[] { 0, 1 }.RandomGetFromArray();
            sbOps.AppendFormat("/vn/{0}", avthumb.VideoNo);

            avthumb.SaveAs = Guid.NewGuid().ToString();
            sbOps.AppendFormat("|saveas/{0}", avthumb.SaveAs);

            var expected = sbOps.ToString();
            var actual = avthumb.GetOpsString();
            Assert.AreEqual(expected, actual);
        }
    }
}
