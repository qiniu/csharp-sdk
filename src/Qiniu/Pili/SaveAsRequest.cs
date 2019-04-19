using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Pili
{
    /// <summary>
    /// 录制直播回放-请求
    /// </summary>
    public class SaveAsRequest
    {
        /// <summary>
        /// 保存的文件名，不指定系统会随机生成
        /// </summary>
        [JsonProperty("fname")]
        public string FileName { get; set; }
        /// <summary>
        /// 整数，Unix 时间戳，要保存的直播的起始时间，不指定或 0 值表示从第一次直播开始
        /// </summary>
        [JsonProperty("start")]
        public long StartTimestamp { get; set; }
        /// <summary>
        /// 整数，Unix 时间戳，要保存的直播的结束时间，不指定或 0 值表示当前时间
        /// </summary>
        [JsonProperty("end")]
        public long EndTimestamp { get; set; }
        /// <summary>
        /// 保存的文件格式，默认为m3u8，如果指定其他格式，则保存动作为异步模式。详细信息可以参考 转码 的api
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }
        /// <summary>
        /// 异步模式时，数据处理的私有队列，不指定则使用公共队列
        /// </summary>
        [JsonProperty("pipeline")]
        public string Pipeline { get; set; }
        /// <summary>
        /// 异步模式时，保存成功回调通知地址，不指定则不通知
        /// </summary>
        [JsonProperty("notify")]
        public string Notify { get; set; }
        /// <summary>
        /// 更改ts文件的过期时间，默认为永久保存。-1 表示不更改ts文件的生命周期，正值表示修改ts文件的生命周期为expireDays
        /// </summary>
        [JsonProperty("expireDays")]
        public int ExpireDays { get; set; }

        /// <summary>
        /// 初始化(所有成员为空，需要后续赋值)
        /// </summary>
        public SaveAsRequest()
        {
            FileName = "";
            StartTimestamp = 0;
            EndTimestamp = 0;
            Format = "";
            Pipeline = "";
            Notify = "";
            ExpireDays = 0;
        }

        /// <summary>
        /// 初始化所有成员
        /// </summary>
        /// <param name="fname">保存的文件名</param>
        /// <param name="start">要保存的直播的起始时间</param>
        /// <param name="end">要保存的直播的结束时间</param>
        /// <param name="format">保存的文件格式</param>
        /// <param name="pipeline">数据处理的私有队列</param>
        /// <param name="notify">保存成功回调通知地址</param>
        /// <param name="expireDays">更改ts文件的过期时间</param>
        public SaveAsRequest(string fname, long start, long end, string format, string pipeline, string notify, int expireDays)
        {
            FileName = fname;
            StartTimestamp = start;
            EndTimestamp = end;
            Format = format;
            Pipeline = pipeline;
            Notify = notify;
            ExpireDays = expireDays;
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <returns>请求内容的JSON字符串</returns>
        public string ToJsonStr()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}