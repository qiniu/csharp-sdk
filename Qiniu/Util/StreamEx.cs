using System;
using System.IO;

namespace Qiniu.Util
{
    public static class StreamEx
    {
        public static Stream ToStream(this string str)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(str);
                    writer.Flush();
                    stream.Position = 0;
                    return stream;
                }
            }
        }
    }
}
