using System;
using System.Net;
using Qiniu.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace QiniuTests.Storage
{   
    class tokensTest
    {
        public static void getQiniuToken()
        {  
            // input accessKey secretKey
            string ak = "";
            string sk = "";
            // input url
            string strUrl = "";
            // input method(POST/GET)
            string method = "";
            // input byte[] body
            JObject jsonBody = new JObject();
            string jsonobj = JsonConvert.SerializeObject(jsonBody);
            byte[] body = jsonobj.ToString().GetBytes("UTF-8");
            //input contentType
            string contentType = "";

            Mac mac = new Mac(ak, sk);
            Auth auth = new Auth(mac);
            string qiniuToken = auth.CreateQiniuToken(strUrl, method, body, contentType);
            Console.WriteLine(qiniuToken);
        }
        public static void getQboxToken()
        {
            // input accessKey secretKey
            string ak = "";
            string sk = "";
            // input url
            string strUrl = "";
            //input body
            JObject jsonBody = new JObject();
            string jsonobj = JsonConvert.SerializeObject(jsonBody);
            byte[] body = jsonobj.ToString().GetBytes("UTF-8");

            Mac mac = new Mac(ak, sk);
            Auth auth = new Auth(mac);
            string qboxToken = auth.CreateManageToken(strUrl,body);
            Console.WriteLine(qboxToken);
        }
    }
}   
   
        
