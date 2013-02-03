using System;
using System.Text;
using System.IO;

namespace QBox.Util
{
    public static class StreamUtil
    {
        public static int bufferLen = 32*1024;

        public static void Copy(Stream src, Stream dst)
        {
            byte[] buffer = new byte[bufferLen];
            while (true)
            {
                int n = src.Read(buffer, 0, bufferLen);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
            }
        }

        public static void CopyN(Stream src, Stream dst, long numBytesToCopy)
        {
            byte[] buffer = new byte[bufferLen];
            long numBytesWritten = 0;
            while (numBytesWritten < numBytesToCopy)
            {
                int len = bufferLen;
                if ((numBytesToCopy - numBytesWritten) < len)
                {
                    len = (int)(numBytesToCopy - numBytesWritten);
                }
                int n = src.Read(buffer, 0, len);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
                numBytesWritten += n;
            }
            if (numBytesWritten != numBytesToCopy)
            {
                throw new Exception("StreamUtil.CopyN: nwritten not equal to ncopy");
            }
        }
    }
}
