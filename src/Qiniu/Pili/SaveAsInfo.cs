using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Pili
{
    /// <summary>
    /// 录制直播回放-消息内容结果
    /// </summary>
    public class SaveAsInfo
    {
        /// <summary>
        /// 代码 含义 说明
        /// 200	success	成功(OK)
        /// 612	stream not found    
        /// 619	no data 没有直播数据
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误消息(状态码非OK时)
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 保存后在存储空间里的文件名
        /// </summary>
        public string FName { get; set; }

        /// <summary>
        /// 持久化异步处理任务ID，异步模式才会返回该字段，可以通过该字段查询转码进度
        /// </summary>
        public string PersistentID { get; set; }
    }
}