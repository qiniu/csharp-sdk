using System;
using System.Collections.Specialized;

namespace Qiniu.IO.Resumable
{
	/// <summary>
	/// Block上传成功事件参数
	/// </summary>
	public class PutNotifyEvent:EventArgs
	{
		int blkIdx;

		public int BlkIdx {
			get { return blkIdx; }
		}

		int blkSize;

		public int BlkSize {
			get { return blkSize; }
		}

		BlkputRet ret;

		public BlkputRet Ret {
			get { return ret; }           
		}

		public PutNotifyEvent (int blkIdx, int blkSize, BlkputRet ret)
		{         
			this.blkIdx = blkIdx;
			this.blkSize = blkSize;
			this.ret = ret; 
		}
	}

	/// <summary>
	/// 上传错误事件参数
	/// </summary>
	public class PutNotifyErrorEvent:EventArgs
	{
		int blkIdx;

		public int BlkIdx {
			get { return blkIdx; }
		}

		int blkSize;

		public int BlkSize {
			get { return blkSize; }
		}

		string error;

		public string Error {
			get { return error; }           
		}

		public PutNotifyErrorEvent (int blkIdx, int blkSize, string error)
		{
			this.blkIdx = blkIdx;
			this.blkSize = blkSize;
			this.error = error; 
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ResumablePutExtra
	{
		//key format as: "x:var"
		public NameValueCollection CallbackParams;
		public string CustomMeta;
		public string MimeType;
		public int chunkSize;
		public int tryTimes;
		public BlkputRet[] Progresses;

		public event EventHandler<PutNotifyEvent> Notify;
		public event EventHandler<PutNotifyErrorEvent> NotifyErr;

		public void OnNotify (PutNotifyEvent arg)
		{
			if (Notify != null)
				Notify (this, arg);
		}

		public void OnNotifyErr (PutNotifyErrorEvent arg)
		{
			if (NotifyErr != null)
				NotifyErr (this, arg);
		}
	}
}
