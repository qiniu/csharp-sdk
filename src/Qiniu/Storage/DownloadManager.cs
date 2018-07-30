using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.Storage
{
    /// <summary>
    ///     空间文件下载，只提供简单下载逻辑
    ///     对于大文件下载、断点续下载等需求，可以根据实际情况自行实现
    /// </summary>
    public class DownloadManager
    {
        /// <summary>
        ///     生成授权的下载链接(访问私有空间中的文件时需要使用这种链接)
        /// </summary>
        /// <param name="mac">账号(密钥)</param>
        /// <param name="domain">(私有)空间文件的下载域名</param>
        /// <param name="fileName">（私有）空间文件名</param>
        /// <param name="expireInSeconds">从生成此链接的时刻算起，该链接有效时间(单位:秒)</param>
        /// <returns>已授权的下载链接</returns>
        public static string CreatePrivateUrl(Mac mac, string domain, string fileName, int expireInSeconds = 3600)
        {
            var deadline = UnixTimestamp.GetUnixTimestamp(expireInSeconds);
            var publicUrl = CreatePublishUrl(domain, fileName);
            var sb = new StringBuilder(publicUrl);
            if (publicUrl.Contains("?"))
            {
                sb.Append($"&e={deadline}");
            }
            else
            {
                sb.Append($"?e={deadline}");
            }

            var token = Auth.CreateDownloadToken(mac, sb.ToString());
            sb.Append($"&token={token}");

            return sb.ToString();
        }

        /// <summary>
        ///     生成公开空间的下载链接
        /// </summary>
        /// <param name="domain">公开空间的文件下载域名</param>
        /// <param name="fileName">公开空间文件名</param>
        /// <returns>公开空间文件下载链接</returns>
        public static string CreatePublishUrl(string domain, string fileName)
        {
            return $"{domain}/{Uri.EscapeUriString(fileName)}";
        }

        /// <summary>
        ///     下载文件到本地
        /// </summary>
        /// <param name="url">(可访问的或者已授权的)链接</param>
        /// <param name="saveasFile">(另存为)本地文件名</param>
        /// <returns>下载资源的结果</returns>
        public static async Task<HttpResult> Download(string url, string saveasFile)
        {
            var result = new HttpResult();

            try
            {
                var httpManager = new HttpManager();

                result = await httpManager.GetAsync(url, null, true);
                if (result.Code == (int)HttpCode.OK)
                {
                    using (var fs = File.Create(saveasFile, result.Data.Length))
                    {
                        fs.Write(result.Data, 0, result.Data.Length);
                        fs.Flush();
                    }

                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [Download] Success: (Remote file) ==> \"{saveasFile}\"\n";
                }
                else
                {
                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [Download] Error: code = {result.Code}\n";
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [Download] Error:  ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_UNDEF;
                result.RefText += sb.ToString();
            }

            return result;
        }
    }
}
