using System;
using Newtonsoft.Json;

namespace Qiniu.IO.Resumable
{
	[JsonObject(MemberSerialization.OptIn)]
	public class BlkputRet
	{
		[JsonProperty("ctx")]
		public string ctx;
		[JsonProperty("checksum")]
		public string checkSum;
		[JsonProperty("crc32")]
		public UInt32 crc32;
		[JsonProperty("offset")]
		public ulong offset;
	}
}
