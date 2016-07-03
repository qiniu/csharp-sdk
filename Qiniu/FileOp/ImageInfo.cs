using System;
using Qiniu.RPC;
#if ABOVE45
using System.Threading.Tasks;
#endif

namespace Qiniu.FileOp
{
	public static class ImageInfo
	{
		public static string MakeRequest (string url)
		{
			return url + "?imageInfo";
		}

#if !ABOVE45
        public static ImageInfoRet Call (string url)
		{
			CallRet callRet = FileOpClient.Get (url);
			return new ImageInfoRet (callRet);
		}
#else
        public static async Task<ImageInfoRet> CallAsync(string url)
        {
            CallRet callRet = await FileOpClient.GetAsync(url);
            return new ImageInfoRet(callRet);
        }
#endif
    }
}
