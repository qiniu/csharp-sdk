using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Qiniu.JSON
{
    /// <summary>
    /// 默认的JSON序列化：基于Json.NET实现
    /// </summary>
    public class NewtonsoftJsonSerializer:IJsonSerializer
    {
        /// <summary>
        /// 实现接口：JSON序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj) where T : new()
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj,settings);
        }
    }
}
