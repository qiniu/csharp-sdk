using System;
using Newtonsoft.Json;

namespace Qiniu.JSON
{
    /// <summary>
    /// JSON序列化/反序列化
    /// 这部分可以根据具体需要自行实现
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// JSON反序列化
        /// </summary>
        /// <typeparam name="T">输出的Object类型</typeparam>
        /// <param name="str">输入的JSON字符串</param>
        /// <param name="obj">输出Object对象</param>
        /// <returns>成功与否</returns>
        public static bool Deserialize<T>(string str,out T obj) where T:new()
        {
            bool ok = true;
            obj = new T();

            try
            {
                obj = JsonConvert.DeserializeObject<T>(str);
            }
            catch(Exception)
            {
                ok = false;
            }

            return ok;
        }

    }
}