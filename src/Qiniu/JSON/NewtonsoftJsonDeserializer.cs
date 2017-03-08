using Newtonsoft.Json; 

namespace Qiniu.JSON
{
    /// <summary>
    /// 默认的JSON反序列化：基于Json.NET实现
    /// </summary>
    public class NewtonsoftJsonDeserializer:IJsonDeserializer
    {
        /// <summary>
        /// 实现接口：JSON反序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Deserialize<T>(string str, out T obj) where T : new()
        {
            obj = default(T);

            bool ok = true;

            try
            {
                obj = JsonConvert.DeserializeObject<T>(str);
            }
            catch(System.Exception)
            {
                ok = false;
            }

            return ok;
        }
    }
}
