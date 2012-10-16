using System;
using System.IO;
using System.Net;

namespace QBox.RS
{
    public class RSService
    {
        public Client Conn { get; private set; }
        public string BucketName { get; private set; }

        public RSService(Client conn, string bucketName)
        {
            Conn = conn;
            BucketName = bucketName;
        }

        public CallRet MkBucket()
        {
            string url = Config.RS_HOST + "/mkbucket/" + BucketName;
            return Conn.Call(url);
        }

        public PutAuthRet PutAuth()
        {
            CallRet callRet = Conn.Call(Config.IO_HOST + "/put-auth/");
            return new PutAuthRet(callRet);
        }

        public PutFileRet PutFile(string key, string mimeType, string localFile, string customMeta)
        {
            string entryURI = BucketName + ":" + key;
            if (String.IsNullOrEmpty(mimeType))
            {
                mimeType = "application/octet-stream";
            }
            string url = Config.IO_HOST + "/rs-put/" + Base64UrlSafe.Encode(entryURI) +
                         "/mimeType/" + Base64UrlSafe.Encode(mimeType);
            if (!String.IsNullOrEmpty(customMeta))
            {
                url += "/meta/" + Base64UrlSafe.Encode(customMeta);
            }

            try
            {
                using (FileStream fs = File.OpenRead(localFile))
                {
                    CallRet callRet = Conn.CallWithBinary(url, mimeType, fs);
                    return new PutFileRet(callRet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutFileRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }

        public GetRet Get(string key, string attName)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/get/" + Base64UrlSafe.Encode(entryURI) + "/attName/"
                    + Base64UrlSafe.Encode(attName);
            CallRet callRet = Conn.Call(url);
            return new GetRet(callRet);
        }

        public GetRet GetIfNotModified(string key, string attName, string hash)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/get/" + Base64UrlSafe.Encode(entryURI) + "/attName/"
                    + Base64UrlSafe.Encode(attName) + "/base/" + hash;
            CallRet callRet = Conn.Call(url);
            return new GetRet(callRet);
        }

        public StatRet Stat(string key)
        {
            String entryURI = BucketName + ":" + key;
            String url = Config.RS_HOST + "/stat/" + Base64UrlSafe.Encode(entryURI);
            CallRet callRet = Conn.Call(url);
            return new StatRet(callRet);
        }

        public CallRet Publish(string domain)
        {
            String url = Config.RS_HOST + "/publish/" + Base64UrlSafe.Encode(domain) + "/from/" + BucketName;
            return Conn.Call(url);
        }

        public CallRet Unpublish(string domain)
        {
            string url = Config.RS_HOST + "/unpublish/" + Base64UrlSafe.Encode(domain);
            return Conn.Call(url);
        }

        public CallRet Delete(string key)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/delete/" + Base64UrlSafe.Encode(entryURI);
            return Conn.Call(url);
        }

        public CallRet Drop()
        {
            string url = Config.RS_HOST + "/drop/" + BucketName;
            return Conn.Call(url);
        }
    }
}
