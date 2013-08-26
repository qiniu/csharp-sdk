using System;
using System.Net;
using System.IO;
using Qiniu.RPC;

namespace Qiniu.Auth
{
	public class PutAuthClient : Client
	{
		public string UpToken { get; set; }

		public PutAuthClient (string upToken)
		{
			UpToken = upToken;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <param name="body"></param>
		public override void SetAuth (HttpWebRequest request, Stream body)
		{
			string authHead = "UpToken " + UpToken;
			request.Headers.Add ("Authorization", authHead);
		}
	}
}
