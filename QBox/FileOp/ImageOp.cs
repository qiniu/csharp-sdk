using System;
using QBox.RPC;

namespace QBox.FileOp
{
    public static class ImageOp
    {
        public static ImageInfoRet ImageInfo(string url)
        {
            CallRet callRet = FileOpClient.Get(url + "?imageInfo");
            return new ImageInfoRet(callRet);
        }

        public static CallRet ImageExif(string url)
        {
            return FileOpClient.Get(url + "?exif");
        }

        public static string ImageViewUrl(string url, ImageViewSpec spec)
        {
            return url + spec.MakeSpecString();
        }

        public static string ImageMogrifyUrl(string url, ImageMogrifySpec spec)
        {
            return url + spec.MakeSpecString();
        }
    }
}
