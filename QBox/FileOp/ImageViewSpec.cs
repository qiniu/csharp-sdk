﻿using System;

namespace QBox.FileOp
{
    public class ImageViewSpec
    {
        public int Mode { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Quality { get; set; }
        public string Format { get; set; }
        public int Sharpen { get; set; }

        public string MakeSpecString()
        {
            string spec = "?imageView/" + Mode.ToString();
            if (Width != 0)
                spec += "/w/" + Width.ToString();
            if (Height != 0)
                spec += "/h/" + Height.ToString();
            if (Quality != 0)
                spec += "/q/" + Quality.ToString();
            if (!String.IsNullOrEmpty(Format))
                spec += "/format/" + Format;
            if (Sharpen != 0)
                spec += "/sharpen/" + Sharpen;
            return spec;
        }
    }
}
