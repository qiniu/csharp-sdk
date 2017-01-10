using Qiniu.Common;
using Qiniu.IO.Model;
using System.IO;
using Qiniu.Http;

namespace Qiniu.IO
{
    /// <summary>
    /// 上传管理器，根据文件大小以及阈值设置自动选择合适的上传方式，支持以下(1)(2)(3)
    /// 
    /// (1)网络较好并且待上传的文件体积较小时使用简单上传
    /// 
    /// (2)文件较大或者网络状况不理想时请使用分片上传
    /// 
    /// (3)文件较大上传需要花费较长时间，建议使用断点续上传
    /// </summary>
    public class UploadManager
    {
        // 根据此阈值确定是否使用分片上传(默认值1MB)
        private long PUT_THRESHOLD = 1048576;

        // 分片上传的ChunkSize(默认值2MB)
        private ChunkUnit CHUNK_UNIT = ChunkUnit.U2048K;

        // 是否从CDN上传
        private bool UPLOAD_FROM_CDN = false;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="putThreshold">根据文件大小选择简单上传或分片上传，阈值</param>
        /// <param name="uploadFromCDN">是否从CDN上传</param>
        public UploadManager(long putThreshold = 1048576, bool uploadFromCDN = false)
        {
            PUT_THRESHOLD = putThreshold;
            UPLOAD_FROM_CDN = uploadFromCDN;
        }

        /// <summary>
        /// 设置分片上传的“片”大小(单位:字节)
        /// </summary>
        /// <param name="cu"></param>
        public void setChunkUnit(ChunkUnit cu)
        {
            CHUNK_UNIT = cu;
        }

        /// <summary>
        /// 生成上传凭证
        /// 有关上传策略请参阅 http://developer.qiniu.com/article/developer/security/put-policy.html
        /// 有关上传凭证请参阅 http://developer.qiniu.com/article/developer/security/upload-token.html
        /// </summary>
        /// <param name="mac">账户访问控制(密钥)</param>
        /// <param name="putPolicy">上传策略</param>
        /// <returns></returns>
        public static string createUploadToken(Mac mac,PutPolicy putPolicy)
        {
            Signature sx = new Signature(mac);
            return sx.signWithData(putPolicy.ToJsonString());
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传结果</returns>
        public HttpResult uploadFile(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            FileInfo fi = new FileInfo(localFile);
            if (fi.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = ru.uploadFile(localFile, saveKey, token);
            }
            else
            {
                SimpleUploader su = new SimpleUploader(UPLOAD_FROM_CDN);
                result = su.uploadFile(localFile, saveKey, token);
            }

            return result;
        }

        /// <summary>
        /// 上传数据流
        /// </summary>
        /// <param name="stream">待上传的数据流</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传结果</returns>
        public HttpResult uploadStream(Stream stream, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (stream.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = ru.uploadStream(stream, saveKey, token, null);
            }
            else
            {
                SimpleUploader su = new SimpleUploader(UPLOAD_FROM_CDN);
                result = su.uploadStream(stream, saveKey, token);
            }

            return result;
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传结果</returns>
        public HttpResult uploadData(byte[] data, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (data.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = ru.uploadData(data, saveKey, token, null);
            }
            else
            {
                SimpleUploader su = new SimpleUploader(UPLOAD_FROM_CDN);
                result = su.uploadData(data, saveKey, token);
            }

            return result;
        }

    }
}
