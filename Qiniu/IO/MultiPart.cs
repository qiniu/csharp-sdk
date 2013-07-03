using System;
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

        public static void WriteBody(Dictionary<string, object> postParams, string boundary, Stream body)
        {
            bool needsCLRF = false;
            foreach (var param in postParams)
            {
                if (needsCLRF)
                    body.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n";
                    string header = string.Format(headfmt, boundary, param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.MimeType ?? "application/octet-stream");

                    body.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                    using (FileStream fs = File.OpenRead(fileToUpload.FileName))
                    {
                        fs.CopyTo(body);
                    }
                }
                else
                {
                    string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
                    string partData = string.Format(headfmt, boundary, param.Key, param.Value);
                    body.Write(encoding.GetBytes(partData), 0, encoding.GetByteCount(partData));
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            body.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));
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
                request.ContentLength = ContentLength(postParams, boundary);
                requestStream = request.GetRequestStream();
                WriteBody(postParams, boundary, requestStream);
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
