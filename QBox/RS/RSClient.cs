using System;
using System.Collections.Generic;
using System.Net;
using QBox.Util;
using QBox.RPC;

namespace QBox.RS
{
    public class RSClient
    {
        public static PutFileRet PutFile(
            string url, string bucketName, string key, string mimeType,
            string localFile, string customMeta, string callbackParam)
        {
            string entryURI = bucketName + ":" + key;
            if (String.IsNullOrEmpty(mimeType))
            {
                mimeType = "application/octet-stream";
            }
            string action = "/rs-put/" + Base64UrlSafe.Encode(entryURI) +
                    "/mimeType/" + Base64UrlSafe.Encode(mimeType);
            if (!String.IsNullOrEmpty(customMeta))
            {
                action += "/meta/" + Base64UrlSafe.Encode(customMeta);
            }

            try
            {
                var postParams = new Dictionary<string, object>();
                postParams["action"] = action;
                postParams["file"] = new FileParameter(localFile, mimeType);
                if (!String.IsNullOrEmpty(callbackParam))
                    postParams["params"] = callbackParam;
                CallRet callRet = MultiPartFormDataPost.Post(url, postParams);
                return new PutFileRet(callRet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutFileRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }

        public static PutFileRet PutFileWithUpToken(
            string upToken, string tableName, string key, string mimeType,
            string localFile, string customMeta, string callbackParam)
        {
            return PutFileWithUpToken(upToken, tableName, key, 
                mimeType, localFile, customMeta, callbackParam, 0);
        }

        public static PutFileRet PutFileWithUpToken(
            string upToken, string tableName, string key, string mimeType,
            string localFile, string customMeta, string callbackParam, UInt32 crc32)
        {
            string entryURI = tableName + ":" + key;
            if (String.IsNullOrEmpty(mimeType))
            {
                mimeType = "application/octet-stream";
            }
            string action = "/rs-put/" + Base64UrlSafe.Encode(entryURI) +
                    "/mimeType/" + Base64UrlSafe.Encode(mimeType);
            if (!String.IsNullOrEmpty(customMeta))
            {
                action += "/meta/" + Base64UrlSafe.Encode(customMeta);
            }
            if (crc32 != 0)
            {
                action += "/crc32/" + crc32.ToString();
            }

            try
            {
                var postParams = new Dictionary<string, object>();
                postParams["auth"] = upToken;
                postParams["action"] = action;
                postParams["file"] = new FileParameter(localFile, mimeType);
                if (!String.IsNullOrEmpty(callbackParam))
                    postParams["params"] = callbackParam;
                CallRet callRet = MultiPartFormDataPost.Post(Config.UP_HOST + "/upload", postParams);
                return new PutFileRet(callRet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutFileRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }
    }
}
