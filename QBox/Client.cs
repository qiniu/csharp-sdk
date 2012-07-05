using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace QBox
{
    public abstract class Client
    {
        public abstract void SetAuth(HttpWebRequest request);

        public CallRet Call(string url)
        {
            Console.WriteLine("URL: " + url);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                throw new NullReferenceException("request is not a http request");

            try
            {
                request.Method = "POST";
                SetAuth(request);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                CallRet callRet = HandleResult(response);
                response.Close();
                return callRet;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
        }

        public CallRet CallWithBinary(string url, string contentType, byte[] body)
        {
            Console.WriteLine("URL: " + url);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null)
                throw new NullReferenceException("request is not a http request");

            try
            {
                request.Method = "POST";
                request.ContentType = contentType;
                request.ContentLength = body.Length;
                SetAuth(request);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(body, 0, body.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }

            try
            {
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                CallRet callRet = HandleResult(response);
                response.Close();
                return callRet;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }
        }

        public static CallRet HandleResult(HttpWebResponse response)
        {
            if (response == null)
                return new CallRet(HttpStatusCode.BadRequest, "No response");

            HttpStatusCode statusCode = response.StatusCode;
            string responseStr;
            try
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                responseStr = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new CallRet(HttpStatusCode.BadRequest, e);
            }

            return new CallRet(statusCode, responseStr);
        }
    }
}
