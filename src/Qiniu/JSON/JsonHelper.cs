namespace Qiniu.JSON
{
    /// <summary>
    /// 公用JSON序列化、反序列化
    /// </summary>
    public sealed class JsonHelper
    {
        /// <summary>
        /// 请设置您的JSON序列化器
        /// </summary>
        public static IJsonSerializer JsonSerializer = new NewtonsoftJsonSerializer();

        /// <summary>
        /// 请设置您的JSON反序列化器
        /// </summary>
        public static IJsonDeserializer JsonDeserializer = new NewtonsoftJsonDeserializer();

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string Serialize<T>(T obj) where T:new()
        {
            return JsonSerializer.Serialize<T>(obj);

        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static bool Deserialize<T>(string jstr,out T obj) where T : new()
        {
            return JsonDeserializer.Deserialize<T>(jstr, out obj);

        }
    }

}