using System;
using System.IO;
using System.Text;

namespace QBox.RS
{
    public static class ResumablePut
    {
        private static int ChunkBits = 22;
        private static long ChunkSize = 1 << ChunkBits;

        public static ResumablePutFileRet Mkblock(Client client, Stream body, long length)
        {
            string url = Config.UP_HOST + "/mkblk/" + Convert.ToString(length);
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", body, length);
            return new ResumablePutFileRet(callRet);
        }

        public static PutFileRet Mkfile(Client client, string entryURI, long fsize, 
            string customMeta, string callbackParam, string[] ctxs)
        {
            string url = Config.UP_HOST + "/rs-mkfile/" + Base64UrlSafe.Encode(entryURI) + 
                "/fsize/" + Convert.ToString(fsize);
            if (!String.IsNullOrEmpty(callbackParam))
            {
                url += "/params/" + callbackParam;
            }
            if (!String.IsNullOrEmpty(customMeta))
            {
                url += "/meta/" + customMeta;
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
            using (FileStream fs = File.OpenRead(localFile))
            {
                fsize = fs.Length;
                int chunkCnt = (int)((fsize + (ChunkSize - 1)) >> ChunkBits);
                long chunkSize = ChunkSize;
                ctxs = new string[chunkCnt];
                for (int i = 0; i < chunkCnt; i++)
                {
                    if (i == chunkCnt - 1)
                    {
                        chunkSize = fsize - (i << ChunkBits);
                    }
                    ResumablePutFileRet ret = null;
                    for (int retry = 0; retry < Config.PUT_RETRY_TIMES; retry++)
                    {
                        ret = Mkblock(client, fs, chunkSize);
                        if (ret.OK)
                        {
                            ctxs[i] = ret.Ctx;
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
            return Mkfile(client, entryURI, fsize, customMeta, callbackParam, ctxs);
        }
    }
}
