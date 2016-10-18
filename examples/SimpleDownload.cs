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
			
			// 生成已授权的链接accUrl,有效期限在?e=时间戳部分
            string accUrl = rawUrl + "&token=" + token;

			// 根据链接访问(下载)文件
            // ...
			
            System.Console.WriteLine(accUrl);
        }
    }
}