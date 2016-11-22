using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Qiniu.Common
{
    public class AutoZone
    {
        /// <summary>
        /// 从uc.qbox.me查询得到回复后，解析出upHost,然后根据upHost确定Zone
        /// </summary>
        /// <param name="accessKey">AK</param>
        /// <param name="bucketName">Bucket</param>
        public static ZoneID Query(string accessKey, string bucketName)
        {
            ZoneID zoneId = ZoneID.Default;

            // HTTP/GET https://uc.qbox.me/v1/query?ak=(AK)&bucket=(Bucket)
            // 该请求的返回数据参见后面的 QueryResponse 结构
            // 根据response消息提取出upHost
            string query = string.Format("https://uc.qbox.me/v1/query?ak={0}&bucket={1}", accessKey, bucketName);

            //不同区域upHost分别唯一，据此确定对应的Zone
            Dictionary<string, ZoneID> ZONE_DICT = new Dictionary<string, ZoneID>()
            {
                {"http://up.qiniu.com",     ZoneID.CN_East },
                {"http://up-z1.qiniu.com",  ZoneID.CN_North},
                {"http://up-z2.qiniu.com",  ZoneID.CN_South},
                {"http://up-na0.qiniu.com", ZoneID.US_North}
            };

            try
            {
                HttpWebRequest wReq = WebRequest.Create(query) as HttpWebRequest;
                wReq.Method = "GET";
                System.Net.HttpWebResponse wResp = wReq.GetResponse() as System.Net.HttpWebResponse;
                using (StreamReader sr = new StreamReader(wResp.GetResponseStream()))
                {
                    string respData = sr.ReadToEnd();
                    QueryResponse qr = Newtonsoft.Json.JsonConvert.DeserializeObject<QueryResponse>(respData);
                    string upHost = qr.HTTP.UP[0];
                    zoneId = ZONE_DICT[upHost];
                }
                wResp.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("ConfigZone:" + ex.Message);
            }

            return zoneId;
        }

        #region UC_QUERY_RESPONSE

        // 从uc.qbox.me返回的消息，使用Json解析
        // 以下是一个response示例
        // {
        //     "ttl" : 86400,
        //     "http" : {
        //         "up" : [
        //                     "http://up.qiniu.com",
        //                     "http://upload.qiniu.com",
        //                     "-H up.qiniu.com http://183.136.139.16"
        //                 ],
        //        "io" : [
        //                      "http://iovip.qbox.me"
        //                ]
        //             },
        //     "https" : {
        //          "io" : [
        //                     "https://iovip.qbox.me"
        //                  ],
        //         "up" : [
        //                     "https://up.qbox.me"
        //                  ]
        //                  }
        // }

        /// <summary>
        /// 从uc.qbox.me返回的消息
        /// </summary>
        private class QueryResponse
        {
            public string TTL { get; set; }
            public HttpBulk HTTP { get; set; }

            public HttpBulk HTTPS { get; set; }
        }

        /// <summary>
        /// HttpBulk作为QueryResponse的成员
        /// 包含uploadHost和iovip等
        /// </summary>
        private class HttpBulk
        {
            public string[] UP { get; set; }
            public string[] IO { get; set; }
        }

        #endregion UC_QUERY_RESPONSE  
    }
}
