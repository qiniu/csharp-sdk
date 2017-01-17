#if Net20 || Net30 || Net35 || Net40

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Qiniu.Util;

namespace Qiniu.Http
{
    /// <summary>
    /// HttpManager for .NET 2.0/3.0/3.5/4.0
    /// </summary>
    public class HttpManager
    {
        private string userAgent;

        /// <summary>
        /// 初始化
        /// </summary>
        public HttpManager()
        {
            userAgent = getUserAgent();
        }

        /// <summary>
        /// 客户端标识
        /// </summary>
        /// <returns>客户端标识UA</returns>
        public static string getUserAgent()
        {
            string osDesc = Environment.OSVersion.Platform + "; " + Environment.OSVersion.Version;
            return string.Format("{0}/{1} ({2})", QiniuCSharpSDK.ALIAS, QiniuCSharpSDK.VERSION, osDesc);
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>多部分表单数据的boundary</returns>
        public static string createFormDataBoundary()
        {
            string now = DateTime.UtcNow.Ticks.ToString();
            return string.Format("-------{0}Boundary{1}", QiniuCSharpSDK.ALIAS, Hashing.calcMD5(now));
        }

        /// <summary>
        /// HTTP-GET方法
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult get(string url, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "GET";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.UserAgent = userAgent;

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Get Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(不包含数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult post(string url, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.UserAgent = userAgent;

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含二进制格式数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postData(string url, byte[] data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = ContentType.APPLICATION_OCTET_STREAM;
                wReq.UserAgent = userAgent;

                if (data != null)
                {
                    wReq.AllowWriteStreamBuffering = true;
                    using (Stream sReq = wReq.GetRequestStream())
                    {
                        sReq.Write(data, 0, data.Length);
                        sReq.Flush();
                    }
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-data Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含JSON编码格式的数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postJson(string url, string data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = ContentType.APPLICATION_JSON;
                wReq.UserAgent = userAgent;

                if (data != null)
                {
                    wReq.AllowWriteStreamBuffering = true;
                    using (Stream sReq = wReq.GetRequestStream())
                    {
                        sReq.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
                        sReq.Flush();
                    }
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-json Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="kvData">键值对数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, Dictionary<string, string> kvData, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = ContentType.WWW_FORM_URLENC;
                wReq.UserAgent = userAgent;

                if (kvData != null)
                {
                    StringBuilder sbb = new StringBuilder();
                    foreach (var kv in kvData)
                    {
                        sbb.AppendFormat("{0}={1}&", Uri.EscapeDataString(kv.Key), Uri.EscapeDataString(kv.Value));
                    }

                    wReq.AllowWriteStreamBuffering = true;
                    using (Stream sReq = wReq.GetRequestStream())
                    {
                        sReq.Write(Encoding.UTF8.GetBytes(sbb.ToString()), 0, sbb.Length - 1);
                        sReq.Flush();
                    }
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, string data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = ContentType.WWW_FORM_URLENC;
                wReq.UserAgent = userAgent;

                if (!string.IsNullOrEmpty(data))
                {
                    wReq.AllowWriteStreamBuffering = true;
                    using (Stream sReq = wReq.GetRequestStream())
                    {
                        sReq.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
                        sReq.Flush();
                    }
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, byte[] data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = ContentType.WWW_FORM_URLENC;
                wReq.UserAgent = userAgent;

                if (data != null)
                {
                    wReq.AllowWriteStreamBuffering = true;
                    using (Stream sReq = wReq.GetRequestStream())
                    {
                        sReq.Write(data, 0, data.Length);
                        sReq.Flush();
                    }
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含多分部数据,multipart/form-data)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="boundary">分界标志</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns></returns>
        public HttpResult postMultipart(string url, byte[] data, string boundary, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            HttpWebRequest wReq = null;

            try
            {
                wReq = WebRequest.Create(url) as HttpWebRequest;
                wReq.Method = "POST";
                if (!string.IsNullOrEmpty(token))
                {
                    wReq.Headers.Add("Authorization", token);
                }
                wReq.ContentType = string.Format("{0}; boundary={1}", ContentType.MULTIPART_FORM_DATA, boundary);
                wReq.UserAgent = userAgent;

                wReq.AllowWriteStreamBuffering = true;
                using (Stream sReq = wReq.GetRequestStream())
                {
                    sReq.Write(data, 0, data.Length);
                    sReq.Flush();
                }

                HttpWebResponse wResp = wReq.GetResponse() as HttpWebResponse;

                if (wResp != null)
                {
                    result.Code = (int)wResp.StatusCode;
                    result.RefCode = (int)wResp.StatusCode;

                    getHeaders(ref result, wResp);

                    if (binaryMode)
                    {
                        int len = (int)wResp.ContentLength;
                        result.Data = new byte[len];
                        int bytesLeft = len;
                        int bytesRead = 0;

                        using (BinaryReader br = new BinaryReader(wResp.GetResponseStream()))
                        {
                            while (bytesLeft > 0)
                            {
                                bytesRead = br.Read(result.Data, len - bytesLeft, bytesLeft);
                                bytesLeft -= bytesRead;
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                        {
                            result.Text = sr.ReadToEnd();
                        }
                    }

                    wResp.Close();
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse xResp = wex.Response as HttpWebResponse;
                if (xResp != null)
                {
                    result.Code = (int)xResp.StatusCode;
                    result.RefCode = (int)xResp.StatusCode;

                    getHeaders(ref result, xResp);

                    using (StreamReader sr = new StreamReader(xResp.GetResponseStream()))
                    {
                        result.Text = sr.ReadToEnd();
                    }

                    xResp.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-multipart Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }
            finally
            {
                if (wReq != null)
                {
                    wReq.Abort();
                }
            }

            return result;
        }

        /// <summary>
        /// 获取返回信息头
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="resp"></param>
        private void getHeaders(ref HttpResult hr, HttpWebResponse resp)
        {
            if (resp != null)
            {
                if (hr.RefInfo == null)
                {
                    hr.RefInfo = new Dictionary<string, string>();
                }

                hr.RefInfo.Add("ProtocolVersion", resp.ProtocolVersion.ToString());

                if (!string.IsNullOrEmpty(resp.CharacterSet))
                {
                    hr.RefInfo.Add("Characterset", resp.CharacterSet);
                }

                if (!string.IsNullOrEmpty(resp.ContentEncoding))
                {
                    hr.RefInfo.Add("ContentEncoding", resp.ContentEncoding);
                }

                if (!string.IsNullOrEmpty(resp.ContentType))
                {
                    hr.RefInfo.Add("ContentType", resp.ContentType);
                }

                hr.RefInfo.Add("ContentLength", resp.ContentLength.ToString());                

                var headers = resp.Headers;
                if (headers != null && headers.Count > 0)
                {
                    if (hr.RefInfo == null)
                    {
                        hr.RefInfo = new Dictionary<string, string>();
                    }
                    foreach (var key in headers.AllKeys)
                    {
                        hr.RefInfo.Add(key, headers[key]);
                    }
                }
            }
        }

    }
}

#else

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Qiniu.Util;

namespace Qiniu.Http
{
    /// <summary>
    /// HttpManager for .NET 4.5+ and for .NET Core
    /// </summary>
    public class HttpManager
    {
        private HttpClient client;
        private string userAgent;

        /// <summary>
        /// HTTP超时间隔默认值(单位：秒)
        /// </summary>
        public int TIMEOUT_DEF_SEC = 30;

        /// <summary>
        /// HTTP超时间隔最大值(单位：秒)
        /// </summary>
        public int TIMEOUT_MAX_SEC = 60;

        /// <summary>
        /// 初始化
        /// </summary>
        public HttpManager()
        {
            client = new HttpClient();
            userAgent = getUserAgent();
        }

        /// <summary>
        /// 清理
        /// </summary>
        ~HttpManager()
        {
            client.Dispose();
            client = null;
        }

        /// <summary>
        /// 客户端标识
        /// </summary>
        /// <returns>客户端标识UA</returns>
        public static string getUserAgent()
        {
#if NetStandard
            string osDesc = "";

            var windows = System.Runtime.InteropServices.OSPlatform.Windows;
            var linux = System.Runtime.InteropServices.OSPlatform.Linux;
            var osx = System.Runtime.InteropServices.OSPlatform.OSX;
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(windows);
            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(linux);
            bool isOSX = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(osx);

            
            if (isWindows)
            {
                osDesc = "Windows;";
            }
            else if (isLinux)
            {
                osDesc = "Linux;";
            }
            else if (isOSX)
            {
                osDesc = "OSX;";
            }
            else
            {
                osDesc = "Other;";
            }
            
            if (isWindows)
            {
                osDesc += " " + System.Runtime.InteropServices.RuntimeInformation.OSDescription.TrimEnd();
            }
            else
            {
                string[] oss = System.Runtime.InteropServices.RuntimeInformation.OSDescription.Split(' ');
                for (int i = 0; i < oss.Length && i < 2; ++i)
                {
                    osDesc += " " + oss[i];
                }
            }

#else
            string osDesc = Environment.OSVersion.Platform + "; " + Environment.OSVersion.Version;
#endif
            return string.Format("{0}/{1} ({2})", QiniuCSharpSDK.ALIAS, QiniuCSharpSDK.VERSION, osDesc);
        }

        /// <summary>
        /// 多部分表单数据(multi-part form-data)的分界(boundary)标识
        /// </summary>
        /// <returns>多部分表单数据的boundary</returns>
        public static string createFormDataBoundary()
        {
            string now = DateTime.UtcNow.Ticks.ToString();
            return string.Format("-------{0}Boundary{1}", QiniuCSharpSDK.ALIAS, Hashing.calcMD5(now));
        }

        /// <summary>
        /// 设置HTTP超时间隔
        /// </summary>
        /// <param name="seconds">超时间隔，单位为秒</param>
        public void setTimeout(int seconds)
        {
            if (seconds >= 1 && seconds <= TIMEOUT_MAX_SEC)
            {
                TIMEOUT_DEF_SEC = seconds;
            }

            client.Timeout = new TimeSpan(0, 0, TIMEOUT_DEF_SEC);
        }

        /// <summary>
        /// HTTP-GET方法
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult get(string url, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Get Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(不包含数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult post(string url, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含二进制格式数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postData(string url, byte[] data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var content = new ByteArrayContent(data);
                req.Content = content;
				req.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.APPLICATION_OCTET_STREAM);

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-data Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含JSON编码格式的数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postJson(string url, string data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var content = new StringContent(data);
                req.Content = content;
                req.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.APPLICATION_JSON);

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-json Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="kvData">键值对数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, Dictionary<string, string> kvData, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var content = new FormUrlEncodedContent(kvData);
                req.Content = content;
				req.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.WWW_FORM_URLENC);

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单数据</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, string data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var content = new StringContent(data);
                req.Content = content;
                req.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.WWW_FORM_URLENC);

                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含表单数据)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">表单</param>
        /// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns>响应结果</returns>
        public HttpResult postForm(string url, byte[] data, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);
                req.Headers.Add("User-Agent", userAgent);
                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }

                var content = new ByteArrayContent(data);
                req.Content = content;
				req.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType.WWW_FORM_URLENC);				
				
                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-form Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// HTTP-POST方法(包含多分部数据,multipart/form-data)
        /// </summary>
        /// <param name="url">请求目标URL</param>
        /// <param name="data">主体数据</param>
        /// <param name="boundary">分界标志</param>
		/// <param name="token">令牌(凭证)</param>
        /// <param name="binaryMode">是否以二进制模式读取响应内容</param>
        /// <returns></returns>
        public HttpResult postMultipart(string url, byte[] data, string boundary, string token, bool binaryMode = false)
        {
            HttpResult result = new HttpResult();

            try
            {
                HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, url);

                if (!string.IsNullOrEmpty(token))
                {
                    req.Headers.Add("Authorization", token);
                }
                req.Headers.Add("User-Agent", userAgent);

                var content = new ByteArrayContent(data);
                req.Content = content;
				string ct = string.Format("{0}; boundary={1}", ContentType.MULTIPART_FORM_DATA, boundary);
                req.Content.Headers.Add("Content-Type", ct);
                
                var msg = client.SendAsync(req).Result;
                result.Code = (int)msg.StatusCode;
                result.RefCode = (int)msg.StatusCode;

                getHeaders(ref result, msg);

                if (binaryMode)
                {
                    result.Data = msg.Content.ReadAsByteArrayAsync().Result;
                }
                else
                {
                    result.Text = msg.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder("Post-multipart Error: ");
                Exception e = ex;
                while (e != null)
                {
                    sb.Append(e.Message + " ");
                    e = e.InnerException;
                }

                sb.AppendFormat(" @{0}\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

                result.RefCode = (int)HttpCode.USER_EXCEPTION;
                result.RefText += sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// 获取返回信息头
        /// </summary>
        /// <param name="hr"></param>
        /// <param name="msg"></param>
        private void getHeaders(ref HttpResult hr, HttpResponseMessage msg)
        {
            if (msg != null)
            {
                var ch = msg.Content.Headers;
                if (ch != null)
                {
                    if (hr.RefInfo == null)
                    {
                        hr.RefInfo = new Dictionary<string, string>();
                    }

                    foreach (var d in ch)
                    {
                        string key = d.Key;
                        StringBuilder val = new StringBuilder();
                        foreach (var v in d.Value)
                        {
                            if (!string.IsNullOrEmpty(v))
                            {
                                val.AppendFormat("{0}; ", v);
                            }
                        }
                        string vs = val.ToString().TrimEnd(';', ' ');
                        if (!string.IsNullOrEmpty(vs))
                        {
                            hr.RefInfo.Add(key, vs);
                        }
                    }
                }

                var hh = msg.Headers;
                if (hh != null)
                {
                    if (hr.RefInfo == null)
                    {
                        hr.RefInfo = new Dictionary<string, string>();
                    }

                    foreach (var d in hh)
                    {
                        string key = d.Key;
                        StringBuilder val = new StringBuilder();
                        foreach (var v in d.Value)
                        {
                            if (!string.IsNullOrEmpty(v))
                            {
                                val.AppendFormat("{0}; ", v);
                            }
                        }
                        string vs = val.ToString().TrimEnd(';', ' ');
                        if (!string.IsNullOrEmpty(vs))
                        {
                            hr.RefInfo.Add(key, vs);
                        }
                    }
                }                
            }
        }

    }
}

#endif