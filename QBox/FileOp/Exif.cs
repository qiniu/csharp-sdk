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

        public static CallRet Call(string url)
        {
            return FileOpClient.Get(url);
        }
    }
}
