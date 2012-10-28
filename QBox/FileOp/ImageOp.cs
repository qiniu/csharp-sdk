using System;
using QBox.RS;

namespace QBox.FileOp
{
    public class ImageOp
    {
        public Client Conn { get; private set; }

        public ImageOp(Client conn)
        {
            Conn = conn;
        }

        public ImageInfoRet ImageInfo(string url)
        {
            CallRet callRet = Conn.Call(url + "?imageInfo");
            return new ImageInfoRet(callRet);
        }

        public CallRet ImageExif(string url)
        {
            return Conn.Call(url + "?exif");
        }

        public string ImageViewUrl(string url, ImageViewSpec spec)
        {
            return url + "?imageView" + spec.MakeSpecString();
        }

        public string ImageMogrifyUrl(string url, ImageMogrifySpec spec)
        {
            return url + "?imageMogr" + spec.MakeSpecString();
        }
    }
}
