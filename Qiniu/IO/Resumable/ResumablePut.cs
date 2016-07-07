using System;
using System.IO;
using System.Text;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.Util;
#if ABOVE45
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif

namespace Qiniu.IO.Resumable
{
    /// <summary>
    /// 异步并行断点上传类
    /// </summary>
    public class ResumablePut
    {
        private const int blockBits = 22;
        private const int blockMashk = (1 << blockBits) - 1;
        private static int BLOCKSIZE = 4 * 1024 * 1024;

        /// <summary>
        /// 上传完成事件
        /// </summary>
        public event EventHandler<CallRet> PutFinished;
        /// <summary>
        /// 上传Failure事件
        /// </summary>
        public event EventHandler<CallRet> PutFailure;

        Settings putSetting;

        /// <summary>
        /// 上传设置
        /// </summary>
        public Settings PutSetting
        {
            get { return putSetting; }
            set { putSetting = value; }
        }

        ResumablePutExtra extra;

        /// <summary>
        /// PutExtra
        /// </summary>
        public ResumablePutExtra Extra
        {
            get { return extra; }
            set { extra = value; }
        }

        /// <summary>
        /// 断点续传类
        /// </summary>
        /// <param name="putSetting"></param>
        /// <param name="extra"></param>
        public ResumablePut(Settings putSetting, ResumablePutExtra extra)
        {
            this.putSetting = putSetting;
            this.extra = extra;
        }
#if !ABOVE45
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken">上传Token</param>
        /// <param name="key">key</param>
        /// <param name="localFile">本地文件名</param>
        public CallRet PutFile(string upToken, string localFile, string key)
        {
            if (!File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }
            
            PutAuthClient client = new PutAuthClient(upToken);
            CallRet ret;
            using (FileStream fs = File.OpenRead(localFile))
            {
                int block_cnt = block_count(fs.Length);
                long fsize = fs.Length;
                extra.Progresses = new BlkputRet[block_cnt];
                byte[] byteBuf = new byte[BLOCKSIZE];
                int readLen = BLOCKSIZE;
                for (int i = 0; i < block_cnt; i++)
                {
                    if (i == block_cnt - 1) { 
                        readLen = (int)(fsize - (long)i * BLOCKSIZE);
                    }
                    fs.Seek((long)i * BLOCKSIZE, SeekOrigin.Begin);
                    fs.Read(byteBuf, 0, readLen);
                    BlkputRet blkRet = ResumableBlockPut(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                    }
                    else
                    {
                        extra.OnNotify(new PutNotifyEvent(i, readLen, extra.Progresses[i]));
                    }
                }
                ret = Mkfile(client, key, fsize);
            }
            if (ret.OK)
            {
                if (PutFinished != null)
                {
                    PutFinished(this, ret);
                }
            }
            else
            {
                if (PutFailure != null)
                {
                    PutFailure(this, ret);
                }
            }
            return ret;
        }


        private BlkputRet ResumableBlockPut(Client client, byte[] body, int blkIdex, int blkSize)
        {
        #region Mkblock
            uint crc32 = CRC32.CheckSumBytes(body, blkSize);
            for (int i = 0; i < putSetting.TryTimes; i++)
            {
                try
                {
                    extra.Progresses[blkIdex] = Mkblock(client, body, blkSize);
                }
                catch (Exception ee)
                {
                    if (i == (putSetting.TryTimes - 1))
                    {
                        throw ee;
                    }
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                if (extra.Progresses[blkIdex] == null || crc32 != extra.Progresses[blkIdex].crc32)
                {
                    if (i == (putSetting.TryTimes - 1))
                    {
                        return null;
                    }
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    break;
                }
            }
        #endregion

            return extra.Progresses[blkIdex];
        }

        private BlkputRet Mkblock(Client client, byte[] firstChunk, int blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);
            
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream",new MemoryStream(firstChunk, 0, blkSize),blkSize);
            if (callRet.OK)
            {
                return QiniuJsonHelper.ToObject<BlkputRet>(callRet.Response);
            }
            return null;
        }

        private CallRet Mkfile(Client client, string key, long fsize)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("{0}/mkfile/{1}", Config.UP_HOST, fsize);
            if (key != null)
            {
                urlBuilder.AppendFormat("/key/{0}", Base64URLSafe.ToBase64URLSafe(key));
            }
            if (!string.IsNullOrEmpty(extra.MimeType))
            {
                urlBuilder.AppendFormat("/mimeType/{0}", Base64URLSafe.ToBase64URLSafe(extra.MimeType));
            }
            if (!string.IsNullOrEmpty(extra.CustomMeta))
            {
                urlBuilder.AppendFormat("/meta/{0}", Base64URLSafe.ToBase64URLSafe(extra.CustomMeta));
            }
            if (extra.CallbackParams != null && extra.CallbackParams.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string _key in extra.CallbackParams.Keys)
                {
                    sb.AppendFormat("/{0}/{1}", _key, Base64URLSafe.ToBase64URLSafe(extra.CallbackParams[_key]));
                }
                urlBuilder.Append(sb.ToString());
            }

            int proCount = extra.Progresses.Length;
            using (Stream body = new MemoryStream())
            {
                for (int i = 0; i < proCount; i++)
                {
                    byte[] bctx = Encoding.ASCII.GetBytes(extra.Progresses[i].ctx);
                    body.Write(bctx, 0, bctx.Length);
                    if (i != proCount - 1)
                    {
                        body.WriteByte((byte)',');
                    }
                }
                body.Seek(0, SeekOrigin.Begin);
                return client.CallWithBinary(urlBuilder.ToString(), "text/plain", body, body.Length);
            }
        }
#else
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken">上传Token</param>
        /// <param name="key">key</param>
        /// <param name="localFile">本地文件名</param>
        public async Task<CallRet> PutFileAsync(string upToken, string localFile, string key)
        {
            if (!File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }

            PutAuthClient client = new PutAuthClient(upToken);
            CallRet ret;
            using (FileStream fs = File.OpenRead(localFile))
            {
                int block_cnt = block_count(fs.Length);
                long fsize = fs.Length;
                extra.Progresses = new BlkputRet[block_cnt];
                byte[] byteBuf = new byte[BLOCKSIZE];
                int readLen = BLOCKSIZE;
                for (int i = 0; i < block_cnt; i++)
                {
                    if (i == block_cnt - 1)
                    {
                        readLen = (int)(fsize - (long)i * BLOCKSIZE);
                    }
                    fs.Seek((long)i * BLOCKSIZE, SeekOrigin.Begin);
                    fs.Read(byteBuf, 0, readLen);
                    BlkputRet blkRet = await ResumableBlockPutAsync(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                    }
                    else
                    {
                        extra.OnNotify(new PutNotifyEvent(i, readLen, extra.Progresses[i]));
                    }
                }
                ret = await MkfileAsync(client, key, fsize);
            }
            if (ret.OK)
            {
                if (PutFinished != null)
                {
                    PutFinished(this, ret);
                }
            }
            else
            {
                if (PutFailure != null)
                {
                    PutFailure(this, ret);
                }
            }
            return ret;
        }

        private async Task<BlkputRet> ResumableBlockPutAsync(Client client, byte[] body, int blkIdex, int blkSize)
        {
            #region Mkblock
            uint crc32 = CRC32.CheckSumBytes(body, blkSize);
            for (int i = 0; i < putSetting.TryTimes; i++)
            {
                try
                {
                    extra.Progresses[blkIdex] = await MkblockAsync(client, body, blkSize);
                }
                catch (Exception ee)
                {
                    if (i == (putSetting.TryTimes - 1))
                    {
                        throw ee;
                    }
                    await Task.Delay(1000);
                    continue;
                }
                if (extra.Progresses[blkIdex] == null || crc32 != extra.Progresses[blkIdex].crc32)
                {
                    if (i == (putSetting.TryTimes - 1))
                    {
                        return null;
                    }
                    await Task.Delay(1000);
                    continue;
                }
                else
                {
                    break;
                }
            }
            #endregion

            return extra.Progresses[blkIdex];
        }

        private async Task<BlkputRet> MkblockAsync(Client client, byte[] firstChunk, int blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);

            var content = new ByteArrayContent(firstChunk, 0, blkSize);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            CallRet callRet = await client.CallWithBinaryAsync(url, content);
            if (callRet.OK)
            {
                return QiniuJsonHelper.ToObject<BlkputRet>(callRet.Response);
            }
            return null;
        }

        private async Task<CallRet> MkfileAsync(Client client, string key, long fsize)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("{0}/mkfile/{1}", Config.UP_HOST, fsize);
            if (key != null)
            {
                urlBuilder.AppendFormat("/key/{0}", Base64URLSafe.ToBase64URLSafe(key));
            }
            if (!string.IsNullOrEmpty(extra.MimeType))
            {
                urlBuilder.AppendFormat("/mimeType/{0}", Base64URLSafe.ToBase64URLSafe(extra.MimeType));
            }
            if (!string.IsNullOrEmpty(extra.CustomMeta))
            {
                urlBuilder.AppendFormat("/meta/{0}", Base64URLSafe.ToBase64URLSafe(extra.CustomMeta));
            }
            if (extra.CallbackParams != null && extra.CallbackParams.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string _key in extra.CallbackParams.Keys)
                {
                    sb.AppendFormat("/{0}/{1}", _key, Base64URLSafe.ToBase64URLSafe(extra.CallbackParams[_key]));
                }
                urlBuilder.Append(sb.ToString());
            }

            int proCount = extra.Progresses.Length;
            using (Stream body = new MemoryStream())
            {
                for (int i = 0; i < proCount; i++)
                {
                    byte[] bctx = Encoding.ASCII.GetBytes(extra.Progresses[i].ctx);
                    body.Write(bctx, 0, bctx.Length);
                    if (i != proCount - 1)
                    {
                        body.WriteByte((byte)',');
                    }
                }
                body.Seek(0, SeekOrigin.Begin);

                var content = new StreamContent(body);
                content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                return await client.CallWithBinaryAsync(urlBuilder.ToString(), content);
            }
        }
#endif



        private int block_count(long fsize)
        {
            return (int)((fsize + blockMashk) >> blockBits);
        }
    }
}
