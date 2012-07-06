using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace QBox
{
    public static class MultiPartFormDataPost
    {
        public static Encoding encoding = Encoding.ASCII;

        private static byte[] GetBody(Dictionary<string, object> postParams, string boundary)
        {
            using (var buffer = new MemoryStream())
            {
                bool needsCLRF = false;
                foreach (var param in postParams)
                {
                    if (needsCLRF)
                        buffer.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsCLRF = true;

                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;

                        string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n";
                        string header = string.Format(headfmt, boundary, param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.MimeType ?? "application/octet-stream");

                        buffer.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));
                        using (FileStream fs = File.OpenRead(fileToUpload.FileName))
                        {
                            fs.CopyTo(buffer);
                        }
                    }
                    else
                    {
                        string headfmt = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}";
                        string partData = string.Format(headfmt, boundary, param.Key, param.Value);
                        buffer.Write(encoding.GetBytes(partData), 0, encoding.GetByteCount(partData));
                    }
                }

                string footer = "\r\n--" + boundary + "--\r\n";
                buffer.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                return buffer.ToArray();
            }
        }

        private static CallRet CallWithBinary(string url, string contentType, byte[] body)
        {
            Console.WriteLine("URL: " + url);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = body.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                }
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return Client.HandleResult(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
        }

        public static CallRet DoPost(string url, Dictionary<string, object> postParams)
        {
            string boundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + boundary;

            byte[] body = GetBody(postParams, boundary);
            return CallWithBinary(url, contentType, body);
        }
    }
}
