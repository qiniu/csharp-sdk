using Qiniu.Common;
using Qiniu.IO.Model;
using System.IO;
using Qiniu.Http;

namespace Qiniu.IO
{
    /// <summary>
    /// 上传管理器，根据文件大小以及阈值设置自动选择合适的上传方式，支持以下(1)(2)(3)
    /// (1)网络较好并且待上传的文件体积较小时使用简单上传
    /// (2)文件较大或者网络状况不理想时请使用分片上传
    /// (3)文件较大上传需要花费较常时间，建议使用断点续上传
    /// </summary>
    public class UploadManager
    {
        // 根据此阈值确定是否使用分片上传(默认值1MB)
        private long PUT_THRESHOLD = 1048576;

        private Signature signature;

        public UploadManager(Mac mac, long putThreshold= 1048576)
        {
            signature = new Signature(mac);

            PUT_THRESHOLD = putThreshold;
        }

        /// <summary>
        /// 生成上传凭证
        /// 参阅 http://developer.qiniu.com/article/developer/security/upload-token.html
        /// </summary>
        /// <param name="putPolicy"></param>
        /// <returns></returns>
        public string CreateUploadToken(PutPolicy putPolicy)
        {
            return signature.SignWithData(putPolicy.ToJsonString());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="putPolicy">上传策略</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, PutPolicy putPolicy)
        {
            string token = CreateUploadToken(putPolicy);
            return UploadFile(localFile, saveKey, token);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            FileInfo fi = new FileInfo(localFile);
            if (fi.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader();
                result = ru.UploadFile(localFile, saveKey, token);
            }
            else
            {
                SimpleUploader su = new SimpleUploader();
                result = su.UploadFile(localFile, saveKey, token);
            }

            return result;
        }

    }
}
