using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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
        class CountBox
        {
            public long Count;
			public int lastPer;
            public CountBox()
            {
                this.Count = 0;
            }
        }

        #region
        /// <summary>
        /// Occurs when upload progress changed when call AsyncPutFile
        /// </summary>
        public event EventHandler<PutProgressEventArgs> PutProgressChanged;

        protected void onPutProgressChanged(object sender, PutProgressEventArgs percentage)
        {
            if (this.PutProgressChanged != null)
            {
                this.PutProgressChanged(sender, percentage);
            }
        }

        #endregion

        private const int blockBits = 22;
        private readonly static int BLOCKSIZE = 1 << blockBits;
        private readonly static int blockMashk = BLOCKSIZE - 1;

        /// <summary>
        /// 上传完成事件
        /// </summary>
        public event EventHandler<CallRet> PutFinished;
        /// <summary>
        /// 上传Failure事件
        /// </summary>
        public event EventHandler<CallRet> PutFailed;

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

        private object putCntLocker = new object();

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
                    if (i == block_cnt - 1)
                    {
                        readLen = (int)(fsize - (long)i * BLOCKSIZE);
                    }
                    fs.Seek((long)i * BLOCKSIZE, SeekOrigin.Begin);
                    fs.Read(byteBuf, 0, readLen);
                    BlkputRet blkRet = ResumableBlockPut(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                        return null;
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
                if (PutFailed != null)
                {
                    PutFailed(this, ret);
                }
            }
            return ret;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken">上传Token</param>
        /// <param name="key">key</param>
        /// <param name="localFile">本地文件名</param>
        public void AsyncPutFile(string upToken, string localFile, string key)
        {
            if (!File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }

            Action<int> action = (nonuse) =>
            {
                PutAuthClient client = new PutAuthClient(upToken);
                CallRet ret;
                using (FileStream fs = File.OpenRead(localFile))
                {
                    upToken = "UpToken " + upToken;
                    int block_cnt = block_count(fs.Length);
                    long fsize = fs.Length;
                    extra.Progresses = new BlkputRet[block_cnt];
                    int readLen = BLOCKSIZE;
                    CountBox putted = new CountBox();
#if NET40
                    int cur = 0;
                    object curLocker = new object();
                    uint workernum = this.putSetting.WorkerCount > block_cnt ? (uint)block_cnt : this.putSetting.WorkerCount ;
					ManualResetEvent[] doneEvents = new ManualResetEvent[workernum];
					for (int i = 0; i < workernum; i++)
                    {
                        doneEvents[i] = new ManualResetEvent(false);
                    }

                    System.Threading.Tasks.Parallel.ForEach(doneEvents, (state) =>
                    {
                        byte[] Buf = new byte[BLOCKSIZE];
                        int lcur = 0;
                        while (cur < block_cnt)
                        {
                            lock (curLocker)
                            {
                                lcur = cur;
                                cur++;
                            }
                            if (lcur == block_cnt - 1)
                            {
                                readLen = (int)(fsize - (long)lcur * BLOCKSIZE);
                                Buf = new byte[readLen];
                            }
                            lock (fs)
                            {
                                fs.Seek((long)lcur * BLOCKSIZE, SeekOrigin.Begin);
                                fs.Read(Buf, 0, readLen);
                            }
                            BlkputRet blkRet = AsyncResumableBlockPut(upToken, Buf, lcur, readLen, fsize, putted);
                            if (blkRet == null)
                            {
                                extra.OnNotifyErr(new PutNotifyErrorEvent(lcur, readLen, "Make Block Error"));
                                return;
                            }
                            else
                            {
                                extra.OnNotify(new PutNotifyEvent(lcur, readLen, extra.Progresses[lcur]));
                            }
                        }
                        state.Set();
                    });
                    EventWaitHandle.WaitAll(doneEvents);
#endif
#if NET20
                    byte[] byteBuf = new byte[BLOCKSIZE];
                    for (int i = 0; i < block_cnt; i++) {
                        if (i == block_cnt - 1) { 
                            readLen = (int)(fsize - (long)i * BLOCKSIZE);
                            byteBuf = new byte[readLen];
                        }
                        fs.Seek ((long)i * BLOCKSIZE, SeekOrigin.Begin);
                        fs.Read (byteBuf, 0, readLen);
                        BlkputRet blkRet = AsyncResumableBlockPut(upToken, byteBuf, i, readLen, fsize, putted);
                        if (blkRet == null) {
                            extra.OnNotifyErr (new PutNotifyErrorEvent (i, readLen, "Make Block Error"));
                            return;
                        } else {
                            extra.OnNotify (new PutNotifyEvent (i, readLen, extra.Progresses [i]));
                        }
                    }
#endif
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
                    if (PutFailed != null)
                    {
                        PutFailed(this, ret);
                    }
                }
            };
            action.BeginInvoke(0, null, null);
        }

        private BlkputRet AsyncResumableBlockPut(string uptoken, byte[] body, int blkIdex, int blkSize, long filesize, CountBox putted)
        {
            #region Mkblock
            ManualResetEvent done = new ManualResetEvent(false);
            uint crc32 = CRC32.CheckSumBytes(body, blkSize);
            for (int i = 0; i < putSetting.TryTimes; i++)
            {
                bool needretry = false;
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("Authorization", uptoken);
                        wc.UploadDataCompleted += (object sender, UploadDataCompletedEventArgs e) =>
                        {
                            //Error happens, need retry
                            if (e.Error != null)
                            {
                                needretry = true;
                            }
                            else
                            {
                                extra.Progresses[blkIdex] = QiniuJsonHelper.ToObject<BlkputRet>(Encoding.UTF8.GetString(e.Result));
                            }
                            done.Set();
                        };
                        if (i == 0)
                        {
                            long last = 0;
                            wc.UploadProgressChanged += (sender, e) =>
                            {
                                int per;
                                lock (putCntLocker)
                                {
                                    putted.Count += (e.BytesSent - last);
                                    last = e.BytesSent;
                                    per = (int)(100 * putted.Count / filesize);
                                }
								if(per != putted.lastPer){
                                	onPutProgressChanged(this, new PutProgressEventArgs(per));
									putted.lastPer = per;
								}
                            };
                        }
                        Mkblock(wc, body, blkSize);
                    }
                }
                catch (Exception ee)
                {
                    if (i == (putSetting.TryTimes - 1))
                    {
                        throw ee;
                    }
                    System.Threading.Thread.Sleep(1000);
                    needretry = true;
                }
                done.WaitOne();
                if (needretry) { continue; }
                if (extra.Progresses[blkIdex] == null || crc32 != extra.Progresses[blkIdex].crc32)
                {
                    if (i == (putSetting.TryTimes - 1)) { return null; }
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


        private void Mkblock(WebClient client, byte[] firstChunk, int blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);
            client.Headers.Add("Content-Type", "application/octet-stream");
            client.UploadDataAsync(new Uri(url), "POST", firstChunk);
        }

        private BlkputRet Mkblock(Client client, byte[] firstChunk, int blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);

            CallRet callRet = client.CallWithBinary(url, "application/octet-stream", new MemoryStream(firstChunk, 0, blkSize), blkSize);
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

