namespace Qiniu.JSON
{
    /// <summary>
    /// JSON序列化接口
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// JSON序列化方法
        /// </summary>
        /// <param name="obj">输入的对象</param>
        /// <returns>JSON编码的字符串</returns>
        string Serialize<T>(T obj) where T : new();            
    }
}
