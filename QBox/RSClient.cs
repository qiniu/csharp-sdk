using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace QBox
{
    public class RSClient
    {
        public static PutFileRet PutFile(
            string url, string tableName, string key, string mimeType,
            string localFile, string customMeta, string callbackParam)
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

            try
            {
                var postParams = new Dictionary<string, object>();
                postParams["action"] = action;
                postParams["file"] = new FileParameter(localFile, mimeType);
                if (!String.IsNullOrEmpty(callbackParam))
                    postParams["params"] = callbackParam;
                CallRet callRet = MultiPartFormDataPost.DoPost(url, postParams);
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
