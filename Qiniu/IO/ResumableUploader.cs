using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Qiniu.Common;
using Qiniu.IO.Model;
using Qiniu.Util;
using Qiniu.Http;
using Newtonsoft.Json;

namespace Qiniu.IO
{
    /// <summary>
    /// 分片上传/断点续上传，适合于以下情形(2)(3)
    /// (1)网络较好并且待上传的文件体积较小时使用简单上传
    /// (2)文件较大或者网络状况不理想时请使用分片上传
    /// (3)文件较大上传需要花费较常时间，建议使用断点续上传
    /// </summary>
    public class ResumableUploader
    {
        //分片上传块的大小，固定为4M，不可修改
        private const int BLOCK_SIZE = 4 * 1024 * 1024;

        //分片上传切片大小(要求:一个块能够恰好被分割为若干片)，可设置
        private int CHUNK_SIZE = 2 * 1024 * 1024;

        // HTTP CLIENT
        private HttpClient client;

        public ResumableUploader()
        {
            client = new HttpClient();
        }

        /// <summary>
        /// 分片上传/断点续上传
        /// 使用默认记录文件(recordFile)和默认进度处理(progressHandler)
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token)
        {
            string ruFile = "QiniuRU_" + StringHelper.CalcMD5(localFile + saveKey);
            string recordFile = Path.Combine(UserEnv.HomeFolder(), ruFile);
            UploadProgressHandler uppHandler = new UploadProgressHandler(DefaultUploadProgressHandler);
            return UploadFile(localFile, saveKey, token, recordFile, uppHandler);
        }

        /// <summary>
        /// 分片上传/断点续上传
        /// 使用默认进度处理(progressHandler)
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="recordFile">记录文件(记录断点信息)</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile)
        {
            UploadProgressHandler uppHandler = new UploadProgressHandler(DefaultUploadProgressHandler);
            return UploadFile(localFile, saveKey, token, recordFile, uppHandler);
        }

        /// <summary>
        /// 分片上传/断点续上传
        /// </summary>
        /// <param name="localFile">本地待上传的文件名</param>
        /// <param name="saveKey">要保存的文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="recordFile">记录文件(记录断点信息)</param>
        /// <param name="uppHandler">上传进度处理</param>
        /// <returns></returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token, string recordFile, UploadProgressHandler uppHandler)
        {
            HttpResult result = new HttpResult();

            FileStream fs = null;
            try
            {
                fs = new FileStream(localFile, FileMode.Open);

                long fileSize = fs.Length;
                long chunkSize = CHUNK_SIZE;
                long blockSize = BLOCK_SIZE;
                byte[] chunkBuffer = new byte[chunkSize];
                int blockCount = (int)((fileSize + blockSize - 1) / blockSize);

                ResumeInfo resumeInfo = ResumeHelper.Load(recordFile);
                if (resumeInfo == null)
                {
                    resumeInfo = new ResumeInfo()
                    {
                        FileSize = fileSize,
                        BlockIndex = 0,
                        BlockCount = blockCount,
                        Contexts = new string[blockCount]
                    };

                    ResumeHelper.Save(resumeInfo, recordFile);
                }

                int index = resumeInfo.BlockIndex;
                long offset = index*blockSize;
                string context = null;
                long leftBytes = fileSize - offset;
                long blockLeft = 0;
                long blockOffset = 0;
                HttpResult hr = null;
                ResumeContext rc = null;

                fs.Seek(offset, SeekOrigin.Begin);

                while (leftBytes > 0)
                {
                    #region one_block

                    #region mkblk
                    if (leftBytes < BLOCK_SIZE)
                    {
                        blockSize = leftBytes;
                    }
                    else
                    {
                        blockSize = BLOCK_SIZE;
                    }
                    if (leftBytes < CHUNK_SIZE)
                    {
                        chunkSize = leftBytes;
                    }
                    else
                    {
                        chunkSize = CHUNK_SIZE;
                    }

                    fs.Read(chunkBuffer, 0, (int)chunkSize);
                    hr = mkblk(chunkBuffer, offset, blockSize, chunkSize, token);
                    if (hr.StatusCode != 200)
                    {
                        result.StatusCode = hr.StatusCode;
                        throw new Exception(hr.Message);
                    }

                    rc = JsonConvert.DeserializeObject<ResumeContext>(hr.Message);
                    context = rc.Ctx;
                    offset += chunkSize;
                    leftBytes -= chunkSize;
                    #endregion mkblk

                    uppHandler(offset, fileSize);

                    if (leftBytes > 0)
                    {
                        blockLeft = blockSize - chunkSize;
                        blockOffset = chunkSize;
                        while (blockLeft > 0)
                        {
                            #region bput-loop

                            if (blockLeft < CHUNK_SIZE)
                            {
                                chunkSize = blockLeft;
                            }
                            else
                            {
                                chunkSize = CHUNK_SIZE;
                            }

                            fs.Read(chunkBuffer, 0, (int)chunkSize);
                            hr = bput(chunkBuffer, blockOffset, chunkSize, context, token);
                            if (hr.StatusCode != 200)
                            {
                                result.StatusCode = hr.StatusCode;
                                throw new Exception(hr.Message);
                            }

                            rc = JsonConvert.DeserializeObject<ResumeContext>(hr.Message);
                            context = rc.Ctx;

                            offset += chunkSize;
                            leftBytes -= chunkSize;
                            blockOffset += chunkSize;
                            blockLeft -= chunkSize;
                            #endregion bput-loop

                            uppHandler(offset, fileSize);
                        }
                    }

                    #endregion one_block

                    resumeInfo.BlockIndex = index;
                    resumeInfo.Contexts[index] = context;
                    ResumeHelper.Save(resumeInfo, recordFile);
                    ++index;
                }

                var mkfileResult = mkfile(localFile, fileSize, saveKey, "application/octet-stream", resumeInfo.Contexts, token);
                if (mkfileResult.StatusCode != 200)
                {
                    result.StatusCode = mkfileResult.StatusCode;
                    throw new Exception(mkfileResult.Message);
                }

                File.Delete(recordFile);
                result.StatusCode = 200;
                result.Message = string.Format("[ResumableUpload] Uploaded: \"{0}\" ==> \"{1}\"", localFile, saveKey);
            }
            catch (Exception ex)
            {
                result.Message = "[ResumableUpload] Error: "+ex.Message;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// 默认的进度处理函数
        /// </summary>
        /// <param name="uploadedBytes">已上传的字节数</param>
        /// <param name="totalBytes">文件总字节数</param>
        public void DefaultUploadProgressHandler(long uploadedBytes, long totalBytes)
        {
            if (uploadedBytes < totalBytes)
            {
                Console.WriteLine("[ResumableUpload] Progress: {0,7:0.000}%", 100.0 * uploadedBytes / totalBytes);
            }
            else
            {
                Console.WriteLine("[ResumableUpload] Progress: finished");
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        private HttpResult mkfile(string fileName, long size, string saveKey,string mimeType,string[] contexts,string token)
        {
            HttpResult result = new HttpResult();

            string fnameStr= string.Format("/fname/{0}", StringHelper.UrlSafeBase64Encode(fileName));
            string mimeTypeStr = string.Format("/mimeType/{0}", StringHelper.UrlSafeBase64Encode(mimeType));
            string keyStr = string.Format("/key/{0}", StringHelper.UrlSafeBase64Encode(saveKey));

            string url = string.Format("{0}/mkfile/{1}{2}{3}{4}",Config.ZONE.UpHost, size, mimeTypeStr, fnameStr, keyStr);
            string body = StringHelper.Join(contexts, ",");

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Add("Authorization", string.Format("UpToken {0}", token));
            req.Content = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
            req.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            try
            {
                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch(Exception ex)
            {
                result.Message = "[mkfile] Error:" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 创建块(携带首片数据)
        /// </summary>
        private HttpResult mkblk(byte[] chunkBuffer, long offset, long blockSize, long chunkSize, string token)
        {
            HttpResult result = new HttpResult();

            string url = string.Format("{0}/mkblk/{1}", Config.ZONE.UpHost, blockSize);

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("Authorization", string.Format("UpToken {0}", token));
                req.Content = new ByteArrayContent(chunkBuffer, 0, (int)chunkSize);
                req.Content.Headers.Add("Content-Type", "application/octet-stream");

                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;

                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[mkblk] Error:" + ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 上传数据片
        /// </summary>
        private HttpResult bput(byte[] chunkBuffer,long offset, long chunkSize, string context, string token)
        {
            HttpResult result = new HttpResult();

            string url  = string.Format("{0}/bput/{1}/{2}", Config.ZONE.UpHost, context, offset);

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("Authorization", string.Format("UpToken {0}", token));
                req.Content = new ByteArrayContent(chunkBuffer, 0, (int)chunkSize);
                req.Content.Headers.Add("Content-Type", "application/octet-stream");

                var msg = client.SendAsync(req);
                result.StatusCode = (int)msg.Result.StatusCode;
                
                var ret = msg.Result.Content.ReadAsStringAsync();
                result.Message = ret.Result;
            }
            catch (Exception ex)
            {
                result.Message = "[bput] Error:" + ex.Message;
            }

            return result;
        }

    }
}
