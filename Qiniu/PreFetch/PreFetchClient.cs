using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;
#if ABOVE45
using System.Threading.Tasks;
#endif

namespace Qiniu.PreFetch
{
	/// <summary>
	/// RS Fetch 
	/// </summary>
	public class PreFetchClient : QiniuAuthClient
	{

#if !ABOVE45
        /// <summary>
        /// Pres the fetch.
        /// </summary>
        /// <returns><c>true</c>, if fetch was pred, <c>false</c> otherwise.</returns>
        /// <param name="path">Path.</param>
        public bool PreFetch(EntryPath path){
			string url = Config.PREFETCH_HOST + "/prefetch/" + path.Base64EncodedURI;
			CallRet ret = Call (url);
			return ret.OK;
		}
#else
        public async Task<bool> PreFetchAsync(EntryPath path)
        {
            string url = Config.PREFETCH_HOST + "/prefetch/" + path.Base64EncodedURI;
            CallRet ret = await CallAsync(url);
            return ret.OK;
        }
#endif
    }
}

