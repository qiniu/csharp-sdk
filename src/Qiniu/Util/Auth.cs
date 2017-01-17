namespace Qiniu.Util
{
    /// <summary>
    /// Authentication/Authorization
    /// </summary>
    public class Auth
    {
        private Signature signature;

        /// <summary>
        /// 一般初始化
        /// </summary>
        /// <param name="mac">账号</param>
        public Auth(Mac mac)
        {
            signature = new Signature(mac);
        }

        /// <summary>
        /// 生成管理凭证
        /// 有关管理凭证请参阅
        /// http://developer.qiniu.com/article/developer/security/access-token.html
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="body">请求的主体内容</param>
        /// <returns>生成的管理凭证</returns>
        public string createManageToken(string url,byte[] body)
        {
            return string.Format("QBox {0}", signature.signRequest(url, body));
        }

        /// <summary>
        /// 生成管理凭证-不包含body
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>生成的管理凭证</returns>
        public string createManageToken(string url)
        {
            return createManageToken(url, null);
        }

        /// <summary>
        /// 生成上传凭证
        /// </summary>
        /// <param name="jsonBody">上传策略JSON串</param>
        /// <returns>生成的上传凭证</returns>
        public string createUploadToken(string jsonBody)
        {
            return signature.signWithData(jsonBody);
        }

        /// <summary>
        /// 生成下载凭证
        /// </summary>
        /// <param name="url">原始链接</param>
        /// <returns></returns>
        public string createDownloadToken(string url)
        {
            return signature.sign(url);
        }

        /// <summary>
        /// 生成推流地址使用的凭证
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string createStreamPublishToken(string path)
        {
            return signature.sign(path);
        }

        /// <summary>
        /// 生成流管理凭证
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string createStreamManageToken(string data)
        {
            return string.Format("Qiniu {0}", signature.signWithData(data));
        }

        /// <summary>
        /// 生成管理凭证
        /// 有关管理凭证请参阅
        /// http://developer.qiniu.com/article/developer/security/access-token.html
        /// </summary>
        /// <param name="mac">账号</param>
        /// <param name="url">访问的URL</param>
        /// <param name="body">请求的body</param>
        /// <returns>生成的管理凭证</returns>
        public static string createManageToken(Mac mac, string url, byte[] body)
        {
            Signature sx = new Signature(mac);
            return string.Format("QBox {0}", sx.signRequest(url, body));
        }

        /// <summary>
        /// 生成管理凭证-不包含body
        /// </summary>
        /// <param name="mac">账号</param>
        /// <param name="url">请求的URL</param>
        /// <returns>生成的管理凭证</returns>
        public static string createManageToken(Mac mac, string url)
        {
            return createManageToken(mac, url, null);
        }

        /// <summary>
        /// 生成上传凭证
        /// </summary>
        /// <param name="mac">账号</param>
        /// <param name="jsonBody">上传策略JSON串</param>
        /// <returns>生成的上传凭证</returns>
        public static string createUploadToken(Mac mac, string jsonBody)
        {
            Signature sx = new Signature(mac);
            return sx.signWithData(jsonBody);
        }

        /// <summary>
        /// 生成下载凭证
        /// </summary>
        /// <param name="mac">账号</param>
        /// <param name="url">原始链接</param>
        /// <returns></returns>
        public static string createDownloadToken(Mac mac, string url)
        {
            Signature sx = new Signature(mac);
            return sx.sign(url);
        }

        /// <summary>
        /// 生成推流地址使用的凭证
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string createStreamPublishToken(Mac mac,string path)
        {
            Signature sx = new Signature(mac);
            return sx.sign(path);
        }

        /// <summary>
        /// 生成流管理凭证
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string createStreamManageToken(Mac mac, string data)
        {
            Signature sx = new Signature(mac);
            return string.Format("Qiniu {0}", sx.sign(data));
        }

    }
}
