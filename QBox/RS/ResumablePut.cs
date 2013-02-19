using System;
using System.IO;
using System.Text;
using QBox.Util;
using QBox.RPC;

namespace QBox.RS
{
    public static class ResumablePut
    {
        private static int ChunkBits = 22;
        private static long ChunkSize = 1 << ChunkBits;

        public static ResumablePutFileRet Mkblock(string host, Client client, Stream body, long length)
        {
            string url = host + "/mkblk/" + Convert.ToString(length);
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", body, length);
            return new ResumablePutFileRet(callRet);
        }

        public static PutFileRet Mkfile(string host, Client client, string entryURI, long fsize, 
            string customMeta, string callbackParam, string[] ctxs)
        {
            string url = host + "/rs-mkfile/" + Base64UrlSafe.Encode(entryURI) + 
                "/fsize/" + Convert.ToString(fsize);
            if (!String.IsNullOrEmpty(callbackParam))
            {
                url += "/params/" + Base64UrlSafe.Encode(callbackParam);
            }
            if (!String.IsNullOrEmpty(customMeta))
            {
                url += "/meta/" + Base64UrlSafe.Encode(customMeta);
            }

            using (Stream body = new MemoryStream())
            {
                for (int i = 0; i < ctxs.Length; i++)
                {
                    byte[] bctx = Encoding.ASCII.GetBytes(ctxs[i]);
                    body.Write(bctx, 0, bctx.Length);
                    if (i != ctxs.Length-1)
                    {
                        body.WriteByte((byte)',');
                    }
                }
                body.Seek(0, SeekOrigin.Begin);
                CallRet ret= client.CallWithBinary(url, "text/plain", body, body.Length);
                return new PutFileRet(ret);
            }
        }

        public static PutFileRet PutFile(
            Client client, string tableName, string key, string mimeType, 
            string localFile, string customMeta, string callbackParam)
        {
            long fsize = 0;
            string[] ctxs = null;
            string host = Config.UP_HOST;
            using (FileStream fs = File.OpenRead(localFile))
            {
                fsize = fs.Length;
                int chunkCnt = (int)((fsize + (ChunkSize - 1)) >> ChunkBits);
                long chunkSize = ChunkSize;
                ctxs = new string[chunkCnt];
                Console.WriteLine("ResumablePut ==> fsize: {0}, chunkCnt: {1}", fsize, chunkCnt);
                for (int i = 0; i < chunkCnt; i++)
                {
                    if (i == chunkCnt - 1)
                    {
                        chunkSize = fsize - (i << ChunkBits);
                    }
                    ResumablePutFileRet ret = null;
                    for (int retry = 0; retry < Config.PUT_RETRY_TIMES; retry++)
                    {
                        fs.Seek(i * ChunkSize, SeekOrigin.Begin);
                        ret = Mkblock(host, client, fs, chunkSize);
                        if (ret.OK)
                        {
                            ctxs[i] = ret.Ctx;
                            host = ret.Host;
                            break;
                        }
                    }
                    if (!ret.OK)
                    {
                        Console.WriteLine(ret.Exception.ToString());
                        return new PutFileRet(new CallRet(ret));
                    }
                }
            }

            string entryURI = tableName + ":" + key;
            return Mkfile(host, client, entryURI, fsize, customMeta, callbackParam, ctxs);
        }
    }
}
