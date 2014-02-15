using System;
using System.IO;
using System.Text;
#if NET40
using System.Threading.Tasks;
#endif
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;
using Qiniu.Util;

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

        #region 记录总文件大小,用于计算上传百分比

        private long fsize;
        private float chunks;
        private float uploadedChunks = 0;

        #endregion

        /// <summary>
        /// 上传完成事件
        /// </summary>
        public event EventHandler<CallRet> PutFinished;
        /// <summary>
        /// 上传Failure事件
        /// </summary>
        public event EventHandler<CallRet> PutFailure;
        /// <summary>
        /// 进度提示事件
        /// </summary>
        public event Action<float> Progress;

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
            extra.chunkSize = putSetting.ChunkSize;
            this.putSetting = putSetting;
            this.extra = extra;
        }

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
                fsize = fs.Length;
                chunks = fsize / extra.chunkSize + 1;
                extra.Progresses = new BlkputRet[block_cnt];
                //并行上传
#if NET35
                for (int i = 0; i < block_cnt; i++)
                {
#elif  NET40
                Parallel.For(0, block_cnt, (i) =>{
#endif

                    int readLen = BLOCKSIZE;
                    if ((i + 1) * BLOCKSIZE > fsize)
                        readLen = (int)(fsize - i * BLOCKSIZE);
                    byte[] byteBuf = new byte[readLen];
#if NET40
                    lock (fs)
                    {
#endif
                        fs.Seek(i * BLOCKSIZE, SeekOrigin.Begin);
                        fs.Read(byteBuf, 0, readLen);
#if NET40
                    }
#endif
                    //并行上传BLOCK
                    BlkputRet blkRet = ResumableBlockPut(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                    }
                    else
                    {
                        extra.OnNotify(new PutNotifyEvent(i, readLen, extra.Progresses[i]));
                    }
#if NET35
                }
#elif NET40
                    });
#endif
                ret = Mkfile(client, key, fs.Length);
            }
            if (ret.OK)
            {
                if (Progress != null)
                {
                    Progress(1.0f);
                }
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


        /// <summary>
        /// 百分比进度提示
        /// </summary>
        private void progress()
        {
            uploadedChunks++;
            if (Progress != null)
            {
                Progress((float)uploadedChunks / chunks);
            }
        }

        private BlkputRet ResumableBlockPut(Client client, byte[] body, int blkIdex, int blkSize)
        {
            int bodyLength;
            int chunkSize = extra.chunkSize;
            #region Mkblock
            if (extra.Progresses[blkIdex] == null)
            {
                bodyLength = chunkSize < blkSize ? chunkSize : blkSize;
                byte[] firstChunk = new byte[bodyLength];
                Array.Copy(body, 0, firstChunk, 0, bodyLength);
                uint crc32 = CRC32.CheckSumBytes(firstChunk);
                for (int i = 0; i < putSetting.TryTimes; i++)
                {
                    extra.Progresses[blkIdex] = Mkblock(client, firstChunk, body.Length);
                    if (extra.Progresses[blkIdex] == null || crc32 != extra.Progresses[blkIdex].crc32)
                    {
                        if (i == (putSetting.TryTimes - 1))
                        {
                            return null;
                        }
                        continue;
                    }
                    else
                    {
                        progress();
                        break;
                    }
                }
            }
            #endregion

            #region PutBlock
            while (extra.Progresses[blkIdex].offset < blkSize)
            {
                bodyLength = (chunkSize < (blkSize - extra.Progresses[blkIdex].offset)) ? chunkSize : (int)(blkSize - extra.Progresses[blkIdex].offset);
                byte[] chunk = new byte[bodyLength];
                Array.Copy(body, extra.Progresses[blkIdex].offset, chunk, 0, bodyLength);
                for (int i = 0; i < putSetting.TryTimes; i++)
                {
                    extra.Progresses[blkIdex] = BlockPut(client, extra.Progresses[blkIdex], new MemoryStream(chunk), bodyLength);
                    if (extra.Progresses[blkIdex] == null)
                    {
                        if (i == (putSetting.TryTimes - 1))
                        {
                            return null;
                        }
                        continue;
                    }
                    else
                    {
                        uploadedChunks++;
                        if (Progress != null)
                        {
                            Progress((float)uploadedChunks / chunks);
                        }
                        break;
                    }
                }
            }
            #endregion
            return extra.Progresses[blkIdex];
        }

        private BlkputRet Mkblock(Client client, byte[] firstChunk, long blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", new MemoryStream(firstChunk), firstChunk.Length);
            if (callRet.OK)
            {
                return callRet.Response.ToObject<BlkputRet>();
            }
            return null;
        }

        private BlkputRet BlockPut(Client client, BlkputRet ret, Stream body, long length)
        {
            string url = string.Format("{0}/bput/{1}/{2}", Config.UP_HOST, ret.ctx, ret.offset);
            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", body, length);
            if (callRet.OK)
            {
                return callRet.Response.ToObject<BlkputRet>();
            }
            return null;
        }

        private CallRet Mkfile(Client client, string key, long fsize)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("{0}/mkfile/{1}", Config.UP_HOST, fsize);
            if (key != null)
            {
                urlBuilder.AppendFormat("/key/{0}", key.ToBase64URLSafe());
            }
            if (!string.IsNullOrEmpty(extra.MimeType))
            {
                urlBuilder.AppendFormat("/mimeType/{0}", extra.MimeType.ToBase64URLSafe());
            }
            if (!string.IsNullOrEmpty(extra.CustomMeta))
            {
                urlBuilder.AppendFormat("/meta/{0}", extra.CustomMeta.ToBase64URLSafe());
            }
            if (extra.CallbackParams != null && extra.CallbackParams.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string _key in extra.CallbackParams.Keys)
                {
                    sb.AppendFormat("/{0}/{1}", _key, extra.CallbackParams[_key].ToBase64URLSafe());
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

        private int block_count(long fsize)
        {
            return (int)((fsize + blockMashk) >> blockBits);
        }
    }
}
