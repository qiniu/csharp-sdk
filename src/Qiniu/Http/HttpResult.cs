using System.Collections.Generic;
using System.Text;

namespace Qiniu.Http
{
    /// <summary>
    /// (HTTP请求的)返回消息
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// 状态码 (200表示OK)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息或错误，文本格式
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 消息或错误，二进制格式
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 参考代码(用户自定义)
        /// </summary>
        public int RefCode { get; set; }

        /// <summary>
        /// 附加信息(如Exception内容)
        /// </summary>
        public string RefText { get; set; }

        /// <summary>
        /// 参考信息(从返回消息的头部获取)
        /// </summary>
        public Dictionary<string, string> RefInfo { get; set; }

        /// <summary>
        /// 初始化(所有成员默认值，需要后续赋值)
        /// </summary>
        public HttpResult()
        {
            Code = HttpHelper.STATUS_CODE_UNDEF;
            Text = null;
            Data = null;

            RefCode = HttpHelper.STATUS_CODE_UNDEF;
            RefInfo = null;
        }

        /// <summary>
        /// 对象复制
        /// </summary>
        /// <param name="hr"></param>
        public void shadow(HttpResult hr)
        {
            this.Code = hr.Code;
            this.Text = hr.Text;
            this.Data = hr.Data;
            this.RefCode = hr.RefCode;
            this.RefText += hr.RefText;
            this.RefInfo = hr.RefInfo;
        }

        /// <summary>
        /// 转换为易读字符串格式
        /// </summary>
        /// <returns>便于打印和阅读的字符串</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("code: {0}\n", Code);

            if (!string.IsNullOrEmpty(Text))
            {
                sb.AppendLine("text:");
                sb.AppendLine(Text);
            }

            sb.AppendLine();

            if (Data != null)
            {
                sb.AppendLine("data:");
                if (Data.Length <= 4096)
                {
                    sb.AppendLine(Encoding.UTF8.GetString(Data));
                }
                else
                {
                    int n = 1024;
                    sb.AppendLine(Encoding.UTF8.GetString(Data, 0, n));
                    sb.AppendFormat("<--- TOO-LARGE-TO-DISPLAY --- REST {0} BYTES --->\n", Data.Length - n);
                }
            }
            sb.AppendLine();

            sb.AppendFormat("ref-code:{0}\n", RefCode);

            if(!string.IsNullOrEmpty(RefText))
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
