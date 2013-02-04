using System;
using System.IO;
using System.Net;
using QBox.FileOp;
using QBox.Util;

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
            return Conn.Post(url);
        }

        public PutAuthRet PutAuth()
        {
            CallRet callRet = Conn.Post(Config.IO_HOST + "/put-auth/");
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
                    CallRet callRet = Conn.PostWithBinary(url, mimeType, fs, fs.Length);
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
            CallRet callRet = Conn.Post(url);
            return new GetRet(callRet);
        }

        public GetRet GetIfNotModified(string key, string attName, string hash)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/get/" + Base64UrlSafe.Encode(entryURI) + "/attName/"
                    + Base64UrlSafe.Encode(attName) + "/base/" + hash;
            CallRet callRet = Conn.Post(url);
            return new GetRet(callRet);
        }

        public StatRet Stat(string key)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/stat/" + Base64UrlSafe.Encode(entryURI);
            CallRet callRet = Conn.Post(url);
            return new StatRet(callRet);
        }

        public CallRet Publish(string domain)
        {
            String url = Config.RS_HOST + "/publish/" + Base64UrlSafe.Encode(domain) + "/from/" + BucketName;
            return Conn.Post(url);
        }

        public CallRet Unpublish(string domain)
        {
            string url = Config.RS_HOST + "/unpublish/" + Base64UrlSafe.Encode(domain);
            return Conn.Post(url);
        }

        public CallRet Delete(string key)
        {
            string entryURI = BucketName + ":" + key;
            string url = Config.RS_HOST + "/delete/" + Base64UrlSafe.Encode(entryURI);
            return Conn.Post(url);
        }

        public CallRet Drop()
        {
            string url = Config.RS_HOST + "/drop/" + BucketName;
            return Conn.Post(url);
        }

        public PutFileRet SaveAs(string url, string specStr, string key)
        {
            string entryURI = BucketName + ":" + key;
            url = url + specStr + "/save-as/" + Base64UrlSafe.Encode(entryURI);
            CallRet callRet = Conn.Post(url);
            return new PutFileRet(callRet);
        }

        public PutFileRet ImageMogrifySaveAs(string url, ImageMogrifySpec spec, string key)
        {
            string specStr = spec.MakeSpecString();
            return SaveAs(url, specStr, key);
        }
    }
}
