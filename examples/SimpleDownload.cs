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
            // 如果rawUrl中已包含?，则改用&e=<UnixTimestamp>
            string rawUrl = "RAW_URL";
            string expireAt = "UNIX_TIMESTAMP";
            string mid = "?e=";
            if(rawUrl.Contains("?"))
            {
                mid = "&e=";
            }
            string token = Auth.createDownloadToken(rawUrl + mid + expireAt, mac);
            string accUrl = rawUrl + mid + expireAt + "&token=" + token;
            // 接下来可以使用accUrl来下载文件

            System.Console.WriteLine(accUrl);            
        }
    }
}