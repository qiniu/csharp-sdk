using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Qiniu.Conf;
using Qiniu.RPC;

namespace Qiniu.IO
{
	class MultiPart
	{

		#region Events
		/// <summary>
		/// Occurs when upload progress changed when call AsyncMultiPost
		/// </summary>
		public event EventHandler<UploadProgressChangedEventArgs> UploadProgressChanged ;

		protected void onUploadProgressChanged(object sender,UploadProgressChangedEventArgs e){
			if (this.UploadProgressChanged != null) {
				this.UploadProgressChanged (sender, e);
			}
		}

		/// <summary>
		/// Occurs when upload completed when call AsyncMultiPost
		/// </summary>
		public event EventHandler<UploadDataCompletedEventArgs> UploadCompleted;

		protected void onUploadCompleted(object sender,UploadDataCompletedEventArgs e){
			if (this.UploadCompleted != null) {
				this.UploadCompleted (sender, e);
			}
		}

		#endregion

		public static Encoding encoding = Config.Encoding;

		public static string RandomBoundary ()
		{
			return String.Format ("----------{0:N}", Guid.NewGuid ());
		}

		public static string FormDataContentType (string boundary)
		{
			return "multipart/form-data; boundary=" + boundary;
		}

		private Stream GetPostStream (Stream putStream, string fileName, NameValueCollection formData, string boundary)
		{
			Stream postDataStream = new System.IO.MemoryStream ();

			//adding form data

			string formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
				"Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}";

			foreach (string key in formData.Keys) {
				byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes (string.Format (formDataHeaderTemplate,
				                                                                                    key, formData [key]));
				postDataStream.Write (formItemBytes, 0, formItemBytes.Length);
			}

			//adding file,Stream data
			#region adding file data
           
			string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
				"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
				Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine;
			byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes (string.Format (fileHeaderTemplate,
			                                                                                   "file", fileName));
			postDataStream.Write (fileHeaderBytes, 0, fileHeaderBytes.Length);
           
			byte[] buffer = new byte[1024];
			int bytesRead = 0;
			while ((bytesRead = putStream.Read(buffer, 0, buffer.Length)) != 0) {
				postDataStream.Write (buffer, 0, bytesRead);
			}
			putStream.Close ();
			#endregion

			#region adding end
			byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes (Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
			postDataStream.Write (endBoundaryBytes, 0, endBoundaryBytes.Length);
			#endregion

			return postDataStream;
 
		}

		private Stream GetPostStream (string filePath, NameValueCollection formData, string boundary)
		{
			Stream postDataStream = new System.IO.MemoryStream ();

			//adding form data

			string formDataHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
				"Content-Disposition: form-data; name=\"{0}\";" + Environment.NewLine + Environment.NewLine + "{1}";

			foreach (string key in formData.Keys) {
				byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes (string.Format (formDataHeaderTemplate,
				                                                                                    key, formData [key]));
				postDataStream.Write (formItemBytes, 0, formItemBytes.Length);
			}

			//adding file data
			#region adding file data
			FileInfo fileInfo = new FileInfo (filePath);
			string fileHeaderTemplate = Environment.NewLine + "--" + boundary + Environment.NewLine +
				"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
				Environment.NewLine + "Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine;
			byte[] fileHeaderBytes = System.Text.Encoding.UTF8.GetBytes (string.Format (fileHeaderTemplate,
			                                                                                   "file", fileInfo.FullName));
			postDataStream.Write (fileHeaderBytes, 0, fileHeaderBytes.Length);
			FileStream fileStream = fileInfo.OpenRead ();
			byte[] buffer = new byte[1024];
			int bytesRead = 0;
			while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0) {
				postDataStream.Write (buffer, 0, bytesRead);
			}
			fileStream.Close ();
			#endregion

			#region adding end
			byte[] endBoundaryBytes = System.Text.Encoding.UTF8.GetBytes (Environment.NewLine + "--" + boundary + "--" + Environment.NewLine);
			postDataStream.Write (endBoundaryBytes, 0, endBoundaryBytes.Length);
			#endregion

			return postDataStream;
		}

		public CallRet MultiPost (string url, NameValueCollection formData, string fileName)
		{
			string boundary = RandomBoundary ();
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create (url);

			webRequest.Method = "POST";
			webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
			FileInfo fileInfo = new FileInfo (fileName);

			using (FileStream fileStream = fileInfo.OpenRead()) {

				Stream postDataStream = GetPostStream (fileStream, fileName, formData, boundary);
				webRequest.ContentLength = postDataStream.Length;
				Stream reqStream = webRequest.GetRequestStream ();
				postDataStream.Position = 0;

				byte[] buffer = new byte[1024];
				int bytesRead = 0;

				while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0) {
					reqStream.Write (buffer, 0, bytesRead);
				}
				postDataStream.Close ();
				reqStream.Close ();
			}
			try {
				using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse) {                   
					return RPC.Client.HandleResult (response);
				}
               
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return new CallRet (HttpStatusCode.BadRequest, e);
			}            
		}

		/// <summary>
		/// Asyncs the multi post.DO NOT USING IT TO UPLOAD BIG FILE!!! IT WILL USE HUGE OF MEMORY!!!
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="formData">Form data.</param>
		/// <param name="inputStream">Input stream.</param>
		public void AsyncMultiPost (string url, NameValueCollection formData, string filename){ 

			FileInfo fileInfo = new FileInfo (filename);

			string boundary = RandomBoundary ();
			using (FileStream fileStream = fileInfo.OpenRead())
			using (WebClient client = new WebClient ())
			using (Stream postDataStream = GetPostStream (fileStream, formData ["key"], formData, boundary)) {
				client.Headers.Add ("Content-Type", "multipart/form-data; boundary=" + boundary);
				byte[] hugeBuffer = new byte[postDataStream.Length];
				postDataStream.Seek (0, SeekOrigin.Begin);
				postDataStream.Read (hugeBuffer, 0, (int)postDataStream.Length);

				client.UploadProgressChanged += new UploadProgressChangedEventHandler ((sender, e) => {
					onUploadProgressChanged(sender,e);
				});
				client.UploadDataCompleted += new UploadDataCompletedEventHandler ((sender, e) => {
					onUploadCompleted(sender,e);
				});
				client.UploadDataAsync (new Uri(url), "POST", hugeBuffer);
			}	
		}

		public CallRet MultiPost (string url, NameValueCollection formData, System.IO.Stream inputStream)
		{
			string boundary = RandomBoundary ();
			System.Net.WebRequest webRequest = System.Net.WebRequest.Create (url);

			webRequest.Method = "POST";
			webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
		
			Stream postDataStream = GetPostStream (inputStream, formData ["key"], formData, boundary);

			webRequest.ContentLength = postDataStream.Length;
			Stream reqStream = webRequest.GetRequestStream ();
			postDataStream.Position = 0;

			byte[] buffer = new byte[1024];
			int bytesRead = 0;

			while ((bytesRead = postDataStream.Read(buffer, 0, buffer.Length)) != 0) {
				reqStream.Write (buffer, 0, bytesRead);
			}
			postDataStream.Close ();
			reqStream.Close ();

			try {
				using (HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse) {                   
					return RPC.Client.HandleResult (response);
				}

			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return new CallRet (HttpStatusCode.BadRequest, e);
			}            
		}
	}
}
