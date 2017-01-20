using System;
using System.IO;
using System.Text;
using Qiniu.Util;
using Qiniu.Common;
using Qiniu.Http;
using Qiniu.RSF.Model;

namespace Qiniu.RSF
{
    /// <summary>
    /// 数据处理
    /// </summary>
    public class OperationManager
    {
        private Auth auth;
        private HttpManager httpManager;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mac">账户访问控制(密钥)</param>
        public OperationManager(Mac mac)
        {
            auth = new Auth(mac);
            httpManager = new HttpManager();
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns>pfop操作返回结果，正确返回结果包含persistentId</returns>
        public PfopResult pfop(string bucket, string key, string fops, string pipeline, string notifyUrl, bool force)
        {
            PfopResult result = new PfopResult();

            try
            {
                string pfopUrl = Config.ZONE.ApiHost + "/pfop/";

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("bucket={0}&key={1}&fops={2}", bucket, key, StringHelper.urlEncode(fops));
                if (!string.IsNullOrEmpty(notifyUrl))
                {
                    sb.AppendFormat("&notifyURL={0}", notifyUrl);
                }
                if (force)
                {
                    sb.Append("&force=1");
                }
                if (!string.IsNullOrEmpty(pipeline))
                {
                    sb.AppendFormat("&pipeline={0}", pipeline);
                }
                byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                string token = auth.createManageToken(pfopUrl, data);

                HttpResult hr = httpManager.postForm(pfopUrl, data, token);
                result.shadow(hr);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] pfop Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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
        /// 数据处理，操作字符串拼接后与另一种形式等价
        /// </summary>
        /// <param name="bucket">空间</param>
        /// <param name="key">空间文件的key</param>
        /// <param name="fops">操作(命令参数)列表</param>
        /// <param name="pipeline">私有队列</param>
        /// <param name="notifyUrl">通知url</param>
        /// <param name="force">forece参数</param>
        /// <returns>操作返回结果，正确返回结果包含persistentId</returns>
        public PfopResult pfop(string bucket, string key, string[] fops, string pipeline, string notifyUrl, bool force)
        {
            string ops = string.Join(";", fops);
            return pfop(bucket, key, ops, pipeline, notifyUrl, force);
        }

        /// <summary>
        /// 查询pfop操作处理结果(或状态)
        /// </summary>
        /// <param name="persistentId">持久化ID</param>
        /// <returns>操作结果</returns>
        public static HttpResult prefop(string persistentId)
        {
            HttpResult result = new HttpResult();

            try
            {
                string prefopUrl = string.Format("{0}/status/get/prefop?id={1}", Config.ZONE.ApiHost, persistentId);

                HttpManager httpMgr = new HttpManager();
                result = httpMgr.get(prefopUrl, null);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] prefop Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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
        /// 根据uri的类型(网络url或者本地文件路径)自动选择dfop_url或者dfop_data
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="uri">资源/文件URI</param>
        /// <returns>操作结果/返回数据</returns>
        public HttpResult dfop(string fop, string uri)
        {
            if (UrlHelper.isValidUrl(uri))
            {
                return dfopUrl(fop, uri);
            }
            else
            {
                return dfopData(fop, uri);
            }
        }

        /// <summary>
        /// 文本处理
        /// </summary>
        /// <param name="fop">文本处理命令</param>
        /// <param name="text">文本内容</param>
        /// <returns></returns>
        public HttpResult dfopText(string fop, string text)
        {
            HttpResult result = new HttpResult();

            try
            {
                string dfopUrl = string.Format("{0}/dfop?fop={1}", Config.DFOP_API_HOST, fop);
                string token = auth.createManageToken(dfopUrl);
                string boundary = HttpManager.createFormDataBoundary();
                string sep = "--" + boundary;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(sep);
                sb.AppendFormat("Content-Type: {0}", ContentType.TEXT_PLAIN);
                sb.AppendLine();
                sb.AppendLine("Content-Disposition: form-data; name=data; filename=text");
                sb.AppendLine();
                sb.AppendLine(text);
                sb.AppendLine(sep + "--");
                byte[] data = Encoding.UTF8.GetBytes(sb.ToString());

                result = httpManager.postMultipart(dfopUrl, data, boundary, token, true);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] dfop Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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
        /// 文本处理
        /// </summary>
        /// <param name="fop">文本处理命令</param>
        /// <param name="textFile">文本文件</param>
        /// <returns></returns>
        public HttpResult dfopTextFile(string fop, string textFile)
        {
            HttpResult result = new HttpResult();

            if (File.Exists(textFile))
            {
                result = dfopText(fop, File.ReadAllText(textFile));
            }
            else
            {
                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText = "[dfop error] File not found: " + textFile;
            }

            return result;
        }

        /// <summary>
        /// 如果uri是网络url则使用此方法
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="url">资源URL</param>
        /// <returns>处理结果</returns>
        public HttpResult dfopUrl(string fop, string url)
        {
            HttpResult result = new HttpResult();

            try
            {
                string encodedUrl = StringHelper.urlEncode(url);
                string dfopUrl = string.Format("{0}/dfop?fop={1}&url={2}", Config.DFOP_API_HOST, fop, encodedUrl);
                string token = auth.createManageToken(dfopUrl);

                result = httpManager.post(dfopUrl, token, true);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] dfop Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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
        /// 如果uri是本地文件路径则使用此方法
        /// </summary>
        /// <param name="fop">文件处理命令</param>
        /// <param name="localFile">文件名</param>
        /// <returns>处理结果</returns>
        public HttpResult dfopData(string fop, string localFile)
        {
            HttpResult result = new HttpResult();

            try
            {
                string dfopUrl = string.Format("{0}/dfop?fop={1}", Config.DFOP_API_HOST, fop);
                string token = auth.createManageToken(dfopUrl);
                string boundary = HttpManager.createFormDataBoundary();
                string sep = "--" + boundary;

                StringBuilder sbp1 = new StringBuilder();
                sbp1.AppendLine(sep);
                string filename = Path.GetFileName(localFile);
                sbp1.AppendFormat("Content-Type: {0}", ContentType.APPLICATION_OCTET_STREAM);
                sbp1.AppendLine();
                sbp1.AppendFormat("Content-Disposition: form-data; name=\"data\"; filename={0}", filename);
                sbp1.AppendLine();
                sbp1.AppendLine();

                StringBuilder sbp3 = new StringBuilder();
                sbp3.AppendLine();
                sbp3.AppendLine(sep + "--");

                byte[] partData1 = Encoding.UTF8.GetBytes(sbp1.ToString());
                byte[] partData2 = File.ReadAllBytes(localFile);
                byte[] partData3 = Encoding.UTF8.GetBytes(sbp3.ToString());

                MemoryStream ms = new MemoryStream();
                ms.Write(partData1, 0, partData1.Length);
                ms.Write(partData2, 0, partData2.Length);
                ms.Write(partData3, 0, partData3.Length);

                result = httpManager.postMultipart(dfopUrl, ms.ToArray(), boundary, token, true);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("[{0}] dfop Error:  ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
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

    }
}
