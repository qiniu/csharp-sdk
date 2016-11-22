using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.Storage.Persistent;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Qiniu.Storage
{
    /// <summary>
    /// 文件分片上传
    /// </summary>
    public class ResumeUploader
    {
        private HttpManager mHttpManager;
        private UploadOptions uploadOptions;
        private UpCompletionHandler upCompletionHandler;
        private string key;
        private long size;
        private string[] contexts;
        private byte[] chunkBuffer;
        private ResumeRecorder resumeRecorder;
        private string recordKey;
        private long lastModifyTime;
        private string filePath;
        private long crc32;
        private Stream fileStream;
        private Dictionary<string, string> upHeaders;

        /// <summary>
        /// 构建分片上传对象
        /// </summary>
        /// <param name="recorder">分片上传进度记录器</param>
        /// <param name="recordKey">分片上传进度记录文件名</param>
        /// <param name="filePath">上传的文件全路径</param>
        /// <param name="key">保存在七牛的文件名</param>
        /// <param name="token">上传凭证</param>
        /// <param name="uploadOptions">上传可选设置</param>
        /// <param name="upCompletionHandler">上传完成结果处理器</param>
        public ResumeUploader(ResumeRecorder recorder, string recordKey, string filePath,
            string key, string token, UploadOptions uploadOptions, UpCompletionHandler upCompletionHandler)
        {
            this.mHttpManager = new HttpManager();
            this.resumeRecorder = recorder;
            this.recordKey = recordKey;
            this.filePath = filePath;
            this.key = key;
            this.uploadOptions = (uploadOptions == null) ? UploadOptions.defaultOptions() : uploadOptions;
            this.upCompletionHandler = new UpCompletionHandler(delegate(string fileKey, ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    this.uploadOptions.ProgressHandler(key, 1.0);
                }

                if (this.fileStream != null)
                {
                    try
                    {
                        this.fileStream.Close();
                        this.fileStream = null;
                    }
                    catch (Exception) { }
                }

                try
                {
                    if (upCompletionHandler != null)
                    {
                        upCompletionHandler(key, respInfo, response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("resumable upload completion error, {0}", ex.Message);
                }
            });
            string upTokenHeader = string.Format("UpToken {0}", token);
            this.upHeaders = new Dictionary<string, string>();
            this.upHeaders.Add("Authorization", upTokenHeader);
            this.chunkBuffer = new byte[Config.CHUNK_SIZE];
        }

        public ResumeUploader(ResumeRecorder recorder, string recordKey, Stream stream,
            string key, string token, UploadOptions uploadOptions, UpCompletionHandler upCompletionHandler)
        {
            this.mHttpManager =new HttpManager() ;
            this.resumeRecorder = recorder;
            this.recordKey = recordKey;
            this.fileStream = stream;
            this.key = key;
            this.uploadOptions = (uploadOptions == null) ? UploadOptions.defaultOptions() : uploadOptions;
            this.upCompletionHandler = new UpCompletionHandler(delegate(string fileKey, ResponseInfo respInfo, string response)
            {
                if (respInfo.isOk())
                {
                    this.uploadOptions.ProgressHandler(key, 1.0);
                }

                if (this.fileStream != null)
                {
                    try
                    {
                        this.fileStream.Close();
                        this.fileStream = null;
                    }
                    catch (Exception) { }
                }

                try
                {
                    if (upCompletionHandler != null)
                    {
                        upCompletionHandler(key, respInfo, response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("resumable upload completion error, {0}", ex.Message);
                }
            });
            string upTokenHeader = string.Format("UpToken {0}", token);
            this.upHeaders = new Dictionary<string, string>();
            this.upHeaders.Add("Authorization", upTokenHeader);
            this.chunkBuffer = new byte[Config.CHUNK_SIZE];
        }

        ~ResumeUploader()
        {
            this.chunkBuffer = null;
            this.contexts = null;
            this.fileStream = null;
            this.upHeaders = null;
        }

        #region 发送mkblk请求
        private void makeBlock(string upHost, long offset, int blockSize, int chunkSize,
            ProgressHandler progressHandler, CompletionHandler completionHandler)
        {
            string url = string.Format("{0}/mkblk/{1}", upHost, blockSize);
            try
            {
                this.fileStream.Read(this.chunkBuffer, 0, chunkSize);
            }
            catch (Exception ex)
            {
                this.upCompletionHandler(this.key, ResponseInfo.fileError(ex), "");
                return;
            }
            this.crc32 = CRC32.CheckSumSlice(this.chunkBuffer, 0, chunkSize);
            this.mHttpManager.postData(url, this.upHeaders, this.chunkBuffer, 0, chunkSize,
                HttpManager.FORM_MIME_OCTECT, new CompletionHandler(delegate (ResponseInfo respInfo, string response)
                {
                    progressHandler(offset, this.size);
                    completionHandler(respInfo, response);
                }));
        }
        #endregion

        #region 发送bput请求
        private void putChunk(string upHost, long offset, int chunkSize, string context,
            ProgressHandler progressHandler, CompletionHandler completionHandler)
        {
            int chunkOffset = (int)(offset % Config.BLOCK_SIZE);
            string url = string.Format("{0}/bput/{1}/{2}", upHost, context, chunkOffset);
            try
            {
                this.fileStream.Read(this.chunkBuffer, 0, chunkSize);
            }
            catch (Exception ex)
            {
                this.upCompletionHandler(this.key, ResponseInfo.fileError(ex), "");
                return;
            }
            this.crc32 = CRC32.CheckSumSlice(this.chunkBuffer, 0, chunkSize);
            this.mHttpManager.postData(url, this.upHeaders, this.chunkBuffer, 0, chunkSize, 
                HttpManager.FORM_MIME_OCTECT, new CompletionHandler(delegate(ResponseInfo respInfo,string response)
                {
                    progressHandler(offset, this.size);
                    completionHandler(respInfo, response);
                }));
        }
        #endregion

        #region 发送mkfile请求
        private void makeFile(string upHost, CompletionHandler completionHandler)
        {

            string fname = this.key;
            if (!string.IsNullOrEmpty(this.filePath))
            {
                fname = Path.GetFileName(this.filePath);
            }

            string fnameStr = "";
            if (!string.IsNullOrEmpty(fname))
            {
                fnameStr = string.Format("/fname/{0}", StringUtils.urlSafeBase64Encode(fname));
            }

            string mimeTypeStr = string.Format("/mimeType/{0}", StringUtils.urlSafeBase64Encode(this.uploadOptions.MimeType));

            string keyStr = "";
            if (this.key != null)
            {
                keyStr = string.Format("/key/{0}", StringUtils.urlSafeBase64Encode(this.key));
            }

            string paramsStr = "";
            if (this.uploadOptions.ExtraParams.Count > 0)
            {
                string[] paramArray = new string[this.uploadOptions.ExtraParams.Count];
                int j = 0;
                foreach (KeyValuePair<string, string> kvp in this.uploadOptions.ExtraParams)
                {
                    paramArray[j++] = string.Format("{0}/{1}", kvp.Key, StringUtils.urlSafeBase64Encode(kvp.Value));
                }
                paramsStr = "/" + StringUtils.join(paramArray, "/");
            }

            string url = string.Format("{0}/mkfile/{1}{2}{3}{4}{5}", upHost, this.size, mimeTypeStr, fnameStr, keyStr, paramsStr);
            string postBody = StringUtils.join(this.contexts, ",");
            byte[] postBodyData = Encoding.UTF8.GetBytes(postBody);
            this.mHttpManager.postData(url, upHeaders, postBodyData, HttpManager.FORM_MIME_URLENCODED, completionHandler);
        }
        #endregion

        /// <summary>
        /// 分片方式上传文件
        /// </summary>
        #region 上传文件
        public void uploadFile()
        {
            try
            {
                this.fileStream = File.Open(this.filePath, FileMode.Open, FileAccess.Read);
                this.lastModifyTime = File.GetLastWriteTime(this.filePath).ToFileTime();
                this.size = this.fileStream.Length;
                long blockCount = (this.size % Config.BLOCK_SIZE == 0) ? (this.size / Config.BLOCK_SIZE) : (this.size / Config.BLOCK_SIZE + 1);
                this.contexts = new string[blockCount];
            }
            catch (Exception ex)
            {
                this.upCompletionHandler(this.key, ResponseInfo.fileError(ex), "");
                return;
            }

            long offset = recoveryFromResumeRecord();
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.nextTask(offset, 0, Config.ZONE.UpHost);
        }
        #endregion

        /// <summary>
        /// 分片方式上传文件流
        /// </summary>
        #region 上传文件流
        public void uploadStream()
        {
            try
            {
                this.lastModifyTime = DateTime.Now.ToFileTime();
                this.size = this.fileStream.Length;
                long blockCount = (this.size % Config.BLOCK_SIZE == 0) ? (this.size / Config.BLOCK_SIZE) : (this.size / Config.BLOCK_SIZE + 1);
                this.contexts = new string[blockCount];
            }
            catch (Exception ex)
            {
                this.upCompletionHandler(this.key, ResponseInfo.fileError(ex), "");
                return;
            }

            long offset = recoveryFromResumeRecord();
            this.fileStream.Seek(offset, SeekOrigin.Begin);
            this.nextTask(offset, 0, Config.ZONE.UpHost);
        }
        #endregion

        #region 从中断日志中读取上传进度
        private long recoveryFromResumeRecord()
        {
            long offset = 0;
            if (this.resumeRecorder != null && this.recordKey != null)
            {
                byte[] data = this.resumeRecorder.get(this.recordKey);
                if (data != null)
                {
                    ResumeRecord r = ResumeRecord.fromJsonData(Encoding.UTF8.GetString(data, 0, data.Length));
                    offset = r.Offset;
                    for (int i = 0; i < r.Contexts.Length; i++)
                    {
                        this.contexts[i] = r.Contexts[i];
                    }
                }
            }
            return offset;
        }
        #endregion

        #region 记录/更新上传进度信息
        private void record(long offset)
        {
            if (this.resumeRecorder == null || offset == 0)
            {
                return;
            }
            ResumeRecord r = new ResumeRecord(this.size, offset, this.lastModifyTime, this.contexts);
            this.resumeRecorder.set(this.recordKey, Encoding.UTF8.GetBytes(r.toJsonData()));
        }
        #endregion

        #region 删除上传进度信息
        private void removeRecord()
        {
            if (this.resumeRecorder != null && this.recordKey != null)
            {
                this.resumeRecorder.del(this.recordKey);
            }
        }
        #endregion

        #region 判断上传是否被取消
        private bool isCancelled()
        {
            return this.uploadOptions.CancellationSignal();
        }
        #endregion

        #region 计算每次上传的分片大小
        private int calcBPutChunkSize(long offset)
        {
            int chunkSize = Config.CHUNK_SIZE;
            long defaultChunkSize = Config.CHUNK_SIZE;
            long left = this.size - offset;
            if (left < defaultChunkSize)
            {
                chunkSize = (int)left;
            }
            return chunkSize;
        }
        #endregion

        #region 计算每次创建的块大小
        private int calcMakeBlockSize(long offset)
        {
            int blockSize = Config.BLOCK_SIZE;
            long defaultBlockSize = Config.BLOCK_SIZE;

            long left = this.size - offset;
            if (left < defaultBlockSize)
            {
                blockSize = (int)left;
            }
            return blockSize;
        }
        #endregion

        #region 文件上传任务
        private void nextTask(long offset, int retried, string upHost)
        {
            //上传中途触发停止
            if (this.isCancelled())
            {
                this.upCompletionHandler(this.key, ResponseInfo.cancelled(), null);
                return;
            }
            //所有分片已上传
            if (offset == this.size)
            {
                this.makeFile(upHost, new CompletionHandler(delegate(ResponseInfo respInfo, string response)
                {
                    //makeFile成功
                    if (respInfo.isOk())
                    {
                        removeRecord();
                        Console.WriteLine("mkfile ok, upload done!");
                        this.upCompletionHandler(this.key, respInfo, response);
                        return;
                    }

                    //失败重试，如果614，则不重试
                    if (respInfo.StatusCode != 614)
                    {
                        if (respInfo.needRetry() && retried < Config.RETRY_MAX)
                        {
                            Console.WriteLine("mkfile retrying due to {0}...", respInfo.StatusCode);
                            string upHost2 = Config.ZONE.UploadHost;
                            nextTask(offset, retried + 1, upHost2);
                            return;
                        }
                    }

                    Console.WriteLine("mkfile error, upload failed due to {0}",respInfo.StatusCode);
                    this.upCompletionHandler(key, respInfo, response);
                }));
                return;
            }

            //创建块或上传分片
            int chunkSize = calcBPutChunkSize(offset);
            ProgressHandler progressHandler = new ProgressHandler(delegate(long bytesWritten, long totalBytes)
            {
                double percent = (double)(offset) / this.size;
                Console.WriteLine("resumable upload progress {0}", percent);
                if (percent > 0.95)
                {
                    percent = 0.95;
                }
                this.uploadOptions.ProgressHandler(this.key, percent);
            });

            CompletionHandler completionHandler = new CompletionHandler(delegate(ResponseInfo respInfo, string response)
            {
                if (offset % Config.BLOCK_SIZE == 0)
                {
                    Console.WriteLine("mkblk result {0}, offset {1}", respInfo.StatusCode, offset);
                }else
                {
                    Console.WriteLine("bput result {0}, offset {1}", respInfo.StatusCode, offset);
                }
                if (!respInfo.isOk())
                {
                    //如果是701错误，为mkblk的ctx过期
                    if (respInfo.StatusCode == 701)
                    {
                        nextTask((offset / Config.BLOCK_SIZE) * Config.BLOCK_SIZE, retried, upHost);
                        return;
                    }

                    if (retried >= Config.RETRY_MAX || !respInfo.needRetry())
                    {
                        this.upCompletionHandler(key, respInfo, response);
                        return;
                    }

                    String upHost2 = upHost;
                    if (respInfo.needRetry())
                    {
                        upHost2 = Config.ZONE.UploadHost;
                    }
                    nextTask(offset, retried + 1, upHost2);
                    return;
                }

                //请求成功
                string chunkContext = null;
                if (response == null || string.IsNullOrEmpty(response))
                {
                    nextTask(offset, retried + 1, upHost);
                    return;
                }

                long chunkCrc32 = 0;
                Dictionary<string, string> respDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                if (respDict.ContainsKey("ctx"))
                {
                    chunkContext = respDict["ctx"];
                }
                if (respDict.ContainsKey("crc32"))
                {
                    chunkCrc32 = Convert.ToInt64(respDict["crc32"]);
                }

                if (chunkContext == null || chunkCrc32 != this.crc32)
                {
                    nextTask(offset, retried + 1, upHost);
                    return;
                }

                this.contexts[offset / Config.BLOCK_SIZE] = chunkContext;
                record(offset + chunkSize);
                nextTask(offset + chunkSize, retried, upHost);
            });

            //创建块
            if (offset % Config.BLOCK_SIZE == 0)
            {
                int blockSize = calcMakeBlockSize(offset);
                this.makeBlock(upHost, offset, blockSize, chunkSize, progressHandler, completionHandler);
                return;
            }

            //上传分片
            string context = this.contexts[offset / Config.BLOCK_SIZE];
            this.putChunk(upHost, offset, chunkSize, context, progressHandler, completionHandler);
        }
        #endregion
    }
}