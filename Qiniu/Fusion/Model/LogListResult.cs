using System.Collections.Generic;
using System.Text;

namespace Qiniu.Fusion.Model
{
    public class LogListResult
    {
        public int Code { get; set; }

        public string Error { get; set; }

        public Dictionary<string,List<LogData>> Data { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code:{0}\n", Code);
            sb.AppendFormat("error:{0}\n", Error);
            sb.Append("logs:\n");
            if (Data != null)
            {
                foreach (var key in Data.Keys)
                {
                    sb.AppendFormat("{0}:\n", key);
                    foreach (var s in Data[key])
                    {
                        sb.Append(s + " ");
                    }
                    sb.Append("\n");
                }
            }

            return sb.ToString();
        }
    }

    public class LogData
    {
        // 文件名
        public string Name { get; set; }

        // 文件大小，单位为 Byte
        public long Size { get; set; }

        // 文件修改时间，Unix 时间戳
        public long Mtime { get; set; }

        // 下载链接
        public string Url { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("name:{0}\n", Name);
            sb.AppendFormat("size:{0}\n", Size);
            sb.AppendFormat("mtime:{0}\n", Mtime);
            sb.AppendFormat("url:{0}\n", Url);

            return sb.ToString();
        }
    }
}
