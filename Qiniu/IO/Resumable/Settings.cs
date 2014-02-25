
namespace Qiniu.IO.Resumable
{
	/// <summary>
	/// 断点续传上传参数设置
	/// </summary>
	public class Settings
	{
		int chunkSize;

		/// <summary>
		/// chunk大小,默认为4MB;
		/// </summary>
		public int ChunkSize {
			get { return chunkSize; }
			set { chunkSize = value; }
		}

		int tryTimes;

		/// <summary>
		/// 失败重试次数,默认为3
		/// </summary>
		public int TryTimes {
			get { return tryTimes; }
			set { tryTimes = value; }
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="chunkSize">chunk大小,默认为4MB</param>
		/// <param name="tryTimes">失败重试次数,默认为3</param>
		public Settings (int chunkSize=1 << 22, int tryTimes=3)
		{
            //取消chunk
            this.chunkSize = 1 << 22;
			this.tryTimes = tryTimes; 
		}
	}
}
