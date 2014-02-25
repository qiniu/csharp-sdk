using System;
using System.Collections.Generic;
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
        /// Allow cache put result
        /// </summary>
        public bool AllowCache = true;

        /// <summary>
        /// 断点续传类
        /// </summary>
        /// <param name="putSetting"></param>
        /// <param name="extra"></param>
        /// <param name="allowcache">true:允许上传结果在本地保存，这样当网络失去连接而再次重新上传时，对已经上传成功的快不需要再次上传</param>
        public ResumablePut(Settings putSetting, ResumablePutExtra extra,bool allowcache = true)
        {
            extra.chunkSize = putSetting.ChunkSize;
            this.putSetting = putSetting;
            this.extra = extra;
            this.AllowCache = allowcache;
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
            string puttedFile = string.Empty;
            Dictionary<int, BlkputRet> puttedBlk = new Dictionary<int, BlkputRet>();
            if (this.AllowCache)
            {
                puttedFile = ResumbalePutHelper.GetPutHistroryFile(localFile);
                puttedBlk = ResumbalePutHelper.GetHistory(puttedFile);
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
                for (int i = 0; i < block_cnt; i++)
                {

                    if (this.AllowCache && puttedBlk != null && puttedBlk.ContainsKey(i) && puttedBlk[i] != null)
                    {
                        Console.WriteLine(string.Format("block{0} has putted", i));
                        extra.Progresses[i] = puttedBlk[i];
                        continue;
                    }
                    int readLen = BLOCKSIZE;
                    if ((long)(i + 1) * BLOCKSIZE > fsize)
                        readLen = (int)(fsize - (long)i * BLOCKSIZE);
                    byte[] byteBuf = new byte[readLen];
                    fs.Seek((long)i * BLOCKSIZE, SeekOrigin.Begin);
                    fs.Read(byteBuf, 0, readLen);
                    //并行上传BLOCK
                    BlkputRet blkRet = ResumableBlockPut(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                    }
                    else
                    {
                        if (this.AllowCache)
                        {
                            ResumbalePutHelper.Append(puttedFile, i, blkRet);
                        }
                        extra.OnNotify(new PutNotifyEvent(i, readLen, extra.Progresses[i]));
                    }
                }
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
                    try
                    {
                        extra.Progresses[blkIdex] = Mkblock(client, firstChunk, body.Length);
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
                        progress();
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

        private int block_count(long fsize)
        {
            return (int)((fsize + blockMashk) >> blockBits);
        }
    }
}
