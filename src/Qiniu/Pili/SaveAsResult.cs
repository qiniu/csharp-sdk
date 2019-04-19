using Newtonsoft.Json;
using Qiniu.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Pili
{
    /// <summary>
    /// 录制直播回放-结果
    /// </summary>
    public class SaveAsResult : HttpResult
    {
        /// <summary>
        /// 获取带宽信息
        /// </summary>
        public SaveAsInfo Result
        {
            get
            {
                SaveAsInfo info = null;
                if ((Code == (int)HttpCode.OK) && (!string.IsNullOrEmpty(Text)))
                {
                    info = JsonConvert.DeserializeObject<SaveAsInfo>(Text);
                }
                return info;
            }
        }

        /// <summary>
        /// 转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code:{0}\n", Code);

            sb.AppendLine();

            if (Result != null)
            {
                sb.AppendLine("result:");
                sb.AppendFormat("code:{0}\n", Result.Code);
                if (!string.IsNullOrEmpty(Result.Error))
                {
                    sb.AppendFormat("error:{0}\n", Result.Error);
                }
                if (!string.IsNullOrEmpty(Result.FName))
                {
                    sb.AppendFormat("fname:{0}\n", Result.FName);
                }
                if (!string.IsNullOrEmpty(Result.PersistentID))
                {
                    sb.AppendFormat("persistentID:{0}\n", Result.PersistentID);
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

            sb.AppendFormat("ref-code:{0}\n", RefCode);

            if (!string.IsNullOrEmpty(RefText))
            {
                sb.AppendLine("ref-text:");
                sb.AppendLine(RefText);
            }

            if (RefInfo != null)
            {
                sb.AppendFormat("ref-info:\n");
                foreach (var d in RefInfo)
                {
                    sb.AppendLine(string.Format("{0}:{1}", d.Key, d.Value));
                }
            }

            return sb.ToString();
        }
    }
}