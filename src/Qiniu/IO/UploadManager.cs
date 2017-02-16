using System.IO;
using Qiniu.Util;
using Qiniu.IO.Model;
using Qiniu.Http;

#if Net45 || Net46 || NetCore 
using System.Threading.Tasks;
#endif

#if WINDOWS_UWP
using System;
using System.Threading.Tasks;
using Windows.Storage;
#endif

namespace Qiniu.IO
{
    /// <summary>
    /// 上传管理器，根据文件/数据(流)大小以及阈值设置自动选择合适的上传方式
    /// </summary>
    public class UploadManager
    {
        // 根据此阈值确定是否使用分片上传(默认值10MB)
        private long PUT_THRESHOLD = 10485760;

        // 分片上传的ChunkSize(默认值2MB)
        private ChunkUnit CHUNK_UNIT = ChunkUnit.U2048K;

        // 是否从CDN上传
        private bool UPLOAD_FROM_CDN = false;

        // 上传进度处理器 - 仅用于上传大文件
        private UploadProgressHandler upph = null;

        // 上传进度处理器 - 仅用于上传数据流
        private StreamProgressHandler sph = null;

        // 上传控制器 - 仅用于上传大文件
        private UploadController upctl = null;

        // 上传记录文件 - 仅用于上传大文件
#if WINDOWS_UWP
        private StorageFile recordFile = null;
#else
        private string recordFile = null;
#endif

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="putThreshold">选择简单上传或分片上传的“阈值”，默认10MB</param>
        /// <param name="uploadFromCDN">是否从CDN上传(默认否)，使用CDN上传可能会有更好的效果</param>
        public UploadManager(long putThreshold = 10485760, bool uploadFromCDN = false)
        {
            PUT_THRESHOLD = putThreshold;
            UPLOAD_FROM_CDN = uploadFromCDN;
        }

        /// <summary>
        /// 设置上传进度处理器-仅对于上传大文件有效，如果不设置则使用默认的进度处理器
        /// </summary>
        /// <param name="upph">上传进度处理器</param>
        public void SetUploadProgressHandler(UploadProgressHandler upph)
        {
            this.upph = upph;
        }

        /// <summary>
        /// 设置上传进度处理器-仅对于上传数据流有效，如果不设置则使用默认的进度处理器
        /// </summary>
        /// <param name="sph">数据流上传进度处理器</param>
        public void SetStreamrogressHandler(StreamProgressHandler sph)
        {
            this.sph = sph;
        }

        /// <summary>
        /// 设置上传控制器-仅对于上传大文件有效，如不设置则使用默认控制器
        /// </summary>
        /// <param name="upctl">上传控制器</param>
        public void SetUploadController(UploadController upctl)
        {
            this.upctl = upctl;
        }

#if WINDOWS_UWP
        /// <summary>
        /// 设置断点记录文件-仅对于上传大文件有效
        /// </summary>
        /// <param name="recordFile">记录文件</param>
        public void SetRecordFile(StorageFile recordFile)
        {
            this.recordFile = recordFile;
        }
#else

        /// <summary>
        /// 设置断点记录文件-仅对于上传大文件有效
        /// </summary>
        /// <param name="recordFile">记录文件</param>
        public void SetRecordFile(string recordFile)
        {
            this.recordFile = recordFile;
        }
#endif

        /// <summary>
        /// 设置分片上传的“片”大小(单位:字节)，如过不设置则为默认的2MB
        /// </summary>
        /// <param name="chunkUnit">分片大小</param>
        public void SetChunkUnit(ChunkUnit chunkUnit)
        {
            CHUNK_UNIT = chunkUnit;
        }

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        /// <summary>
        /// 上传文件，根据文件大小以及设置的阈值(用户初始化UploadManager时可指定该值)自动选择：
        /// 若文件大小超过设定阈值，使用ResumableUploader，否则使用FormUploader
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            FileInfo fi = new FileInfo(localFile);
            if (fi.Length > PUT_THRESHOLD)
            {
                if (string.IsNullOrEmpty(recordFile))
                {
                    string recordKey = ResumeHelper.GetDefaultRecordKey(localFile, saveKey);
                    recordFile = Path.Combine(UserEnv.GetHomeFolder(), recordKey);
                }

                if (upph == null)
                {
                    upph = new UploadProgressHandler(ResumableUploader.DefaultUploadProgressHandler);
                }

                if (upctl == null)
                {
                    upctl = new UploadController(ResumableUploader.DefaultUploadController);
                }

                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN, CHUNK_UNIT);
                result = ru.UploadFile(localFile, saveKey, token, recordFile, upph, upctl);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = fu.UploadFile(localFile, saveKey, token);
            }

            return result;
        }

#endif

#if Net45 || Net46 || NetCore

        /// <summary>
        /// [异步async]上传文件
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            var fi = new FileInfo(localFile);

            if (fi.Length > PUT_THRESHOLD)
            {
                if (recordFile == null)
                {
                    string recordKey = ResumeHelper.GetDefaultRecordKey(localFile, saveKey);
                    recordFile = Path.Combine(UserEnv.GetHomeFolder(), recordKey);
                }
                if (upph == null)
                {
                    upph = new UploadProgressHandler(ResumableUploader.DefaultUploadProgressHandler);
                }

                if (upctl == null)
                {
                    upctl = new UploadController(ResumableUploader.DefaultUploadController);
                }

                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN, CHUNK_UNIT);
                result = await ru.UploadFileAsync(localFile, saveKey, token, recordFile, upph, upctl);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = await fu.UploadFileAsync(localFile, saveKey, token);
            }

            return result;
        }

#endif

#if WINDOWS_UWP

        /// <summary>
        /// [异步async]上传文件
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(StorageFile localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();           

            var fi = await localFile.GetBasicPropertiesAsync();

            if (fi.Size > (ulong)PUT_THRESHOLD)
            {
                if (recordFile == null)
                {
                    string recordKey = ResumeHelper.GetDefaultRecordKey(localFile.Path, saveKey);
                    recordFile = await (await UserEnv.GetHomeFolderAsync()).CreateFileAsync(recordKey, CreationCollisionOption.OpenIfExists);
                }
                if (upph == null)
                {
                    upph = new UploadProgressHandler(ResumableUploader.DefaultUploadProgressHandler);
                }

                if (upctl == null)
                {
                    upctl = new UploadController(ResumableUploader.DefaultUploadController);
                }

                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN, CHUNK_UNIT);
                result = await ru.UploadFileAsync(localFile, saveKey, token, recordFile, upph, upctl);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = await fu.UploadFileAsync(localFile, saveKey, token);
            }

            return result;
        }


#endif

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public HttpResult UploadData(byte[] data, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (data.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = ru.UploadData(data, saveKey, token, null);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = fu.UploadData(data, saveKey, token);
            }

            return result;
        }

        /// <summary>
        /// 上传数据流，根据流长度以及设置的阈值(用户初始化UploadManager时可指定该值)自动选择表单或者分片上传
        /// </summary>
        /// <param name="stream">待上传的数据流，要求：流长度(Stream.Length)是确定的</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传数据后的返回结果</returns>
        public HttpResult UploadStream(Stream stream, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (stream.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = ru.UploadStream(stream, saveKey, token, null);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = fu.UploadStream(stream, saveKey, token);
            }

            return result;
        }

#endif

#if Net45 || Net46 || NetCore || WINDOWS_UWP

        /// <summary>
        /// [异步async]上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadDataAsync(byte[] data, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (data.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = await ru.UploadDataAsync(data, saveKey, token, upph);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = await fu.UploadDataAsync(data, saveKey, token);
            }

            return result;
        }

        /// <summary>
        /// [异步async]上传数据流，根据流长度以及设置的阈值(用户初始化UploadManager时可指定该值)自动选择表单或者分片上传
        /// </summary>
        /// <param name="stream">待上传的数据流，要求：流长度(Stream.Length)是确定的</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传数据后的返回结果</returns>
        public async Task<HttpResult> UploadStreamAsync(Stream stream, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            if (stream.Length > PUT_THRESHOLD)
            {
                ResumableUploader ru = new ResumableUploader(UPLOAD_FROM_CDN);
                result = await ru.UploadStreamAsync(stream, saveKey, token, sph);
            }
            else
            {
                FormUploader fu = new FormUploader(UPLOAD_FROM_CDN);
                result = await fu.UploadStreamAsync(stream, saveKey, token);
            }

            return result;
        }

#endif

    }
}
