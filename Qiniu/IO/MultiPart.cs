using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Qiniu.Conf;
using Qiniu.RPC;

namespace Qiniu.IO
{
    static class MultiPart
    {
        public static Encoding encoding = Config.Encoding;

        public static string RandomBoundary()
        {
            return String.Format("----------{0:N}", Guid.NewGuid());
        }

        public static string FormDataContentType(string boundary)
        {
            return "multipart/form-data; boundary=" + boundary;
        }

        public static long ContentLength(Dictionary<string, object> postParams, string boundary)
        {
            long length = 0;
            bool needsCLRF = false;
            foreach (var param in postParams)
            {
                if (needsCLRF)
                    length += encoding.GetByteCount("\r\n");
                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n";
                    string header = string.Format(headfmt, boundary, param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.MimeType ?? "application/octet-stream");

                    length += encoding.GetByteCount(header);
                    using (FileStream fs = File.OpenRead(fileToUpload.FileName))
                    {
                        length += fs.Length;
                    }
                }
                else
                {
                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
                    string partData = string.Format(headfmt, boundary, param.Key, param.Value);
                    length += encoding.GetByteCount(partData);
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            length += encoding.GetByteCount(footer);
            return length;
        }

        public static long WriteBody(Dictionary<string, object> postParams, string boundary, Stream body)
        {
            long length = 0;
            bool needsCLRF = false;
            foreach (var param in postParams)
            {
                if (needsCLRF)
                {
                    body.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                    length += encoding.GetByteCount("\r\n");
                }
                if (param.Value is PutParameter)
                {
                    PutParameter objToUpload = (PutParameter)param.Value ;
                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n";
                    string header = string.Format(headfmt, boundary, param.Key,
                        objToUpload is FileParameter ? (((FileParameter)objToUpload).FileName ?? param.Key) : param.Key,
                        objToUpload.MimeType ?? "application/octet-stream");
                    
                    body.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    length += encoding.GetByteCount(header);

                    length += objToUpload.CopyTo(body);                           
                }
                else
                {
                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
                    string partData = string.Format(headfmt, boundary, param.Key, param.Value);
                    body.Write(encoding.GetBytes(partData), 0, encoding.GetByteCount(partData));
                    length += encoding.GetByteCount(partData);
                }
            }
            string footer = "\r\n--" + boundary + "--\r\n";
            length += encoding.GetByteCount(footer);
            body.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
            
            return length;
        }


        private static Stream GetPostStream(Stream putStream, string fileName,NameValueCollection formData, string boundary)
        {
            Stream postDataStream = new System.IO.MemoryStream();

            //adding form data

            string formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}";

            foreach (string key in formData.Keys)
            {
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate,
                key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            //adding file,Stream data
            #region adding file data
           
            string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
            Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine;
            byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate,
            "file", fileName));
            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
           
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = putStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            putStream.Close();
            #endregion

            #region adding end
            byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            #endregion

            return postDataStream;
 
        }

        private static Stream GetPostStream(string filePath, NameValueCollection formData, string boundary)
        {
            Stream postDataStream = new System.IO.MemoryStream();

            //adding form data

            string formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}";

            foreach (string key in formData.Keys)
            {
                byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(formDataHeaderTemplate,
                key, formData[key]));
                postDataStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            //adding file data
            #region adding file data
            FileInfo fileInfo = new FileInfo(filePath);
            string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
            Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine;
            byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes(string.Format(fileHeaderTemplate,
            "file", fileInfo.FullName));
            postDataStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);
            FileStream fileStream = fileInfo.OpenRead();
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                postDataStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            #endregion

            #region adding end
            byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
            postDataStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            #endregion

            return postDataStream;
        }

        public static CallRet MultiPost(string url, NameValueCollection formData,string fileName)
        {
            string boundary = RandomBoundary();
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(url);

            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            FileInfo fileInfo = new FileInfo(fileName);

            using (FileStream fileStream = fileInfo.OpenRead())
            {

                Stream postDataStream = GetPostStream(fileStream, fileName, formData, boundary);
                webRequest.ContentLength = postDataStream.Length;
                Stream reqStream = webRequest.GetRequestStream();
                postDataStream.Position = 0;

                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    reqStream.Write(buffer, 0, bytesRead);
                }
                postDataStream.Close();
                reqStream.Close();
            }
            try
            {
                using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse)
                {                   
                    return RPC.Client.HandleResult(response);
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }            
        }


        public static CallRet Post(string url, Dictionary<string, object> postParams)
        {
            Console.WriteLine("URL: " + url);
            string boundary = RandomBoundary();
            string contentType = FormDataContentType(boundary);

            Stream requestStream = null;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = Conf.Config.USER_AGENT;
                request.Method = "POST";
                request.ContentType = contentType;
                //request.ContentLength = ContentLength(postParams, boundary);
                Stream body = new MemoryStream();
                request.ContentLength = WriteBody(postParams, boundary, body);
                requestStream = request.GetRequestStream();        
                
                body.CopyTo(requestStream); 
                
                response = request.GetResponse() as HttpWebResponse;
                return RPC.Client.HandleResult(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
            finally
            {
                requestStream.Close();
                response.Close();
            }
        }
    }
}
