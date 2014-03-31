using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using Qiniu.Conf;
using Qiniu.Auth;
using Qiniu.RPC;
using Qiniu.Util;
using System.Collections.Specialized;

namespace Qiniu.IO
{

	/// <summary>
	/// Upload progress event arguments.
	/// </summary>
	public class PutProgressEventArgs:EventArgs{
		private int percentage;

		/// <summary>
		/// Gets the percentage.
		/// </summary>
		/// <value>The percentage.</value>
		public int Percentage {
			get {
				return percentage;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.IO.UploadProgressEventArgs"/> class.
		/// </summary>
		/// <param name="percentage">Percentage.</param>
		public PutProgressEventArgs(int percentage){
			this.percentage = percentage;
		}
	} 

	/// <summary>
	/// Upload failed event arguments.
	/// </summary>
	public class PutFailedEventArgs:EventArgs{
		private Exception error;

		/// <summary>
		/// Gets the exception.
		/// </summary>
		/// <value>The exception.</value>
		public Exception Error{
			get {
				return this.error;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.IO.UploadFailedEventArgs"/> class.
		/// </summary>
		/// <param name="msg">Message.</param>
		public PutFailedEventArgs(Exception e){
			this.error = e ;
		}	
	}
		
    /// <summary>
    /// 上传客户端
    /// </summary>
    public class IOClient
    {
		#region Events
		/// <summary>
		/// Occurs when put finished when call AsyncPutFile
		/// </summary>
        public event EventHandler<PutRet> PutFinished;

		protected void onPutFinished(object sender,PutRet e){
			if (this.PutFinished != null) {
				this.PutFinished (sender, e);
			}
		}

		/// <summary>
		/// Occurs when upload progress changed when call AsyncPutFile
		/// </summary>
		public event EventHandler<PutProgressEventArgs> PutProgressChanged;

		protected void onPutProgressChanged(object sender,PutProgressEventArgs percentage){
			if(this.PutProgressChanged!=null){
				this.PutProgressChanged (sender, percentage);
			}
		}

		/// <summary>
		/// Occurs when upload failed when call AsyncPutFile
		/// </summary>
		public event EventHandler<PutFailedEventArgs> PutFailed;

		protected void onPutFailed(object sender,PutFailedEventArgs e){
			if (this.PutFailed != null) {
				this.PutFailed (sender, e);
			}
		}
		#endregion

        private static NameValueCollection getFormData(string upToken, string key, PutExtra extra)
        {
            NameValueCollection formData = new NameValueCollection();
            formData["token"] = upToken;
            formData["key"] = key;
            if (extra != null)
            {
                if (extra.CheckCrc == CheckCrcType.CHECK_AUTO)
                {
                    formData["crc32"] = extra.Crc32.ToString();
                }
                if (extra.Params != null)
                {
                    foreach (KeyValuePair<string, string> pair in extra.Params)
                    {
                        formData[pair.Key] = pair.Value;
                    }
                }
            }
            return formData;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upToken"></param>
        /// <param name="key"></param>h
        /// <param name="localFile"></param>
        /// <param name="extra"></param>
        public PutRet PutFile(string upToken, string key, string localFile, PutExtra extra)
        {
            if (!System.IO.File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }

            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
				CallRet callRet = new MultiPart().MultiPost(Config.UP_HOST, formData, localFile);
				return  new PutRet(callRet);
            }
            catch (Exception e)
            {
				return new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
            }
        }

		/// <summary>
		/// Asyncs put file.
		/// </summary>
		/// <param name="upToken">Up token.</param>
		/// <param name="key">Key.</param>
		/// <param name="localFile">Local file.</param>
		/// <param name="extra">Extra.</param>
		public void AsyncPutFile(string upToken, string key, string localFile, PutExtra extra){
			if (!System.IO.File.Exists(localFile)) {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }

            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
				MultiPart multi =new MultiPart();
				multi.UploadProgressChanged += new EventHandler<UploadProgressChangedEventArgs>((sender,e)=>{
					int per = (int)(100 * e.BytesSent/e.TotalBytesToSend);
					onPutProgressChanged(this,new PutProgressEventArgs(per));
				});
				multi.UploadCompleted += new EventHandler<UploadDataCompletedEventArgs>((sender, e) => {
					if(e.Error!=null){
						WebException we = (WebException)e.Error;
						onPutFailed(this,new PutFailedEventArgs(we));
						return;
					}
					onPutFinished(this, new PutRet(Encoding.UTF8.GetString(e.Result)));
				});
				multi.AsyncMultiPost(Config.UP_HOST,formData,localFile);
            }
            catch (Exception e)
            {
				onPutFailed(this,new PutFailedEventArgs(e));
            }
		}

        /// <summary>
        /// Puts the file without key.
        /// </summary>
        /// <returns>The file without key.</returns>
        /// <param name="upToken">Up token.</param>
        /// <param name="localFile">Local file.</param>
        /// <param name="extra">Extra.</param>
        public PutRet PutFileWithoutKey(string upToken, string localFile, PutExtra extra)
        {
            return PutFile(upToken, string.Empty, localFile, extra);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="upToken">Up token.</param>
        /// <param name="key">Key.</param>
        /// <param name="putStream">Put stream.</param>
        /// <param name="extra">Extra.</param>
        public PutRet Put(string upToken, string key, System.IO.Stream putStream, PutExtra extra)
        {
            if (!putStream.CanRead)
            {
                throw new Exception("read put Stream error");
            }
            PutRet ret;
            NameValueCollection formData = getFormData(upToken, key, extra);
            try
            {
				CallRet callRet = new MultiPart().MultiPost(Config.UP_HOST, formData, putStream);
                ret = new PutRet(callRet);
                return ret;
            }
            catch (Exception e)
            {
                ret = new PutRet(new CallRet(HttpStatusCode.BadRequest, e));
                return ret;
            }
        }
    }
}
