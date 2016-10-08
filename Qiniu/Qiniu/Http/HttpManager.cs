using Newtonsoft.Json;
using Qiniu.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Qiniu.Http
{
    /// <summary>
    // instance of this class can be sahred
    /// </summary>
    public class HttpManager
    {
        public static string FORM_MIME_URLENCODED = "application/x-www-form-urlencoded";
        public static string FORM_MIME_OCTECT = "application/octect-stream";
        public static string FORM_MIME_JSON = "application/json";
        public static string FORM_BOUNDARY_TAG = "--";
        public static int COPY_BYTES_BUFFER = 40 * 1024 * 1024; //40 KB

        private string getUserAgent()
        {
            return string.Format("QiniuCSharpSDK/{0} ({1}; {2}; {3})",
                Config.VERSION,
                Environment.MachineName,
                Environment.OSVersion.Platform,
                Environment.OSVersion.Version);
        }

        private string createFormDataBoundary()
        {
            string now = DateTime.Now.ToLongTimeString();
            return string.Format("-------QiniuCSharpSDKBoundary{0}", Qiniu.Util.StringUtils.md5Hash(now));
        }

        private string createRandomFilename()
        {
            string now = DateTime.Now.ToLongTimeString();
            return string.Format("randomfile{0}", Qiniu.Util.StringUtils.urlSafeBase64Encode(now));
        }

        /// <summary>
        /// get info from remote server
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pHeaders"></param>
        /// <param name="pCompletionHandler"></param>
        public void get(string pUrl, Dictionary<string, string> pHeaders,
            CompletionHandler pCompletionHandler)
        {
            HttpWebRequest vWebReq = null;
            HttpWebResponse vWebResp = null;
            try
            {
                vWebReq = (HttpWebRequest)WebRequest.Create(pUrl);
            }
            catch (Exception ex)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.invalidRequest(ex.Message), "");
                }
                return;
            }

            try
            {
                vWebReq.AllowAutoRedirect = false;
                vWebReq.Method = "GET";
                vWebReq.UserAgent = this.getUserAgent();
                if (pHeaders != null)
                {
                    foreach (KeyValuePair<string, string> kvp in pHeaders)
                    {
                        if (!kvp.Key.Equals("Content-Type"))
                        {
                            vWebReq.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                //fire request
                vWebResp = (HttpWebResponse)vWebReq.GetResponse();
                handleWebResponse(vWebResp, pCompletionHandler);
            }
            catch(WebException wexp)
            {
                // 在 HTTP-400 错误情形下HttpWebResponse(上述vWebResp)已经为null值
                // 需要在此处WebException的wexp.Response中获取真实的Response内容
                // 2016-08-10, 18:20 FIXED by fengyh (http://fengyh.cn/) 
                if (wexp.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse xWebResp = wexp.Response as HttpWebResponse;
                    handleErrorWebResponse(xWebResp, pCompletionHandler, wexp);
                }
            }
            catch (Exception exp)
            {
                handleErrorWebResponse(vWebResp, pCompletionHandler, exp);
            }
        }

        /// <summary>
        /// post the url encoded form to remote server
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pHeaders"></param>
        /// <param name="pParamDict"></param>
        /// <param name="pCompletionHandler"></param>
        public void postForm(string pUrl, Dictionary<string, string> pHeaders,
            Dictionary<string, string[]> pPostParams, CompletionHandler pCompletionHandler)
        {
            HttpWebRequest vWebReq = null;
            HttpWebResponse vWebResp = null;
            try
            {
                vWebReq = (HttpWebRequest)WebRequest.Create(pUrl);
            }
            catch (Exception ex)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.invalidRequest(ex.Message), "");
                }
                return;
            }

            try
            {
                vWebReq.UserAgent = this.getUserAgent();
                vWebReq.AllowAutoRedirect = false;
                vWebReq.Method = "POST";
                vWebReq.ContentType = FORM_MIME_URLENCODED;
                if (pHeaders != null)
                {
                    foreach (KeyValuePair<string, string> kvp in pHeaders)
                    {
                        if (!kvp.Key.Equals("Content-Type"))
                        {
                            vWebReq.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                // format the post body
                StringBuilder vPostBody = new StringBuilder();
                if (pPostParams != null)
                {
                    foreach (KeyValuePair<string, string[]> kvp in pPostParams)
                    {
                        foreach (string vVal in kvp.Value)
                        {
                            vPostBody.AppendFormat("{0}={1}&",
                                Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(vVal));
                        }
                    }
                    // write data
                    vWebReq.AllowWriteStreamBuffering = true;
                    using (Stream vWebReqStream = vWebReq.GetRequestStream())
                    {
                        vWebReqStream.Write(Encoding.UTF8.GetBytes(vPostBody.ToString()),
                            0, vPostBody.Length - 1);
                        vWebReqStream.Flush();
                    }
                }

                //fire request
                vWebResp = (HttpWebResponse)vWebReq.GetResponse();
                handleWebResponse(vWebResp, pCompletionHandler);
            }
            catch (WebException wexp)
            {
                // FIX-HTTP400E-NullWebResponse 2016-08-17, 14:57 @fengyh
                if (wexp.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse xWebResp = wexp.Response as HttpWebResponse;
                    handleErrorWebResponse(xWebResp, pCompletionHandler, wexp);
                }
            }
            catch (Exception exp)
            {
                handleErrorWebResponse(vWebResp, pCompletionHandler, exp);
            }
        }


        /// <summary>
        /// post the binary data to the remote server
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pHeaders"></param>
        /// <param name="pPostData"></param>
        /// <param name="pCompletionHandler"></param>
        public void postData(string pUrl, Dictionary<string, string> pHeaders,
            byte[] pPostData, string contentType, CompletionHandler pCompletionHandler)
        {
            HttpWebRequest vWebReq = null;
            HttpWebResponse vWebResp = null;
            try
            {
                vWebReq = (HttpWebRequest)WebRequest.Create(pUrl);
                vWebReq.ServicePoint.Expect100Continue = false;
            }
            catch (Exception ex)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.invalidRequest(ex.Message), "");
                }
                return;
            }

            try
            {
                vWebReq.UserAgent = this.getUserAgent();
                vWebReq.AllowAutoRedirect = false;
                vWebReq.Method = "POST";
                if (!string.IsNullOrEmpty(contentType))
                {
                    vWebReq.ContentType = contentType;
                }
                else
                {
                    vWebReq.ContentType = FORM_MIME_OCTECT;
                }
                if (pHeaders != null)
                {
                    foreach (KeyValuePair<string, string> kvp in pHeaders)
                    {
                        if (!kvp.Key.Equals("Content-Type"))
                        {
                            vWebReq.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                vWebReq.AllowWriteStreamBuffering = true;
                // write data
                using (Stream vWebReqStream = vWebReq.GetRequestStream())
                {
                    vWebReqStream.Write(pPostData, 0, pPostData.Length);
                    vWebReqStream.Flush();
                }

                //fire request
                vWebResp = (HttpWebResponse)vWebReq.GetResponse();
                handleWebResponse(vWebResp, pCompletionHandler);
            }
            catch (WebException wexp)
            {
                // FIX-HTTP400E-NullWebResponse 2016-08-17, 14:57 @fengyh
                if (wexp.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse xWebResp = wexp.Response as HttpWebResponse;
                    handleErrorWebResponse(xWebResp, pCompletionHandler, wexp);
                }
            }
            catch (Exception exp)
            {
                handleErrorWebResponse(vWebResp, pCompletionHandler, exp);
            }
        }

        /// <summary>
        /// post binary data to remote server
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pHeaders"></param>
        /// <param name="pPostData"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="pCompletionHandler"></param>
        public void postData(string pUrl, Dictionary<string, string> pHeaders,
            byte[] pPostData, int offset, int count, string contentType,
            CompletionHandler pCompletionHandler)
        {
            HttpWebRequest vWebReq = null;
            HttpWebResponse vWebResp = null;
            try
            {
                vWebReq = (HttpWebRequest)WebRequest.Create(pUrl);
            }
            catch (Exception ex)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.invalidRequest(ex.Message), "");
                }
                return;
            }

            try
            {
                vWebReq.UserAgent = this.getUserAgent();
                vWebReq.AllowAutoRedirect = false;
                vWebReq.Method = "POST";
                if (!string.IsNullOrEmpty(contentType))
                {
                    vWebReq.ContentType = contentType;
                }
                else
                {
                    vWebReq.ContentType = FORM_MIME_OCTECT;
                }
                if (pHeaders != null)
                {
                    foreach (KeyValuePair<string, string> kvp in pHeaders)
                    {
                        if (!kvp.Key.Equals("Content-Type"))
                        {
                            vWebReq.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                vWebReq.AllowWriteStreamBuffering = true;
                // write data
                using (Stream vWebReqStream = vWebReq.GetRequestStream())
                {
                    vWebReqStream.Write(pPostData, offset, count);
                    vWebReqStream.Flush();
                }

                //fire request
                vWebResp = (HttpWebResponse)vWebReq.GetResponse();
                handleWebResponse(vWebResp, pCompletionHandler);
            }
            catch (WebException wexp)
            {
                // FIX-HTTP400E-NullWebResponse 2016-08-17, 14:57 @fengyh
                if (wexp.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse xWebResp = wexp.Response as HttpWebResponse;
                    handleErrorWebResponse(xWebResp, pCompletionHandler, wexp);
                }
            }
            catch (Exception exp)
            {
                handleErrorWebResponse(vWebResp, pCompletionHandler, exp);
            }
        }

        /// <summary>
        /// post multi-part data form to remote server
        /// used to upload file
        /// </summary>
        /// <param name="pUrl"></param>
        /// <param name="pHeaders"></param>
        /// <param name="pPostParams"></param>
        /// <param name="httpFormFile"></param>
        /// <param name="pProgressHandler"></param>
        /// <param name="pCompletionHandler"></param>
        public void postMultipartDataForm(string pUrl, Dictionary<string, string> pHeaders,
           Dictionary<string, string> pPostParams, HttpFormFile pFormFile,
            ProgressHandler pProgressHandler, CompletionHandler pCompletionHandler)
        {
            if (pFormFile == null)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.fileError(new Exception("no file specified")), "");
                }
                return;
            }

            HttpWebRequest vWebReq = null;
            HttpWebResponse vWebResp = null;
            try
            {
                vWebReq = (HttpWebRequest)WebRequest.Create(pUrl);
                vWebReq.ServicePoint.Expect100Continue = false;
            }
            catch (Exception ex)
            {
                if (pCompletionHandler != null)
                {
                    pCompletionHandler(ResponseInfo.invalidRequest(ex.Message), "");
                }
                return;
            }

            try
            {
                vWebReq.UserAgent = this.getUserAgent();
                vWebReq.AllowAutoRedirect = false;
                vWebReq.Method = "POST";

                //create boundary
                string formBoundaryStr = this.createFormDataBoundary();
                string contentType = string.Format("multipart/form-data; boundary={0}", formBoundaryStr);
                vWebReq.ContentType = contentType;
                if (pHeaders != null)
                {
                    foreach (KeyValuePair<string, string> kvp in pHeaders)
                    {
                        if (!kvp.Key.Equals("Content-Type"))
                        {
                            vWebReq.Headers.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                //write post body
                vWebReq.AllowWriteStreamBuffering = true;

                byte[] formBoundaryBytes = Encoding.UTF8.GetBytes(string.Format("{0}{1}\r\n",
                    FORM_BOUNDARY_TAG, formBoundaryStr));
                byte[] formBoundaryEndBytes = Encoding.UTF8.GetBytes(string.Format("\r\n{0}{1}{2}\r\n",
                    FORM_BOUNDARY_TAG, formBoundaryStr, FORM_BOUNDARY_TAG));

                using (Stream vWebReqStream = vWebReq.GetRequestStream())
                {
                    //write params
                    if (pPostParams != null)
                    {
                        foreach (KeyValuePair<string, string> kvp in pPostParams)
                        {
                            vWebReqStream.Write(formBoundaryBytes, 0, formBoundaryBytes.Length);

                            byte[] formPartTitleData = Encoding.UTF8.GetBytes(
                                string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n", kvp.Key));
                            vWebReqStream.Write(formPartTitleData, 0, formPartTitleData.Length);

                            byte[] formPartBodyData = Encoding.UTF8.GetBytes(string.Format("\r\n{0}\r\n", kvp.Value));
                            vWebReqStream.Write(formPartBodyData, 0, formPartBodyData.Length);
                        }
                    }

                    vWebReqStream.Write(formBoundaryBytes, 0, formBoundaryBytes.Length);

                    //write file name
                    string filename = pFormFile.Filename;
                    if (string.IsNullOrEmpty(filename))
                    {
                        filename = this.createRandomFilename();
                    }
                    byte[] filePartTitleData = Encoding.UTF8.GetBytes(
                                string.Format("Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\n", filename));
                    vWebReqStream.Write(filePartTitleData, 0, filePartTitleData.Length);
                    //write content type
                    string mimeType = FORM_MIME_OCTECT;  //!!!注意这里 @fengyh 2016-08-17 15:00
                    if (!string.IsNullOrEmpty(pFormFile.ContentType))
                    {
                        mimeType = pFormFile.ContentType;
                    }
                    byte[] filePartMimeData = Encoding.UTF8.GetBytes(string.Format("Content-Type: {0}\r\n\r\n", mimeType));
                    vWebReqStream.Write(filePartMimeData, 0, filePartMimeData.Length);

                    //write file data
                    switch (pFormFile.BodyType)
                    {
                        case HttpFileType.FILE_PATH:
                            try
                            {
                                FileStream fs = File.Open(pFormFile.BodyFile, FileMode.Open, FileAccess.Read);
                                this.writeHttpRequestBody(fs, vWebReqStream);
                            }
                            catch (Exception fex)
                            {
                                if (pCompletionHandler != null)
                                {
                                    pCompletionHandler(ResponseInfo.fileError(fex), "");
                                }
                            }
                            break;
                        case HttpFileType.FILE_STREAM:
                            this.writeHttpRequestBody(pFormFile.BodyStream, vWebReqStream);
                            break;
                        case HttpFileType.DATA_BYTES:
                            vWebReqStream.Write(pFormFile.BodyBytes, 0, pFormFile.BodyBytes.Length);
                            break;
                        case HttpFileType.DATA_SLICE:
                            vWebReqStream.Write(pFormFile.BodyBytes, pFormFile.Offset, pFormFile.Count);
                            break;
                    }

                    vWebReqStream.Write(formBoundaryEndBytes, 0, formBoundaryEndBytes.Length);
                    vWebReqStream.Flush();
                }

                //fire request
                vWebResp = (HttpWebResponse)vWebReq.GetResponse();
                handleWebResponse(vWebResp, pCompletionHandler);
            }
            catch (WebException wexp)
            {
                // FIX-HTTP400E-NullWebResponse 2016-08-17, 14:57 @fengyh
                if (wexp.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse xWebResp = wexp.Response as HttpWebResponse;
                    handleErrorWebResponse(xWebResp, pCompletionHandler, wexp);
                }
            }
            catch (Exception exp)
            {
                handleErrorWebResponse(vWebResp, pCompletionHandler, exp);
            }
        }

        private void writeHttpRequestBody(Stream fromStream, Stream toStream)
        {
            byte[] buffer = new byte[COPY_BYTES_BUFFER];
            int count = -1;
            using (fromStream)
            {
                while ((count = fromStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    toStream.Write(buffer, 0, count);
                }
            }
        }

        private void handleWebResponse(HttpWebResponse pWebResp, CompletionHandler pCompletionHandler)
        {
            DateTime startTime = DateTime.Now;
            //check for exception
            int statusCode = ResponseInfo.NetworkError;
            string reqId = null;
            string xlog = null;
            string ip = null;
            string xvia = null;
            string error = null;
            string host = null;
            string respData = null;

            statusCode = (int)pWebResp.StatusCode;
            if (pWebResp.Headers != null)
            {
                WebHeaderCollection respHeaders = pWebResp.Headers;
                foreach (string headerName in respHeaders.AllKeys)
                {
                    if (headerName.Equals("X-Reqid"))
                    {
                        reqId = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Log"))
                    {
                        xlog = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Via"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Px"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("Fw-Via"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("Host"))
                    {
                        host = respHeaders[headerName].ToString();
                    }
                }

                using (StreamReader respStream = new StreamReader(pWebResp.GetResponseStream()))
                {
                    respData = respStream.ReadToEnd();
                }

                try
                {
                    /////////////////////////////////////////////////////////////
                    // 改进Response的error解析, 根据HttpStatusCode
                    // @fengyh 2016-08-17 18:29
                    /////////////////////////////////////////////////////////////
                    if (statusCode != (int)HCODE.OK)
                    {
                        bool isOtherCode = HttpCode.GetErrorMessage(statusCode, out error);

                        if (isOtherCode)
                        {
                            Dictionary<string, string> errorDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(respData);
                            error = errorDict["error"];
                        }
                    }
                }
                catch (Exception) { }
            }

            double duration = DateTime.Now.Subtract(startTime).TotalSeconds;
            ResponseInfo respInfo = new ResponseInfo(statusCode, reqId, xlog, xvia, host, ip, duration, error);
            if (pCompletionHandler != null)
            {
                pCompletionHandler(respInfo, respData);
            }
        }

        private void handleErrorWebResponse(HttpWebResponse pWebResp, CompletionHandler pCompletionHandler, Exception pExp)
        {
            DateTime startTime = DateTime.Now;
            int statusCode = ResponseInfo.NetworkError;
            //parse self defined code from the error message
            string expMsg = pExp.Message;
            int indexStart = expMsg.IndexOf("(");
            if (indexStart != -1)
            {
                int indexEnd = expMsg.IndexOf(")", indexStart);
                if (indexStart != -1 && indexEnd != -1)
                {
                    string statusCodeStr = expMsg.Substring(indexStart + 1, indexEnd - indexStart - 1);
                    try
                    {
                        statusCode = Convert.ToInt32(statusCodeStr);
                    }
                    catch (Exception) { }
                }
            }
            //check for exception
            string reqId = null;
            string xlog = null;
            string ip = null;
            string xvia = null;
            string error = null;
            string host = null;
            string respData = null;

            if (pWebResp != null && pWebResp.Headers != null)
            {
                WebHeaderCollection respHeaders = pWebResp.Headers;
                foreach (string headerName in respHeaders.AllKeys)
                {
                    if (headerName.Equals("X-Reqid"))
                    {
                        reqId = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Log"))
                    {
                        xlog = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Via"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("X-Px"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("Fw-Via"))
                    {
                        xvia = respHeaders[headerName].ToString();
                    }
                    else if (headerName.Equals("Host"))
                    {
                        host = respHeaders[headerName].ToString();
                    }
                }

                using (StreamReader respStream = new StreamReader(pWebResp.GetResponseStream()))
                {
                    respData = respStream.ReadToEnd();
                }

                try
                {
                    /////////////////////////////////////////////////////////////
                    // 改进Response的error解析, 根据HttpStatusCode
                    // @fengyh 2016-08-17 18:29
                    /////////////////////////////////////////////////////////////
                    if (statusCode != (int)HCODE.OK)
                    {
                        bool isOtherCode = HttpCode.GetErrorMessage(statusCode, out error);

                        if (isOtherCode)
                        {
                            Dictionary<string, string> errorDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(respData);
                            error = errorDict["error"];
                        }
                    }
                }
                catch (Exception) { }
            }
            else
            {
                error = pExp.Message;
            }

            double duration = DateTime.Now.Subtract(startTime).TotalSeconds;
            ResponseInfo respInfo = new ResponseInfo(statusCode, reqId, xlog, xvia, host, ip, duration, error);
            if (pCompletionHandler != null)
            {
                pCompletionHandler(respInfo, respData);
            }
        }

    }
}

