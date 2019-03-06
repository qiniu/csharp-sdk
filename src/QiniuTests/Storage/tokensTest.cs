using System;
using Qiniu.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qiniu.Tests;
using NUnit.Framework;
using System.Text;

namespace QiniuTests.Storage
{
    /// <summary>
    /// TokensTest
    /// </summary>
    [TestFixture]
    public class TokensTest:TestEnv
    {
        /// <summary>
        /// getQiniuToken
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void getQiniuToken()
        {  
            // input url
            string strUrl = "";
            // input method(POST/GET)
            string method = "";
            // input byte[] body
            JObject jsonBody = new JObject();
            string jsonobj = JsonConvert.SerializeObject(jsonBody);
            byte[] body = Encoding.UTF8.GetBytes(jsonobj);
            //input contentType
            string contentType = ""; 

            Mac mac = new Mac(AccessKey, SecretKey);
            Auth auth = new Auth(mac);
            string qiniuToken = auth.CreateQiniuToken(strUrl, method, body, contentType);
            Console.WriteLine(qiniuToken);
        }
        /// <summary>
        /// getQboxToken
        /// </summary>
        /// <returns>void</returns>
        [Test]
        public void getQboxToken()
        {
            // input url
            string strUrl = "";
            //input body
            JObject jsonBody = new JObject();
            string jsonobj = JsonConvert.SerializeObject(jsonBody);
            byte[] body = Encoding.UTF8.GetBytes(jsonobj);

            Mac mac = new Mac(AccessKey, SecretKey);
            Auth auth = new Auth(mac);
            string qboxToken = auth.CreateManageToken(strUrl,body);
            Console.WriteLine(qboxToken);
        }
    }
}
