
using Newtonsoft.Json;
using System;
using System.IO;
namespace Qiniu.Util
{

    public class Utils
    {
        public static int bufferLen = 32 * 1024;

        public static string JsonEncode(object obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static void Copy(Stream dst, Stream src)
        {
            long l = src.Position;
            byte[] buffer = new byte[bufferLen];
            while (true)
            {
                int n = src.Read(buffer, 0, bufferLen);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
            }
            src.Seek(l, SeekOrigin.Begin);
        }
        public static void CopyN(Stream dst, Stream src, long numBytesToCopy)
        {
            long l = src.Position;
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
            src.Seek(l, SeekOrigin.Begin);
            if (numBytesWritten != numBytesToCopy)
            {
                throw new Exception("StreamUtil.CopyN: nwritten not equal to ncopy");
            }
        }
        public static string UserAgent
        {
            get
            {

                string csharpVersion = "csharp";
                string os = "windows";
                string sdk = Config.USER_AGENT + Config.SDK_VERSION;
                return sdk + os + csharpVersion;
            }
        }

        /*
         * check the arg.
         * 1. arg == null, return false, treat as empty situation
         * 2. arg == "", return false, treat as empty situation
         * 3. arg == " " or arg == "   ", return false, treat as empty situation
         * 4. return true, only if the arg is a illegal string
         *
         * */
        public static bool isArgNotEmpty(string arg)
        {
            return arg != null && arg.Trim().Length > 0;
        }

        //    public static String getPath(String streamId) {
        //        String[] res = streamId.split("\\.");
        //        // res[1] -> hub, res[2] -> title
        //        return String.format("/%s/%s", res[1], res[2]);
        //    }
    }

}
