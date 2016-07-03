using System;
using Qiniu.RPC;
#if ABOVE45
using System.Threading.Tasks;
#endif

namespace Qiniu.FileOp
{
	public static class Exif
	{
		public static string MakeRequest (string url)
		{
			return url + "?exif";
		}

#if !ABOVE45
        public static ExifRet Call (string url)
		{
			CallRet callRet = FileOpClient.Get (url);
			return new ExifRet (callRet);
		}
#else
        public static async Task<ExifRet> CallAsync(string url)
        {
            CallRet callRet = await FileOpClient.GetAsync(url);
            return new ExifRet(callRet);
        }
#endif
	}
}
