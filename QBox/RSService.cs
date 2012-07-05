using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace QBox
{
    public class RSService
    {
        public Client Conn { get; private set; }
        public string TableName { get; private set; }

        public RSService(Client conn, string tableName)
        {
            Conn = conn;
            TableName = tableName;
        }

        public PutAuthRet PutAuth()
        {
            CallRet callRet = Conn.Call(Config.IO_HOST + "/put-auth/");
            return new PutAuthRet(callRet);
        }

        public PutFileRet Put(string key, string mimeType, byte[] body, string customMeta)
        {
            string entryURI = TableName + ":" + key;
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

            CallRet callRet = Conn.CallWithBinary(url, mimeType, body);
            return new PutFileRet(callRet);
        }

        public PutFileRet PutFile(string key, string mimeType, string localFile, string customMeta)
        {
            byte[] bs = null;
            try
            {
                using (FileStream fs = File.OpenRead(localFile))
                {
                    bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new PutFileRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
            
            return Put(key, mimeType, bs, customMeta);
        }

        public GetRet Get(string key, string attName)
        {
            string entryURI = TableName + ":" + key;
            string url = Config.RS_HOST + "/get/" + Base64UrlSafe.Encode(entryURI) + "/attName/"
                    + Base64UrlSafe.Encode(attName);
            CallRet callRet = Conn.Call(url);
            return new GetRet(callRet);
        }

        public GetRet GetIfNotModified(string key, string attName, string hash)
        {
            string entryURI = TableName + ":" + key;
            string url = Config.RS_HOST + "/get/" + Base64UrlSafe.Encode(entryURI) + "/attName/"
                    + Base64UrlSafe.Encode(attName) + "/base/" + hash;
            CallRet callRet = Conn.Call(url);
            return new GetRet(callRet);
        }

        public StatRet Stat(string key)
        {
            String entryURI = TableName + ":" + key;
            String url = Config.RS_HOST + "/stat/" + Base64UrlSafe.Encode(entryURI);
            CallRet callRet = Conn.Call(url);
            return new StatRet(callRet);
        }

        public PublishRet Publish(string domain)
        {
            String url = Config.RS_HOST + "/publish/" + Base64UrlSafe.Encode(domain) + 
                         "/from/" + TableName;
            CallRet callRet = Conn.Call(url);
            return new PublishRet(callRet);
        }

        public PublishRet UnPublish(string domain)
        {
            string url = Config.RS_HOST + "/unpublish/" + Base64UrlSafe.Encode(domain);
            CallRet callRet = Conn.Call(url);
            return new PublishRet(callRet);
        }

        public DeleteRet Delete(string key)
        {
            string entryURI = TableName + ":" + key;
            string url = Config.RS_HOST + "/delete/" + Base64UrlSafe.Encode(entryURI);
            CallRet callRet = Conn.Call(url);
            return new DeleteRet(callRet);
        }

        public DropRet Drop()
        {
            string url = Config.RS_HOST + "/drop/" + TableName;
            CallRet callRet = Conn.Call(url);
            return new DropRet(callRet);
        }
    }
}
