using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Qiniu.Http;

namespace Qiniu.Storage
{
    /// <summary>
    ///     持久化
    /// </summary>
    public class PfopResult : HttpResult
    {
        /// <summary>
        ///     此ID可用于查询持久化进度
        /// </summary>
        public string PersistentId
        {
            get
            {
                string pid = null;

                if (Code == (int)HttpCode.OK && !string.IsNullOrEmpty(Text))
                {
                    var ret = JsonConvert.DeserializeObject<Dictionary<string, string>>(Text);
                    if (ret.ContainsKey("persistentId"))
                    {
                        pid = ret["persistentId"];
                    }
                }

                return pid;
            }
        }

        /// <summary>
        ///     转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"code: {Code}");

            if (!string.IsNullOrEmpty(PersistentId))
            {
                sb.AppendLine($"PersistentId: {PersistentId}");
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
