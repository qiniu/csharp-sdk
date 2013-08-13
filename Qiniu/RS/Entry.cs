using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Qiniu.RPC;

namespace Qiniu.RS
{
	public class Entry : CallRet
	{
		public string Hash { get; private set; }

		public long Fsize { get; private set; }

		public long PutTime { get; private set; }

		public string MimeType { get; private set; }

		public string Customer { get; private set; }

		public Entry (CallRet ret)
            : base(ret)
		{
			if (OK && string.IsNullOrEmpty (Response)) {
				try {
					Unmarshal (Response);
				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
					this.Exception = e;
				}
			}
		}

		private void Unmarshal (string json)
		{
			var dict = JsonConvert.DeserializeObject<Dictionary<string,dynamic>> (json);
			if (dict != null) {
				dynamic tmp;
				if (dict.TryGetValue ("hash", out tmp)) {
					Hash = (string)tmp;
				}
				if (dict.TryGetValue ("mimeType", out tmp)) {
					MimeType = (string)tmp;
				}
				if (dict.TryGetValue ("fsize", out tmp)) {
					Fsize = (long)tmp;
				}
				if (dict.TryGetValue ("putTime", out tmp)) {
					PutTime = (long)tmp;
				}
				if (dict.TryGetValue ("customer", out tmp)) {
					Customer = (string)tmp;
				}
			}
		}
	}
}
