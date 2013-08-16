using System;
using System.Collections.Generic;
using Qiniu.RPC;
using Newtonsoft.Json;

namespace Qiniu.FileOp
{
	public class ExifValType
	{
		public string val { get; set; }

		public int type { get; set; }
	}

	public class ExifRet : CallRet
	{
		private Dictionary<string, ExifValType> dict;

		public ExifValType this [string key] {
			get {
				return dict [key];
			}
		}

		public ExifRet (CallRet ret)
            : base(ret)
		{
			if (!String.IsNullOrEmpty (Response)) {
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
			dict = JsonConvert.DeserializeObject<Dictionary<string,ExifValType>> (json);
		}

		public override string ToString ()
		{
			try {
				return JsonConvert.SerializeObject (dict).ToString ();
			} catch {
				return string.Empty;
			}
		}
	}
}
