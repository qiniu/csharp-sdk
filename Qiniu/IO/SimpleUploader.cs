using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Qiniu.Common;
using Qiniu.IO.Model;
using Qiniu.Http;

namespace Qiniu.IO
{
    /// <summary>
    /// 简单上传，适合于以下情形(1)
    /// (1)网络较好并且待上传的文件体积较小时使用简单上传
    /// (2)文件较大或者网络状况不理想时请使用分片上传
    /// (3)文件较大上传需要花费较常时间，建议使用断点续上传
    /// </summary>
    public class SimpleUploader
    {
        private HttpClient client;

        private HttpHelper helper;

        public SimpleUploader()
        {
            client = new HttpClient();
            helper = new HttpHelper();
        }

        /// <summary>
        /// 上传文件
        /// 需要提供正确的上传凭证(参阅[1]，另关于上传策略请参阅[2])
        /// [1] http://developer.qiniu.com/article/developer/security/upload-token.html
        /// [2] http://developer.qiniu.com/article/developer/security/put-policy.html
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, Config.ZONE.UpHost);
                req.Headers.Add("User-Agent", helper.GetUserAgent());

                string boundary = helper.CreateFormDataBoundary();
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("--{0}\r\nContent-Disposition: form-data; name=\"key\"\r\n\r\n{1}\r\n", boundary, saveKey);
                sb.AppendFormat("--{0}\r\nContent-Disposition: form-data; name=\"token\"\r\n\r\n{1}\r\n", boundary, token);
                sb.AppendFormat("--{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{1}\"\r\n", boundary, saveKey);
                sb.AppendFormat("Content-Type: {0}\r\n\r\n", "application/octet-stream");

                using (FileStream fs = new FileStream(localFile, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        sb.Append(sr.ReadToEnd());
                    }
                }
                sb.AppendFormat("\r\n--{0}--\r\n\r\n", boundary);

                byte[] content = Encoding.UTF8.GetBytes(sb.ToString());
                req.Content = new ByteArrayContent(content);
                req.Content.Headers.Add("Content-Type", string.Format("multipart/form-data; boundary={0}", boundary));

                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[SimpleUpload] Error:" + ex.Message;
            }

            return result;
        }

    }
}
