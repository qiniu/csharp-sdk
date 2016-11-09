using System.Collections.Generic;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 断点续上传断点信息
    /// </summary>
    public class ResumeInfo
    {
        public long FileSize { get; set; }

        public int BlockIndex { get; set; }

        public int BlockCount { get; set; }

        public string[] Contexts { get; set; }
    }
}
