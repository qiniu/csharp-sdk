using System;

namespace QBox.FileOp
{
    public class ImageMogrifySpec
    {
        public string Thumbnail { get; set; }
        public string Gravity { get; set; }
        public string Crop { get; set; }
        public int Quality { get; set; }
        public int Rotate { get; set; }
        public string Format { get; set; }
        public bool AutoOrient { get; set; }

        public string MakeSpecString()
        {
            string spec = "";
            if (!String.IsNullOrEmpty(Thumbnail))
                spec += "/thumbnail/" + Thumbnail;
            if (!String.IsNullOrEmpty(Gravity))
                spec += "/gravity/" + Gravity;
            if (!String.IsNullOrEmpty(Crop))
                spec += "/crop/" + Crop;
            if (Quality != 0)
                spec += "/quality/" + Quality.ToString();
            if (Rotate != 0)
                spec += "/rotate/" + Rotate.ToString();
            if (!String.IsNullOrEmpty(Format))
                spec += "/format/" + Format;
            if (AutoOrient)
                spec += "/auto-orient";
            return spec;
        }
    }
}
