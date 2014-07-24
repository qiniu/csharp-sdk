using System;
using System.Collections.Generic;
using Qiniu.RPC;
using Newtonsoft.Json;

namespace Qiniu.IO
{
	public class PutRet : CallRet
	{
		/// <summary>
		/// 上传结果的原始JOSN字符串。
		/// </summary>
		/// <value>The raw string.</value>
		public string RawString { get; private set;}

		/// <summary>
		/// 如果 uptoken 没有指定 ReturnBody，那么返回值是标准的 PutRet 结构
		/// </summary>
		public string Hash { get; private set; }

		/// <summary>
		/// 如果传入的 key == UNDEFINED_KEY，则服务端返回 key
		/// </summary>
		public string key { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.IO.PutRet"/> class.
		/// </summary>
		/// <param name="ret">Ret.</param>
		public PutRet (CallRet ret) : base(ret)
		{
			if (!String.IsNullOrEmpty (Response)) {
				this.RawString = Response;
				try {
					Unmarshal (Response);
				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
					this.Exception = e;
				}
			}
		}	

		/// <summary>
		/// Initializes a new instance of the <see cref="Qiniu.IO.PutRet"/> class.
		/// </summary>
		/// <param name="result">Result.</param>
		public PutRet(string result){
			this.RawString = result;
			if (!string.IsNullOrEmpty (result)) {
				Unmarshal (result);
			}
		}

        private void Unmarshal(string json)
        {
            try
            {
                Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                object tmp;
                if (dict.TryGetValue("hash", out tmp))
                    Hash = (string)tmp;
                if (dict.TryGetValue("key", out tmp))
                    key = (string)tmp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }	
	}
}
