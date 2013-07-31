using System;
using System.Collections.Generic;
using NUnit.Framework;
using Qiniu.RS;
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
	}
}

