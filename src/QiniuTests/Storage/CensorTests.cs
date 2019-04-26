using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qiniu.Tests;
using Qiniu.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using Qiniu.Tests;
using NUnit.Framework;

namespace QiniuTests.Storage
{
    /// <summary>
    /// CensorTests
    /// </summary>
    [TestFixture]
    public class CensorTests
    {
        /// <summary>
        /// CensorTest
        /// </summary>
        /// <returns>void</returns>
        public static void CensorTest(string[] args)
        {
            // input accessKey secretKey
            string ak = "";
            string sk = "";
            // input url
            string strUrl = "http://ai.qiniuapi.com/v3/image/censor";
            string testUrl = "";
            // input method(POST/GET)
            string method = "POST";
            // input byte[] body
            JObject jsonBody = new JObject();
            JArray array = new JArray() { "pulp", "terror", "politician" };
            JObject scenesJson = new JObject();
            scenesJson["scenes"] = array;
            JObject uriJson = new JObject();
            uriJson["uri"] = testUrl;

            jsonBody["data"] = uriJson;
            jsonBody["params"] = scenesJson;

            string jsonobj = JsonConvert.SerializeObject(jsonBody);

            Console.WriteLine(jsonBody.ToString());

            //input contentType
            string contentType = "application/json";

            Mac mac = new Mac(ak, sk);
            Auth auth = new Auth(mac);

            byte[] body = Encoding.Default.GetBytes(jsonobj);

            string qiniuToken = Auth.CreateQiniuToken(mac, strUrl, method, body, contentType);

            Console.WriteLine(qiniuToken);

            HttpWebResponse response = null;
            try
            {
                Uri url = new Uri(strUrl);
                HttpWebRequest mOkHttpClient = (HttpWebRequest)HttpWebRequest.Create(url);
                mOkHttpClient.Method = WebRequestMethods.Http.Post;
                mOkHttpClient.ContentType = contentType;
                mOkHttpClient.Headers.Add("Authorization", qiniuToken);
                mOkHttpClient.ContentLength = body.Length;
                using (System.IO.Stream requestStream = mOkHttpClient.GetRequestStream())
                {
                    CopyN(requestStream, new MemoryStream(body), body.Length);
                }
                response = (HttpWebResponse)mOkHttpClient.GetResponse();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception(e.Message);
            }

            // response never be null
            if ((int)response.StatusCode == 200)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string text = reader.ReadToEnd();
                JObject jsonObj = JObject.Parse(text);
                Console.WriteLine(jsonObj.ToString());
            }
            else
            {
                Console.WriteLine(response.StatusCode);
            }
        }

        public static void CopyN(Stream dst, Stream src, long numBytesToCopy)
        {
            int bufferLen = 32 * 1024;
            long l = src.Position;
            byte[] buffer = new byte[bufferLen];
            long numBytesWritten = 0;
            while (numBytesWritten<numBytesToCopy)
            {
                int len = bufferLen;
                if ((numBytesToCopy - numBytesWritten) < len)
                {
                    len = (int)(numBytesToCopy - numBytesWritten);
                }
                int n = src.Read(buffer, 0, len);
                if (n == 0) break;
                dst.Write(buffer, 0, n);
                numBytesWritten += n;
            }
            src.Seek(l, SeekOrigin.Begin);
            if (numBytesWritten != numBytesToCopy)
            {
                throw new Exception("StreamUtil.CopyN: nwritten not equal to ncopy");
            }
        }
    }   
}
