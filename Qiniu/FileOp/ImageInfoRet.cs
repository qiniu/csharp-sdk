using System;
using System.Collections.Generic;
using Qiniu.RPC;
using Newtonsoft.Json;

namespace Qiniu.FileOp
{
	public class ImageInfoRet : CallRet
	{
		public int Width { get; private set; }

		public int Height { get; private set; }

		public string Format { get; private set; }

		public string ColorModel { get; private set; }

		public ImageInfoRet (CallRet ret)
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
			var dics = JsonConvert.DeserializeObject<Dictionary<string,dynamic>> (json);
			dynamic tmp;
			if (dics.TryGetValue ("format", out tmp)) {
				Format = (string)tmp;
			}
			if (dics.TryGetValue ("width", out tmp)) {
				Width = (int)tmp;
			}
			if (dics.TryGetValue ("height", out tmp)) {
				Height = (int)tmp;
			}
			if (dics.TryGetValue ("colorModel", out tmp)) {
				ColorModel = (string)tmp;
			}
		}
	}
}
