using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Qiniu.Common;
using Qiniu.Http;

namespace Qiniu.IO
{
    /// <summary>
    /// 空间文件下载
    /// </summary>
    public class DownloadManager
    {
        private Signature signature;
        private HttpClient client;

        public DownloadManager(Mac mac)
        {
            signature = new Signature(mac);
            client = new HttpClient();
        }

        /// <summary>
        /// 生成下载凭证
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string CreateDownloadToken(string url)
        {
            return signature.Sign(url);
        }

        /// <summary>
        /// 生成授权的下载链接(访问私有空间中的文件时需要使用)
        /// </summary>
        /// <param name="url">初始链接</param>
        /// <param name="expireInSeconds">有效时间</param>
        /// <returns></returns>
        public string CreateSignedUrl(string url,int expireInSeconds)
        {
            string deadline = "1478341290";
            StringBuilder sb = new StringBuilder(url);
            if (url.Contains('?'))
            {
                sb.AppendFormat("&e={0}", deadline);
            }
            else
            {
                sb.AppendFormat("?e={0}", deadline);
            }
            string token = CreateDownloadToken(sb.ToString());
            sb.AppendFormat("&token={0}", token);

            return sb.ToString();
        }

        /// <summary>
        /// 下载文件到本地
        /// </summary>
        /// <param name="signedUrl">(可访问的)链接</param>
        /// <param name="saveasFile">(另存为)本地文件名</param>
        /// <returns></returns>
        public HttpResult Download(string signedUrl,string saveasFile)
        {
            HttpResult result = new HttpResult();
             
            try
            {
                var msg = client.GetAsync(signedUrl);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsByteArrayAsync();               
                using (FileStream fs = File.Create(saveasFile, ret.Result.Length))
                {
                    fs.Write(ret.Result, 0, ret.Result.Length);
                    fs.Flush();
                }
                result.Message = string.Format("[Download] Success: (Remote file) ==> \"{0}\"", saveasFile);
            }
            catch(Exception ex)
            {
                result.Message = "[Download] Error: " + ex.Message;
            }

            return result;
        }

    }
}
