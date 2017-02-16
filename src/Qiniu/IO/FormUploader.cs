using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Qiniu.Common;
using Qiniu.Http;

#if Net45 || Net46 || NetCore
using System.Threading.Tasks;
#endif

#if WINDOWS_UWP
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

namespace Qiniu.IO
{
    /// <summary>
    /// 简单上传，适合于以下"情形1":  
    /// (1)网络较好并且待上传的文件体积较小时(比如100MB或更小一点)使用简单上传;
    /// (2)文件较大或者网络状况不理想时请使用分片上传;
    /// (3)文件较大并且需要支持断点续上传，请使用分片上传(断点续上传)
    /// 上传时需要提供正确的上传凭证(由对应的上传策略生成)
    /// 上传策略 http://developer.qiniu.com/article/developer/security/upload-token.html
    /// 上传凭证 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    public class FormUploader
    {
        private string uploadHost;

        private HttpManager httpManager;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="uploadFromCDN">是否从CDN上传(默认否)，使用CDN上传可能会有更好的效果</param>
        public FormUploader(bool uploadFromCDN = false)
        {
            httpManager = new HttpManager();

            uploadHost = uploadFromCDN ? Config.ZONE.UploadHost : Config.ZONE.UpHost;
        }

#if Net20 || Net35 || Net40 || Net45 || Net46 || NetCore

        #region NET-UPLOAD-NORMAL

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine("--" + boundary + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = File.ReadAllBytes(localFile);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                ms.Write(partData1, 0, partData1.Length);
                ms.Write(partData2, 0, partData2.Length);
                ms.Write(partData3, 0, partData3.Length);

                result = httpManager.PostMultipart(uploadHost, ms.ToArray(), boundary, null);

                if (result.Code == (int)HttpCode.OK)
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
                }
                else
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), result.Code, result.Text);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 上传文件 - 可附加自定义参数
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extraParams">用户自定义的附加参数</param>
        /// <returns>上传文件后的返回结果</returns>
        public HttpResult UploadFile(string localFile, string saveKey, string token, Dictionary<string, string> extraParams)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine("--" + boundary);

                foreach (var d in extraParams)
                {
                    sbp1.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", d.Key);
                    sbp1.AppendLine();
                    sbp1.AppendLine();
                    sbp1.AppendLine(d.Value);
                    sbp1.AppendLine("--" + boundary);
                }

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine("--" + boundary + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = File.ReadAllBytes(localFile);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                ms.Write(partData1, 0, partData1.Length);
                ms.Write(partData2, 0, partData2.Length);
                ms.Write(partData3, 0, partData3.Length);

                result = httpManager.PostMultipart(uploadHost, ms.ToArray(), boundary, null);
                if (result.Code == (int)HttpCode.OK)
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
                }
                else
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), result.Code, result.Text);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 读取文件内容到byte数组中
        /// </summary>
        /// <param name="file"></param>
        /// <returns>存放文件内容的字节数组</returns>
        public static byte[] ReadToByteArray(string file)
        {
            byte[] bytes = null;
            using (var stream = new FileStream(file, FileMode.Open))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }
            return bytes;
        }

        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="saveKey">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传数据后的返回结果</returns>
        public HttpResult UploadData(byte[] data, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine("--" + boundary + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = data;
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                ms.Write(partData1, 0, partData1.Length);
                ms.Write(partData2, 0, partData2.Length);
                ms.Write(partData3, 0, partData3.Length);

                result = httpManager.PostMultipart(uploadHost, ms.ToArray(), boundary, null);
                if (result.Code == (int)HttpCode.OK)
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Uploaded: #DATA# ==> \"{1}\"\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
                }
                else
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), result.Code, result.Text);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 上传数据流
        /// </summary>
        /// <param name="stream">(确定长度的)数据流</param>
        /// <param name="saveKey">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传数据流后的返回结果</returns>
        public HttpResult UploadStream(Stream stream, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine("--" + boundary);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                int bufferSize = 1024 * 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                MemoryStream dataMS = new MemoryStream();
                while (true)
                {
                    bytesRead = stream.Read(buffer, 0, bufferSize);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    dataMS.Write(buffer, 0, bytesRead);
                }

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine("--" + boundary + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = dataMS.ToArray();
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                ms.Write(partData1, 0, partData1.Length);
                ms.Write(partData2, 0, partData2.Length);
                ms.Write(partData3, 0, partData3.Length);

                result = httpManager.PostMultipart(uploadHost, ms.ToArray(), boundary, null);
                if (result.Code == (int)HttpCode.OK)
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Uploaded: #STREAM# ==> \"{1}\"\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
                }
                else
                {
                    result.RefText += string.Format("[{0}] [FormUpload] Failed: code = {1}, text = {2}\n",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), result.Code, result.Text);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        #endregion NET-UPLOAD-NORMAL

#endif

#if Net45 || Net46 || NetCore

        #region NET-UPLOAD-ASYNC

        /// <summary>
        /// [异步async]上传文件
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(string localFile, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = await ReadToByteArrayAsync(localFile);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();               

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]上传文件 - 可附加自定义参数
        /// </summary>
        /// <param name="localFile">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extraParams">用户自定义的附加参数</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(string localFile, string saveKey, string token, Dictionary<string, string> extraParams)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;
                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                foreach (var d in extraParams)
                {
                    sbp1.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", d.Key);
                    sbp1.AppendLine();
                    sbp1.AppendLine();
                    sbp1.AppendLine(d.Value);
                    sbp1.AppendLine(sep);
                }

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = await ReadToByteArrayAsync(localFile);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());



                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), localFile, saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        #endregion NET-UPLOAD-ASYNC

        /// <summary>
        /// [异步async]读取文件内容到byte数组中
        /// </summary>
        /// <param name="file"></param>
        /// <returns>存放文件内容的字节数组</returns>
        public static async Task<byte[]> ReadToByteArrayAsync(string file)
        {
            byte[] bytes = null;
            using (var stream = new FileStream(file, FileMode.Open))
            {
                bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
            }
            return bytes;
        }

#endif

#if WINDOWS_UWP

        #region UWP-UPLOAD-ASYNC

        /// <summary>
        /// [异步async]上传文件
        /// </summary>
        /// <param name="file">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(StorageFile file, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = await ReadToByteArrayAsync(file);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), file, saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]上传文件 - 可附加自定义参数
        /// </summary>
        /// <param name="file">待上传的本地文件</param>
        /// <param name="saveKey">要保存的目标文件名称</param>
        /// <param name="token">上传凭证</param>
        /// <param name="extraParams">用户自定义的附加参数</param>
        /// <returns>上传文件后的返回结果</returns>
        public async Task<HttpResult> UploadFileAsync(StorageFile file, string saveKey, string token, Dictionary<string, string> extraParams)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;
                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                foreach (var d in extraParams)
                {
                    sbp1.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", d.Key);
                    sbp1.AppendLine();
                    sbp1.AppendLine();
                    sbp1.AppendLine(d.Value);
                    sbp1.AppendLine(sep);
                }

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = await ReadToByteArrayAsync(file);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());



                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"{1}\" ==> \"{2}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), file, saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]将文件(StorageFile)内容读取到字节数组中
        /// </summary>
        /// <param name="file">文件StorageFile</param>
        /// <returns>存放文件按内容的字节数组</returns>
        public static async Task<byte[]> ReadToByteArrayAsync(StorageFile file)
        {
            byte[] bytes = null;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                bytes = new byte[stream.Length];
                using (var dataReader = new DataReader(stream.AsInputStream()))
                {
                    await dataReader.LoadAsync((uint)stream.Length);
                    dataReader.ReadBytes(bytes);
                }
            }
            return bytes;
        }

        #endregion UWP-UPLOAD-ASYNC

#endif

#if Net45 || Net46 || NetCore || WINDOWS_UWP

        /// <summary>
        /// [异步async]上传字节数据
        /// </summary>
        /// <param name="data">待上传的数据</param>
        /// <param name="saveKey">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <returns>上传数据后的返回结果</returns>
        public async Task<HttpResult> UploadDataAsync(byte[] data, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;

                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = data;
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"#DATA#\" ==> \"{1}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// [异步async]上传数据流
        /// </summary>
        /// <param name="stream">数据流，流长度必须可确定</param>
        /// <param name="saveKey">要保存的key</param>
        /// <param name="token">上传凭证</param>
        /// <returns>数据流上传后的返回结果</returns>
        public async Task<HttpResult> UploadStreamAsync(Stream stream, string saveKey, string token)
        {
            HttpResult result = new HttpResult();

            try
            {
                string boundary = HttpManager.CreateFormDataBoundary();
                string sep = "--" + boundary;
                StringBuilder sbp1 = new StringBuilder();

                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=key");
                sbp1.AppendLine();
                sbp1.AppendLine(saveKey);
                sbp1.AppendLine(sep);

                sbp1.AppendLine("Content-Disposition: form-data; name=token");
                sbp1.AppendLine();
                sbp1.AppendLine(token);
                sbp1.AppendLine(sep);

                sbp1.AppendFormat("Content-Disposition: form-data; name=file; filename=\"{0}\"", saveKey);
                sbp1.AppendLine();
                sbp1.AppendLine();

                int bufferSize = 1024 * 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead = 0;
                MemoryStream dataMS = new MemoryStream();
                while (true)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    await dataMS.WriteAsync(buffer, 0, bytesRead);
                }

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = dataMS.ToArray();
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                await ms.WriteAsync(partData1, 0, partData1.Length);
                await ms.WriteAsync(partData2, 0, partData2.Length);
                await ms.WriteAsync(partData3, 0, partData3.Length);

                result = await httpManager.PostMultipartAsync(uploadHost, ms.ToArray(), boundary, null);
                result.RefText += string.Format("[{0}] [FormUpload] Uploaded: \"#STREAM#\" ==> \"{1}\"\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), saveKey);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] [FormUpload] Error: ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }
                sb.AppendLine();

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

#endif

    }

}
