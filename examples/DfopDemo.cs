using System;
using Qiniu.Util;
using Qiniu.Processing;

namespace CSharpSDKExamples
{
    public class DfopDemo
    {
        /// <summary>
        /// dfop形式1:URL
        /// </summary>
        public static void dfopUrl()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            Dfop dfx = new Dfop(mac);

            string fop = "FOP"; // E.G.: "imageInfo"
            string url = "RES_URL";

            var ret = dfx.dfop(fop, url);

            if (ret.ResponseInfo.StatusCode != 200)
            {
                Console.WriteLine(ret.ResponseInfo);
            }
            Console.WriteLine(ret.Response);
        }

        /// <summary>
        /// dfop形式2:Data
        /// </summary>
        public static void dfopData()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);
            Dfop dfx = new Dfop(mac);

            string fop = "FOP";
            byte[] data = System.IO.File.ReadAllBytes("LOCAL_FILE");

            var ret = dfx.dfop(fop, data);

            if (ret.ResponseInfo.StatusCode != 200)
            {
                Console.WriteLine(ret.ResponseInfo);
            }
            Console.WriteLine(ret.Response);
        }
    }
}
