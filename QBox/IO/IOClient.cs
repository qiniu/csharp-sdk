using System;
using System.Collections.Generic;
using System.Net;
using QBox.Conf;
using QBox.Auth;
using QBox.RPC;
using QBox.Util;
using QBox.IO.Resumable;

namespace QBox.IO
{
    /// <summary>
    /// 上传客户端
    /// </summary>
    public class IOClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static PutRet PutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            //string entryURI = extra.Bucket + ":" + key;
            //string action = "/rs-put/" + Base64URLSafe.Encode(entryURI);
            //if (!String.IsNullOrEmpty(extra.MimeType))
            //{
            //    action += "/mimeType/" + Base64URLSafe.Encode(extra.MimeType);
            //}
            //if (extra.Crc32 >= 0)
            //{
            //    action += "/crc32/" + extra.Crc32.ToString();
            //}
            try
            {
                var postParams = new Dictionary<string, object>();
                postParams["token"] = upToken;
                postParams["key"] = key;
                postParams["file"] = new FileParameter(localFile, extra.MimeType);
                if (extra.Params!=null)
                {
                    foreach (KeyValuePair<string, string> pair in extra.Params)
                    {
                        postParams["x:" + pair.Key] = pair.Value;
                    }
                }
                CallRet callRet = MultiPart.Post(Config.UP_HOST, postParams);
                return new PutRet(callRet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public static PutRet ResumablePutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            PutAuthClient client = new PutAuthClient(upToken);
            Settings setting = new Settings(1, 1);
            IO.Resumable.PutExtra ext = new Resumable.PutExtra();            
            return ResumablePut.PutFile(client, extra.Bucket, key, extra.MimeType, localFile, "");
            //return ResumablePut.PutFile(client, extra.Bucket, key, extra.MimeType, localFile,extra.Params);
        }        
    }
}
