using System;

namespace Qiniu.JSON
{
    /// <summary>
    /// 指示JSON序列化时的属性名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute:Attribute
    {
        /// <summary>
        /// 设置JSON序列化的属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 未设置
        /// </summary>
        public JsonPropertyAttribute()
        {

        }

        /// <summary>
        /// 设置JSON序列化的属性名称
        /// </summary>
        /// <param name="propertyName">设置JSON序列化的属性名称</param>
        public JsonPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
