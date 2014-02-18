using Newtonsoft.Json;


namespace Qiniu.Util
{
	public static class QiniuJsonHelper
	{
		public static string JsonEncode (object obj)
		{
			JsonSerializerSettings setting = new JsonSerializerSettings ();
			setting.NullValueHandling = NullValueHandling.Ignore;
			return JsonConvert.SerializeObject (obj, setting);
		}

		public static T ToObject<T> (string value)
		{
			return JsonConvert.DeserializeObject<T> (value);
		}
	}
}
