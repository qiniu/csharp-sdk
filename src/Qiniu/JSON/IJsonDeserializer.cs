namespace Qiniu.JSON
{
    /// <summary>
    /// JSON反序列化接口
    /// </summary>
    public interface IJsonDeserializer
    {
        /// <summary>
        /// JSON反序列化方法
        /// </summary>
        /// <param name="str">输入JSON编码的字符串</param>
        /// <param name="obj">输出目标对象</param>
        /// <returns>成功时返回true，否则false</returns>
        bool Deserialize<T>(string str, out T obj) where T:new();
    }
}
