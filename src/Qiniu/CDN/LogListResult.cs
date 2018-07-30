using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.CDN
{
    /// <summary>
    ///     查询日志-结果
    /// </summary>
    public class LogListResult : HttpResult
    {
        /// <summary>
        ///     获取日志列表信息
        /// </summary>
        public LogListInfo Result
        {
            get
            {
                LogListInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<LogListInfo>(Text);
                }

                return info;
            }
        }

        /// <summary>
        ///     转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"code:{Code}");
            sb.AppendLine();

            if (Result != null)
            {
                sb.AppendLine("result:");
                sb.AppendLine($"code:{Result.Code}");
                if (!string.IsNullOrEmpty(Result.Error))
                {
                    sb.AppendLine($"error:{Result.Error}");
                }

                if (Result.Data != null && Result.Data.Count > 0)
                {
                    sb.AppendLine("log:");
                    foreach (var key in Result.Data.Keys)
                    {
                        sb.AppendLine($"{key}:");
                        foreach (var d in Result.Data)
                        {
                            if (d.Value != null)
                            {
                                sb.AppendLine($"Domain:{d.Key}");
                                foreach (var s in d.Value)
                                {
                                    if (s != null)
                                    {
                                        sb.AppendLine($"Name:{s.Name}");
                                        sb.AppendLine($"Size:{s.Size}");
                                        sb.AppendLine($"Mtime:{s.Mtime}");
                                        sb.AppendLine($"Url:{s.Url}");
                                        sb.AppendLine();
                                    }
                                }
                            }
                        }

                        sb.AppendLine();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Text))
                {
                    sb.AppendLine("text:");
                    sb.AppendLine(Text);
                }
            }

            sb.AppendLine();

            sb.Append($"ref-code:{RefCode}");

            if (!string.IsNullOrEmpty(RefText))
            {
                sb.AppendLine("ref-text:");
                sb.AppendLine(RefText);
            }

            if (RefInfo != null)
            {
                sb.AppendLine("ref-info:");
                foreach (var d in RefInfo) sb.AppendLine($"{d.Key}:{d.Value}");
            }

            return sb.ToString();
        }
    }
}
