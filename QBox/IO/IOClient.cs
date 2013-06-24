using System;
using System.Collections.Generic;
using System.Net;
using QBox.Conf;
using QBox.Auth;
using QBox.RPC;
using QBox.Util;

namespace QBox.IO
{
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
            string entryURI = extra.Bucket + ":" + key;
            string action = "/rs-put/" + Base64URLSafe.Encode(entryURI);
            if (!String.IsNullOrEmpty(extra.MimeType))
            {
                action += "/mimeType/" + Base64URLSafe.Encode(extra.MimeType);
            }
            //if (!String.IsNullOrEmpty(extra.))
            //{
            //    action += "/meta/" + Base64URLSafe.Encode(extra.CustomMeta);
            //}
            if (extra.Crc32 >= 0)
            {
                action += "/crc32/" + extra.Crc32.ToString();
            }

            try
            {
                var postParams = new Dictionary<string, object>();
                postParams["key"] = key;
                postParams["auth"] = upToken;
                postParams["action"] = action;
                postParams["file"] = new FileParameter(localFile, extra.MimeType);
                if (!String.IsNullOrEmpty(extra.Params))
                    postParams["params"] = extra.Params;
               // MultiPart.Post(@"http://up.qiniu.com", postParams);
                //return null;
                CallRet callRet = MultiPart.Post(Config.UP_HOST + "/upload", postParams);
                return new PutRet(callRet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }

        public static PutRet ResumablePutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            PutAuthClient client = new PutAuthClient(upToken);
            return null;
            //return ResumablePut.PutFile(client, extra.Bucket, key, extra.MimeType, localFile, 
               // extra.CustomMeta, extra.Params);
        }
    }
}
