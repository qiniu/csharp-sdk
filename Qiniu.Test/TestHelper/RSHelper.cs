using System;
using System.Collections.Generic;
using Qiniu.RS;
using Qiniu.IO;
using Qiniu.Util;
using Qiniu.RPC;
#if NET20 || NET40
using NUnit.Framework;
#else
using Xunit;
using System.Threading.Tasks;
#endif

namespace Qiniu.Test
{
	public class RSHelper
	{
		public RSHelper ()
		{
		}

#if NET20 || NET40
		public static void RSDel(string bucket,string key)
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath scope = new EntryPath(bucket,key); // TODO: 初始化为适当的值       
			CallRet actual;
			actual = target.Delete(scope);			   
		}
#else
        public static async Task RSDel(string bucket, string key)
        {
            RSClient target = new RSClient(); // TODO: 初始化为适当的值
            EntryPath scope = new EntryPath(bucket, key); // TODO: 初始化为适当的值       
            CallRet actual;
            actual = await target.DeleteAsync(scope);
        }
#endif

#if NET20 || NET40
        public static List<string> RSPut(string bucket,int num)
#else
        public static async Task<List<string>> RSPut(string bucket, int num)
#endif
        {
			IOClient target = new IOClient(); 
			string key = "csharp" + Guid.NewGuid().ToString();
			//PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			PutPolicy put = new PutPolicy(bucket);
			List<string> newKeys=new List<string>();
			for (int i=0; i<num; i++) {
				key = "csharp" + Guid.NewGuid ().ToString ();
#if NET20 || NET40
				PutRet ret = target.Put (put.Token (), key,StreamEx.ToStream("Hello, Qiniu Cloud!"), extra);
#else
                PutRet ret = await target.PutAsync(put.Token(), key, StreamEx.ToStream("Hello, Qiniu Cloud!"), extra);
#endif
                if (ret.OK) {
					newKeys.Add (key);
				}
			
			}
			return newKeys;
		}
	}
}

