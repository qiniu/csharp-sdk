using System.IO;
using System.Threading.Tasks;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     上传管理器，根据文件/数据(流)大小以及阈值设置自动选择合适的上传方式
    /// </summary>
    public class UploadManager
    {
        private readonly Config _config;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="config">文件上传的配置信息</param>
        public UploadManager(Config config)
        {
            _config = config;
        }

        /// <summary>
        ///     上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="key">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传文件后的返回结果</returns>
        public Task<HttpResult> UploadData(byte[] data, string key, string token, PutExtra extra)
        {
            var formUploader = new FormUploader(_config);
            return formUploader.UploadData(data, key, token, extra);
        }

        /// <summary>
        ///     上传文件，根据文件大小以及设置的阈值(用户初始化UploadManager时可指定该值)自动选择：
        ///     若文件大小超过设定阈值，使用ResumableUploader，否则使用FormUploader
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="key">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传文件后的返回结果</returns>
        public Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra extra)
        {
            var fi = new System.IO.FileInfo(localFile);
            if (fi.Length > _config.PutThreshold)
            {
                var resumeUploader = new ResumableUploader(_config);
                return resumeUploader.UploadFile(localFile, key, token, extra);
            }

            var formUploader = new FormUploader(_config);
            return formUploader.UploadFile(localFile, key, token, extra);
        }


        /// <summary>
        ///     上传文件数据流，根据文件大小以及设置的阈值(用户初始化UploadManager时可指定该值)自动选择：
        ///     若文件大小超过设定阈值，使用ResumableUploader，否则使用FormUploader
        /// </summary>
        /// <param name="stream">待上传的数据流</param>
        /// <param name="key">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extra">上传可选设置</param>
        /// <returns>上传文件后的返回结果</returns>
        public Task<HttpResult> UploadStream(Stream stream, string key, string token, PutExtra extra)
        {
            if (stream.Length > _config.PutThreshold)
            {
                var resumeUploader = new ResumableUploader(_config);
                return resumeUploader.UploadStream(stream, key, token, extra);
            }

            var formUploader = new FormUploader(_config);
            return formUploader.UploadStream(stream, key, token, extra);
        }
    }
}
