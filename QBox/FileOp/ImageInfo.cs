using System;
using QBox.RPC;

namespace QBox.FileOp
{
    public static class ImageInfo
    {
        public static string MakeRequest(string url)
        {
            return url + "?imageInfo";
        }

        public static ImageInfoRet Call(string url)
        {
            CallRet callRet = FileOpClient.Get(url);
            return new ImageInfoRet(callRet);
        }
    }
}
