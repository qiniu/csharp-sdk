using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qiniu.RS;
using Qiniu.IO;
using Qiniu.Util;
using Qiniu.RPC;

namespace Qiniu.Test
{
	public class RSHelper
	{
		public RSHelper ()
		{
		}
		public static void RSDel(string bucket,string key)
		{
			RSClient target = new RSClient(); // TODO: 初始化为适当的值
			EntryPath scope = new EntryPath(bucket,key); // TODO: 初始化为适当的值       
			CallRet actual;
			actual = target.Delete(scope);			   
		}
		public static List<string> RSPut(string bucket,int num)
		{
			IOClient target = new IOClient(); 
			string key = "csharp" + Guid.NewGuid().ToString();
			//PrintLn(key);
			PutExtra extra = new PutExtra(); // TODO: 初始化为适当的值
			extra.MimeType = "text/plain";
			extra.Scope = bucket;
			PutPolicy put = new PutPolicy(extra.Scope);

			List<string> newKeys=new List<string>();
			for (int i=0; i<num; i++) {
				key = "csharp" + Guid.NewGuid ().ToString ();
				PutRet ret = target.Put (put.Token (), key, "Hello, Qiniu Cloud!".ToStream (), extra);
				if (ret.OK) {
					newKeys.Add (key);
				}
			
			}
			return newKeys;
		}
	}
}

