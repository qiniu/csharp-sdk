using System;

namespace Qiniu.Util
{
    /// <summary>
    /// 账户访问控制(密钥)
    /// </summary>
    public class Mac
    {
        /// <summary>
        /// 密钥-AccessKey
        /// </summary>
        public string AccessKey { init; get; }

        /// <summary>
        /// 密钥-SecretKey
        /// </summary>
        public string SecretKey { init; get; }

        /// <summary>
        /// 初始化密钥AK/SK
        /// </summary>
        /// <param name="accessKey">AccessKey</param>
        /// <param name="secretKey">SecretKey</param>
        public Mac(string accessKey, string secretKey)
        {
            ArgumentNullException.ThrowIfNull(accessKey);
            ArgumentNullException.ThrowIfNull(secretKey);

            AccessKey = accessKey;
            SecretKey = secretKey;
        }
    }
}