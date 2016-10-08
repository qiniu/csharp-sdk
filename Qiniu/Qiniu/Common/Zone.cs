using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace Qiniu.Common
{
    /// 华东(CN_East)
    /// 华北(CN_North)
    /// 华南(CN_South)
    /// 北美(US_North)
    public enum ZoneID
    {
        CN_East,
        CN_North,
        CN_South,
        US_North,
        Default=CN_East
    };

    /// <summary>
    /// 目前已支持的区域：华东/华北/华南/北美
    /// </summary>
    public class Zone
    {
        // 资源管理
        public string RsHost { set; get; }

        // 资源列表
        public string RsfHost { set; get; }

        // 数据处理
        public string ApiHost { set; get; }

        // 镜像刷新、资源抓取
        public string IovipHost { set; get; }

        // 资源上传
        public string UpHost { set; get; }

        // CDN加速
        public string UploadHost { set; get; }

        /// <summary>
        /// 华东
        /// xx-(NULL)
        /// </summary>
        public static Zone ZONE_CN_East()
        {
            return new Zone()
            {
                RsHost = "http://rs.qbox.me",
                RsfHost = "http://rsf.qbox.me",
                ApiHost = "http://api.qiniu.com",
                IovipHost = "http://iovip.qbox.me",
                UpHost = "http://up.qiniu.com",
                UploadHost = "http://upload.qiniu.com"
            };
        }

        /// <summary>
        /// 华北
        /// xx-z1
        /// </summary>
        public static Zone ZONE_CN_North()
        {
            return new Zone()
            {
                RsHost = "http://rs-z1.qbox.me",
                RsfHost = "http://rsf-z1.qbox.me",
                ApiHost = "http://api-z1.qiniu.com",
                IovipHost = "http://iovip-z1.qbox.me",
                UpHost = "http://up-z1.qiniu.com",
                UploadHost = "http://upload-z1.qiniu.com"
            };
        }

        /// <summary>
        /// 华南
        /// xx-z2
        /// </summary>
        public static Zone ZONE_CN_South()
        {
            return new Zone()
            {
                RsHost = "http://rs-z2.qbox.me",
                RsfHost = "http://rsf-z2.qbox.me",
                ApiHost = "http://api-z2.qiniu.com",
                IovipHost = "http://iovip-z2.qbox.me",
                UpHost = "http://up-z2.qiniu.com",
                UploadHost = "http://upload-z2.qiniu.com"
            };
        }

        /// <summary>
        /// 北美
        /// xx-na0
        /// </summary>
        public static Zone ZONE_US_North()
        {
            return new Zone()
            {
                RsHost = "http://rs-na0.qbox.me",
                RsfHost = "http://rsf-na0.qbox.me",
                ApiHost = "http://api-na0.qiniu.com",
                IovipHost = "http://iovip-na0.qbox.me",
                UpHost = "http://up-na0.qiniu.com",
                UploadHost = "http://upload-na0.qiniu.com"
            };
        }

    }

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
