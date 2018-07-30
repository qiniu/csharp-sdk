using System;
using System.IO;
using System.Text;
#if WINDOWS_UWP
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
#else
using System.Security.Cryptography;

#endif

namespace Qiniu.Util
{
    /// <summary>
    ///     签名/加密
    ///     特别注意，不同平台使用的Cryptography可能略有不同，使用中如有遇到问题，请反馈
    ///     提交您的issue到 https://github.com/qiniu/csharp-sdk
    /// </summary>
    public class Signature
    {
        private readonly Mac _mac;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="mac">账号(密钥)</param>
        public Signature(Mac mac)
        {
            _mac = mac;
        }

        private string EncodedSign(byte[] data)
        {
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(_mac.SecretKey));
            var digest = hmac.ComputeHash(data);
            return Base64.UrlSafeBase64Encode(digest);
        }

        private string EncodedSign(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            return EncodedSign(data);
        }

        /// <summary>
        ///     签名-字节数据
        /// </summary>
        /// <param name="data">待签名的数据</param>
        /// <returns></returns>
        public string Sign(byte[] data)
        {
            return $"{_mac.AccessKey}:{EncodedSign(data)}";
        }

        /// <summary>
        ///     签名-字符串数据
        /// </summary>
        /// <param name="str">待签名的数据</param>
        /// <returns></returns>
        public string Sign(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            return Sign(data);
        }

        /// <summary>
        ///     附带数据的签名
        /// </summary>
        /// <param name="data">待签名的数据</param>
        /// <returns></returns>
        public string SignWithData(byte[] data)
        {
            var sstr = Base64.UrlSafeBase64Encode(data);
            return $"{_mac.AccessKey}:{EncodedSign(sstr)}:{sstr}";
        }

        /// <summary>
        ///     附带数据的签名
        /// </summary>
        /// <param name="str">待签名的数据</param>
        /// <returns>签名结果</returns>
        public string SignWithData(string str)
        {
            var data = Encoding.UTF8.GetBytes(str);
            return SignWithData(data);
        }

        /// <summary>
        ///     HTTP请求签名
        /// </summary>
        /// <param name="url">请求目标的URL</param>
        /// <param name="body">请求的主体数据</param>
        /// <returns></returns>
        public string SignRequest(string url, byte[] body)
        {
            var u = new Uri(url);
            var pathAndQuery = u.PathAndQuery;
            var pathAndQueryBytes = Encoding.UTF8.GetBytes(pathAndQuery);

            using (var buffer = new MemoryStream())
            {
                buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                buffer.WriteByte((byte)'\n');
                if (body != null && body.Length > 0)
                {
                    buffer.Write(body, 0, body.Length);
                }

                var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(_mac.SecretKey));
                var digest = hmac.ComputeHash(buffer.ToArray());
                var digestBase64 = Base64.UrlSafeBase64Encode(digest);
                return $"{_mac.AccessKey}:{digestBase64}";
            }
        }

        /// <summary>
        ///     HTTP请求签名
        /// </summary>
        /// <param name="url">请求目标的URL</param>
        /// <param name="body">请求的主体数据</param>
        /// <returns></returns>
        public string SignRequest(string url, string body)
        {
            var data = Encoding.UTF8.GetBytes(body);
            return SignRequest(url, data);
        }
    }
}
