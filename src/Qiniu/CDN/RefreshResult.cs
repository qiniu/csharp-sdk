using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.CDN
{
    /// <summary>
    ///     缓存刷新-结果
    /// </summary>
    public class RefreshResult : HttpResult
    {
        /// <summary>
        ///     获取缓存刷新信息
        /// </summary>
        public RefreshInfo Result
        {
            get
            {
                RefreshInfo info = null;
                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    info = JsonConvert.DeserializeObject<RefreshInfo>(Text);
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

                if (Result.InvalidDirs != null && Result.InvalidDirs.Count > 0)
                {
                    sb.Append("invalidDirs:");
                    foreach (var s in Result.InvalidDirs) sb.Append(s + " ");
                    sb.AppendLine();
                }

                if (Result.InvalidUrls != null && Result.InvalidUrls.Count > 0)
                {
                    sb.Append("invalidUrls:");
                    foreach (var s in Result.InvalidUrls) sb.Append(s + " ");
                    sb.AppendLine();
                }

                sb.AppendLine($"dirQuotaDay:{Result.DirQuotaDay}");
                sb.AppendLine($"dirSurplusDay:{Result.DirSurplusDay}");
                sb.AppendLine($"urlQuotaDay:{Result.UrlQuotaDay}");
                sb.AppendLine($"urlSurplusDay:{Result.UrlSurplusDay}");
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
