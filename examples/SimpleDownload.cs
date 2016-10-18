using Qiniu.Util;
using Qiniu.Storage;

namespace QiniuDemo
{
    public class SimpleDownload
    {
        public static void downloadFile()
        {
            Mac mac = new Mac(Settings.AccessKey, Settings.SecretKey);

            // 加上过期参数，使用?e=<UnixTimestamp>
            string rawUrl = "RAW_URL" + "?e=1482207600"; 
            string token = Auth.createDownloadToken(rawUrl, mac);
            string accUrl = rawUrl + "&token=" + token;

            System.Console.WriteLine(accUrl);
        }
    }
}