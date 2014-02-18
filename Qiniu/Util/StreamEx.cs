using System;
using System.IO;

namespace Qiniu.Util
{
	public static class StreamEx
	{
		/// <summary>
		/// string To Stream
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static Stream ToStream (string str)
		{
			Stream s = new MemoryStream (Conf.Config.Encoding.GetBytes (str));
			return s;
		}
	}
}
