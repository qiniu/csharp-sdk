using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.CDN
{
    /// <summary>
    ///     查询流量-结果
    /// </summary>
    public class FluxResult : HttpResult
    {
        /// <summary>
        ///     获取流量信息
        /// </summary>
        public FluxInfo Result
        {
            get
            {
                FluxInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<FluxInfo>(Text);
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

                if (Result.Time != null)
                {
                    sb.Append("time:");
                    foreach (var t in Result.Time) sb.Append(t + " ");
                    sb.AppendLine();
                }

                if (Result.Data != null && Result.Data.Count > 0)
                {
                    sb.Append("flux:");
                    foreach (var kvp in Result.Data)
                    {
                        sb.AppendLine($"{kvp.Key}:");
                        sb.AppendLine($"China: {kvp.Value.China}, Oversea={kvp.Value.Oversea}");
                    }

                    sb.AppendLine();
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

            sb.AppendLine($"ref-code:{RefCode}");

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
