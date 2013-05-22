using System;
using QBox.RPC;

namespace QBox.FileOp
{
    public static class Exif
    {
        public static string MakeRequest(string url)
        {
            return url + "?exif";
        }

        public static ExifRet Call(string url)
        {
            CallRet callRet = FileOpClient.Get(url);
            return new ExifRet(callRet);
        }
    }
}
