namespace Qiniu.Common
{
    /// <summary>
    /// 账户访问控制(密钥)
    /// </summary>
    public class Mac
    {
        public string AccessKey { set; get; }

        public string SecretKey { set; get; }

        public Mac(string accessKey, string secretKey)
        {
            this.AccessKey = accessKey;
            this.SecretKey = secretKey;
        }
    }
}