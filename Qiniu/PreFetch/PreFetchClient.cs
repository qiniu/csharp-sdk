using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;
namespace Qiniu.RSF
{
	/// <summary>
	/// RS Fetch 
	/// </summary>
	public class PreFetchClient : QiniuAuthClient
	{        

		/// <summary>
		/// Pres the fetch.
		/// </summary>
		/// <returns><c>true</c>, if fetch was pred, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public  bool PreFetch(EntryPath path){
			string url = Config.PREFETCH_HOST + "/prefetch/" + path.Base64EncodedURI;
			CallRet ret = Call (url);
			return ret.OK;
		}
	}
}

