using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.CDN
{
    /// <summary>
    ///     文件预取-结果
    /// </summary>
    public class PrefetchResult : HttpResult
    {
        /// <summary>
        ///     获取文件预取信息
        /// </summary>
        public PrefetchInfo Result
        {
            get
            {
                PrefetchInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<PrefetchInfo>(Text);
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

                if (!string.IsNullOrEmpty(Result.RequestId))
                {
                    sb.AppendLine($"requestId:{Result.RequestId}");
                }

                if (Result.InvalidUrls != null && Result.InvalidUrls.Count > 0)
                {
                    sb.Append("invalidUrls:");
                    foreach (var s in Result.InvalidUrls) sb.Append(s + " ");
                }

                sb.AppendLine();
                sb.AppendLine($"quotaDay:{Result.QuotaDay}");
                sb.AppendLine($"surplusaDay:{Result.SurplusDay}");
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
