using System.Collections.Generic;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 断点续上传断点信息
    /// </summary>
    public class ResumeInfo
    {
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 当前块编号
        /// </summary>
        public int BlockIndex { get; set; }

        /// <summary>
        /// 文件块总数
        /// </summary>
        public int BlockCount { get; set; }

        /// <summary>
        /// 上下文信息列表
        /// </summary>
        public string[] Contexts { get; set; }

        /// <summary>
        /// 上传数据流时使用
        /// </summary>
        public List<string> SContexts { get; set; }

    }
}
