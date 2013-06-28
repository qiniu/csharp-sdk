using System;
using System.IO;
using System.Text;
using QBox.Conf;
using QBox.Util;
using QBox.RPC;

namespace QBox.IO
{
    static class ResumablePut
    {
        private static int ChunkBits = 18;
        private static long ChunkSize = 1 << ChunkBits;
        private static int PutRetryTimes = 3;

        public static ResumablePutRet Mkblock(string host, Client client, Stream body, long length)
        {
            string url = host + "/mkblk/" + Convert.ToString(length);
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", body, length);
            return new ResumablePutRet(callRet);
        }

        public static PutRet Mkfile(string host, Client client, string entryURI, long fsize, 
            string customMeta, string callbackParam, string[] ctxs)
        {
            string url = host + "/rs-mkfile/" + Base64URLSafe.Encode(entryURI) + 
                "/fsize/" + Convert.ToString(fsize);
            if (!String.IsNullOrEmpty(callbackParam))
            {
                url += "/params/" + Base64URLSafe.Encode(callbackParam);
            }
            if (!String.IsNullOrEmpty(customMeta))
            {
                url += "/meta/" + Base64URLSafe.Encode(customMeta);
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
                return new PutRet(ret);
            }
        }
        

        public static PutRet PutFile(
            Client client, string tableName, string key, string mimeType, 
            string localFile, string callbackParam)
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
                    ResumablePutRet ret = null;
                    for (int retry = 0; retry < PutRetryTimes; retry++)
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
                        return new PutRet(new CallRet(ret));
                    }
                }
            }

            string entryURI = tableName + ":" + key;
            return Mkfile(Config.UP_HOST, client, entryURI, fsize, "", callbackParam, ctxs);
        }
    }
}
