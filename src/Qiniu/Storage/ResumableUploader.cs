using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Qiniu.Http;
using Qiniu.Util;

namespace Qiniu.Storage
{
    /// <summary>
    ///     分片上传/断点续上传，适合于以下"情形2~3":
    ///     (1)网络较好并且待上传的文件体积较小时(比如100MB或更小一点)使用简单上传;
    ///     (2)文件较大或者网络状况不理想时请使用分片上传;
    ///     (3)文件较大并且需要支持断点续上传，请使用分片上传(断点续上传)
    ///     上传时需要提供正确的上传凭证(由对应的上传策略生成)
    ///     上传策略 https://developer.qiniu.com/kodo/manual/1206/put-policy
    ///     上传凭证 https://developer.qiniu.com/kodo/manual/1208/upload-token
    /// </summary>
    public class ResumableUploader
    {
        //分片上传块的大小，固定为4M，不可修改
        private const int BlockSize = 4 * 1024 * 1024;
        private readonly Config _config;

        // HTTP请求管理器(GET/POST等)
        private readonly HttpManager _httpManager;

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="config">分片上传的配置信息</param>
        public ResumableUploader(Config config = null)
        {
            _config = config ?? new Config();

            _httpManager = new HttpManager();
        }

        /// <summary>
        ///     分片上传，支持断点续上传，带有自定义进度处理、高级控制功能
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="key">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="putExtra">上传可选配置</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFile(string localFile, string key, string token, PutExtra putExtra)
        {
            try
            {
                var fs = new FileStream(localFile, FileMode.Open);
                return await UploadStream(fs, key, token, putExtra);
            }
            catch (Exception ex)
            {
                var ret = HttpResult.InvalidFile;
                ret.RefText = ex.Message;
                return ret;
            }
        }

        /// <summary>
        ///     分片上传/断点续上传，带有自定义进度处理和上传控制，检查CRC32，可自动重试
        /// </summary>
        /// <param name="stream">待上传文件流</param>
        /// <param name="key">要保存的文件名称</param>
        /// <param name="upToken">上传凭证</param>
        /// <param name="putExtra">可选配置参数</param>
        /// <returns>上传文件后返回结果</returns>
        public async Task<HttpResult> UploadStream(Stream stream, string key, string upToken, PutExtra putExtra)
        {
            var result = new HttpResult();

            //check put extra
            if (putExtra == null)
            {
                putExtra = new PutExtra();
            }

            if (putExtra.ProgressHandler == null)
            {
                putExtra.ProgressHandler = DefaultUploadProgressHandler;
            }

            if (putExtra.UploadController == null)
            {
                putExtra.UploadController = DefaultUploadController;
            }

            if (!(putExtra.BlockUploadThreads > 0 && putExtra.BlockUploadThreads <= 64))
            {
                putExtra.BlockUploadThreads = 1;
            }

            using (stream)
            {
                //start to upload
                try
                {
                    long uploadedBytes = 0;
                    var fileSize = stream.Length;
                    var blockCount = (fileSize + BlockSize - 1) / BlockSize;

                    //check resume record file
                    ResumeInfo resumeInfo = null;
                    if (File.Exists(putExtra.ResumeRecordFile))
                    {
                        resumeInfo = ResumeHelper.Load(putExtra.ResumeRecordFile);
                        if (resumeInfo != null && fileSize == resumeInfo.FileSize)
                        {
                            //check whether ctx expired
                            if (UnixTimestamp.IsContextExpired(resumeInfo.ExpiredAt))
                            {
                                resumeInfo = null;
                            }
                        }
                    }

                    if (resumeInfo == null)
                    {
                        resumeInfo = new ResumeInfo
                        {
                            FileSize = fileSize,
                            BlockCount = blockCount,
                            Contexts = new string[blockCount],
                            ExpiredAt = 0
                        };
                    }

                    //calc upload progress
                    for (long blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        var context = resumeInfo.Contexts[blockIndex];
                        if (!string.IsNullOrEmpty(context))
                        {
                            uploadedBytes += BlockSize;
                        }
                    }

                    //set upload progress
                    putExtra.ProgressHandler(uploadedBytes, fileSize);

                    //init block upload error
                    //check not finished blocks to upload
                    var upCtrl = putExtra.UploadController();
                    var manualResetEvent = new ManualResetEvent(false);
                    var blockDataDict = new Dictionary<long, byte[]>();
                    var blockMakeResults = new Dictionary<long, HttpResult>();
                    var uploadedBytesDict = new Dictionary<string, long> { { "UploadProgress", uploadedBytes } };
                    var blockBuffer = new byte[BlockSize];
                    for (long blockIndex = 0; blockIndex < blockCount; blockIndex++)
                    {
                        var context = resumeInfo.Contexts[blockIndex];
                        if (string.IsNullOrEmpty(context))
                        {
                            //check upload controller action before each chunk
                            while (true)
                            {
                                upCtrl = putExtra.UploadController();

                                if (upCtrl == UploadControllerAction.Aborted)
                                {
                                    result.Code = (int)HttpCode.USER_CANCELED;
                                    result.RefCode = (int)HttpCode.USER_CANCELED;
                                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Info: upload task is aborted\n";
                                    manualResetEvent.Set();
                                    return result;
                                }

                                if (upCtrl == UploadControllerAction.Suspended)
                                {
                                    result.RefCode = (int)HttpCode.USER_PAUSED;
                                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Info: upload task is paused\n";
                                    manualResetEvent.WaitOne(1000);
                                }
                                else if (upCtrl == UploadControllerAction.Activated)
                                {
                                    break;
                                }
                            }

                            var offset = blockIndex * BlockSize;
                            stream.Seek(offset, SeekOrigin.Begin);
                            var blockLen = stream.Read(blockBuffer, 0, BlockSize);
                            var blockData = new byte[blockLen];
                            Array.Copy(blockBuffer, blockData, blockLen);
                            blockDataDict.Add(blockIndex, blockData);

                            if (blockDataDict.Count == putExtra.BlockUploadThreads)
                            {
                                ProcessMakeBlocks(blockDataDict, upToken, putExtra, resumeInfo, blockMakeResults, uploadedBytesDict, fileSize);
                                //check mkblk results
                                foreach (var blkIndex in blockMakeResults.Keys)
                                {
                                    var mkblkRet = blockMakeResults[blkIndex];
                                    if (mkblkRet.Code != 200)
                                    {
                                        result = mkblkRet;
                                        manualResetEvent.Set();
                                        return result;
                                    }
                                }

                                blockDataDict.Clear();
                                blockMakeResults.Clear();
                                if (!string.IsNullOrEmpty(putExtra.ResumeRecordFile))
                                {
                                    ResumeHelper.Save(resumeInfo, putExtra.ResumeRecordFile);
                                }
                            }
                        }
                    }

                    if (blockDataDict.Count > 0)
                    {
                        ProcessMakeBlocks(blockDataDict, upToken, putExtra, resumeInfo, blockMakeResults, uploadedBytesDict, fileSize);
                        //check mkblk results
                        foreach (var blkIndex in blockMakeResults.Keys)
                        {
                            var mkblkRet = blockMakeResults[blkIndex];
                            if (mkblkRet.Code != 200)
                            {
                                result = mkblkRet;
                                manualResetEvent.Set();
                                return result;
                            }
                        }

                        blockDataDict.Clear();
                        blockMakeResults.Clear();
                        if (!string.IsNullOrEmpty(putExtra.ResumeRecordFile))
                        {
                            ResumeHelper.Save(resumeInfo, putExtra.ResumeRecordFile);
                        }
                    }

                    if (upCtrl == UploadControllerAction.Activated)
                    {
                        var hr = await MakeFile(key, fileSize, key, upToken, putExtra, resumeInfo.Contexts);
                        if (hr.Code != (int)HttpCode.OK)
                        {
                            result.Shadow(hr);
                            result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Error: mkfile: code = {hr.Code}, text = {hr.Text}\n";
                        }

                        if (File.Exists(putExtra.ResumeRecordFile))
                        {
                            File.Delete(putExtra.ResumeRecordFile);
                        }

                        result.Shadow(hr);
                        result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Uploaded: \"{putExtra.ResumeRecordFile}\" ==> \"{key}\"\n";
                    }
                    else
                    {
                        result.Code = (int)HttpCode.USER_CANCELED;
                        result.RefCode = (int)HttpCode.USER_CANCELED;
                        result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Info: upload task is aborted, mkfile\n";
                    }

                    manualResetEvent.Set();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    var sb = new StringBuilder();
                    sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Error: ");
                    var e = ex;
                    while (e != null)
                    {
                        sb.Append(e.Message + " ");
                        e = e.InnerException;
                    }

                    sb.AppendLine();

                    result.RefCode = (int)HttpCode.USER_UNDEF;
                    result.RefText += sb.ToString();
                }
            }

            return result;
        }

        private void ProcessMakeBlocks(
            Dictionary<long, byte[]> blockDataDict,
            string upToken,
            PutExtra putExtra,
            ResumeInfo resumeInfo,
            Dictionary<long, HttpResult> blockMakeResults,
            Dictionary<string, long> uploadedBytesDict,
            long fileSize)
        {
            var taskMax = blockDataDict.Count;
            var doneEvents = new ManualResetEvent[taskMax];
            var eventIndex = 0;
            var progressLock = new object();

            var makeBlockTasks = blockDataDict.Keys.Select(
                blockIndex =>
                {
                    //signal task
                    var doneEvent = new ManualResetEvent(false);
                    doneEvents[eventIndex] = doneEvent;
                    eventIndex += 1;

                    //queue task
                    var blockData = blockDataDict[blockIndex];
                    var resumeBlocker = new ResumeBlocker(
                        doneEvent,
                        blockData,
                        blockIndex,
                        upToken,
                        putExtra,
                        resumeInfo,
                        blockMakeResults,
                        progressLock,
                        uploadedBytesDict,
                        fileSize);

                    return MakeBlock(resumeBlocker);
                }).ToArray();

            try
            {
                Task.WaitAll(makeBlockTasks);
                // ReSharper disable once CoVariantArrayConversion
                WaitHandle.WaitAll(doneEvents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("wait all exceptions:" + ex.StackTrace);
                //pass
            }
        }

        /// <summary>
        ///     创建块(携带首片数据),同时检查CRC32
        /// </summary>
        /// <param name="resumeBlockerObj">创建分片上次的块请求</param>
        private async Task MakeBlock(object resumeBlockerObj)
        {
            var resumeBlocker = (ResumeBlocker)resumeBlockerObj;
            var doneEvent = resumeBlocker.DoneEvent;
            var blockMakeResults = resumeBlocker.BlockMakeResults;
            var putExtra = resumeBlocker.PutExtra;
            var blockIndex = resumeBlocker.BlockIndex;
            var result = new HttpResult();
            //check whether to cancel
            while (true)
            {
                var upCtl = resumeBlocker.PutExtra.UploadController();
                if (upCtl == UploadControllerAction.Suspended)
                {
                    doneEvent.WaitOne(1000);
                }
                else if (upCtl == UploadControllerAction.Aborted)
                {
                    doneEvent.Set();

                    result.Code = (int)HttpCode.USER_CANCELED;
                    result.RefCode = (int)HttpCode.USER_CANCELED;
                    result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Info: upload task is aborted, mkblk {blockIndex}\n";
                    blockMakeResults.Add(blockIndex, result);
                    return;
                }
                else
                {
                    break;
                }
            }

            var blockBuffer = resumeBlocker.BlockBuffer;
            var blockSize = blockBuffer.Length;

            var upToken = resumeBlocker.UploadToken;
            var uploadedBytesDict = resumeBlocker.UploadedBytesDict;
            var fileSize = resumeBlocker.FileSize;
            var progressLock = resumeBlocker.ProgressLock;
            var resumeInfo = resumeBlocker.ResumeInfo;

            try
            {
                //get upload host
                var ak = UpToken.GetAccessKeyFromUpToken(upToken);
                var bucket = UpToken.GetBucketFromUpToken(upToken);
                if (ak == null || bucket == null)
                {
                    result = HttpResult.InvalidToken;
                    doneEvent.Set();
                    return;
                }

                var uploadHost = await _config.UpHost(ak, bucket);

                var url = $"{uploadHost}/mkblk/{blockSize}";
                var upTokenStr = $"UpToken {upToken}";
                using (var ms = new MemoryStream(blockBuffer, 0, blockSize))
                {
                    var data = ms.ToArray();

                    result = await _httpManager.PostDataAsync(url, data, token: upTokenStr);

                    if (result.Code == (int)HttpCode.OK)
                    {
                        var rc = JsonConvert.DeserializeObject<ResumeContext>(result.Text);

                        if (rc.Crc32 > 0)
                        {
                            var crc1 = rc.Crc32;
                            var crc2 = Crc32.CheckSumSlice(blockBuffer, 0, blockSize);
                            if (crc1 != crc2)
                            {
                                result.RefCode = (int)HttpCode.USER_NEED_RETRY;
                                result.RefText += $" CRC32: remote={crc1}, local={crc2}\n";
                            }
                            else
                            {
                                //write the mkblk context
                                resumeInfo.Contexts[blockIndex] = rc.Ctx;
                                resumeInfo.ExpiredAt = rc.ExpiredAt;
                                lock (progressLock)
                                {
                                    uploadedBytesDict["UploadProgress"] += blockSize;
                                }

                                putExtra.ProgressHandler(uploadedBytesDict["UploadProgress"], fileSize);
                            }
                        }
                        else
                        {
                            result.RefText += $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] JSON Decode Error: text = {result.Text}";
                            result.RefCode = (int)HttpCode.USER_NEED_RETRY;
                        }
                    }
                    else
                    {
                        result.RefCode = (int)HttpCode.USER_NEED_RETRY;
                    }
                }
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] mkblk Error: ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                if (ex is QiniuException qex)
                {
                    result.Code = qex.HttpResult.Code;
                    result.RefCode = qex.HttpResult.Code;
                    result.Text = qex.HttpResult.Text;
                    result.RefText += sb.ToString();
                }
                else
                {
                    result.RefCode = (int)HttpCode.USER_UNDEF;
                    result.RefText += sb.ToString();
                }
            }

            //return the http result
            blockMakeResults.Add(blockIndex, result);
            doneEvent.Set();
        }

        /// <summary>
        ///     根据已上传的所有分片数据创建文件
        /// </summary>
        /// <param name="fileName">源文件名</param>
        /// <param name="size">文件大小</param>
        /// <param name="key">要保存的文件名</param>
        /// <param name="contexts">所有数据块的Context</param>
        /// <param name="upToken">上传凭证</param>
        /// <param name="putExtra">用户指定的额外参数</param>
        /// <returns>此操作执行后的返回结果</returns>
        private async Task<HttpResult> MakeFile(string fileName, long size, string key, string upToken, PutExtra putExtra, string[] contexts)
        {
            var result = new HttpResult();

            try
            {
                var fnameStr = "fname";
                var mimeTypeStr = "";
                var keyStr = "";
                var paramStr = "";
                //check file name
                if (!string.IsNullOrEmpty(fileName))
                {
                    fnameStr = $"/fname/{Base64.UrlSafeBase64Encode(fileName)}";
                }

                //check mime type
                if (!string.IsNullOrEmpty(putExtra.MimeType))
                {
                    mimeTypeStr = $"/mimeType/{Base64.UrlSafeBase64Encode(putExtra.MimeType)}";
                }

                //check key
                if (!string.IsNullOrEmpty(key))
                {
                    keyStr = $"/key/{Base64.UrlSafeBase64Encode(key)}";
                }

                //check extra params
                if (putExtra.Params != null && putExtra.Params.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var kvp in putExtra.Params)
                    {
                        var k = kvp.Key;
                        var v = kvp.Value;
                        if (k.StartsWith("x:") && !string.IsNullOrEmpty(v))
                        {
                            sb.Append($"/{k}/{v}");
                        }
                    }

                    paramStr = sb.ToString();
                }

                //get upload host
                var ak = UpToken.GetAccessKeyFromUpToken(upToken);
                var bucket = UpToken.GetBucketFromUpToken(upToken);
                if (ak == null || bucket == null)
                {
                    return HttpResult.InvalidToken;
                }

                var uploadHost = await _config.UpHost(ak, bucket);

                var url = $"{uploadHost}/mkfile/{size}{mimeTypeStr}{fnameStr}{keyStr}{paramStr}";
                var body = string.Join(",", contexts);
                var upTokenStr = $"UpToken {upToken}";

                result = await _httpManager.PostTextAsync(url, body, upTokenStr);
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();
                sb.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] mkfile Error: ");
                var e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendLine();

                if (ex is QiniuException qex)
                {
                    result.Code = qex.HttpResult.Code;
                    result.RefCode = qex.HttpResult.Code;
                    result.Text = qex.HttpResult.Text;
                    result.RefText += sb.ToString();
                }
                else
                {
                    result.RefCode = (int)HttpCode.USER_UNDEF;
                    result.RefText += sb.ToString();
                }
            }

            return result;
        }


        /// <summary>
        ///     默认的进度处理函数-上传文件
        /// </summary>
        /// <param name="uploadedBytes">已上传的字节数</param>
        /// <param name="totalBytes">文件总字节数</param>
        public static void DefaultUploadProgressHandler(long uploadedBytes, long totalBytes)
        {
            if (uploadedBytes < totalBytes)
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Progress: {100.0 * uploadedBytes / totalBytes,7:0.000}%");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [ResumableUpload] Progress: {100.0,7:0.000}%\n");
            }
        }

        /// <summary>
        ///     默认的上传控制函数，默认不执行任何控制
        /// </summary>
        /// <returns>控制状态</returns>
        public static UploadControllerAction DefaultUploadController()
        {
            return UploadControllerAction.Activated;
        }
    }
}
