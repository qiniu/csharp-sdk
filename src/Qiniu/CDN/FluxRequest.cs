using System.Text;

namespace Qiniu.Cdn
{
    /// <summary>
    /// 查询流量-请求
    /// </summary>
    public class FluxRequest
    {
        /// <summary>
        /// 起始日期，例如2016-09-01
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束日期，例如2016-09-10
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 时间粒度((取值：5min ／ hour ／day))
        /// </summary>
        public string Granularity { get; set; }

        /// <summary>
        /// 域名列表，以西文半角分号分割
        /// </summary>
        public string Domains { get; set; }

        /// <summary>
        /// 初始化(所有成员为空，需要后续赋值)
        /// </summary>
        public FluxRequest()
        {
            StartDate = "";
            EndDate = "";
            Granularity = "";
            Domains = "";
        }

        /// <summary>
        /// 初始化所有成员
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="granularity">时间粒度</param>
        /// <param name="domains">域名列表</param>
        public FluxRequest(string startDate, string endDate, string granularity, string domains)
        {
            StartDate = startDate;
            EndDate = endDate;
            Granularity = granularity;
            Domains = domains;
        }

        /// <summary>
        /// 转换到JSON字符串
        /// </summary>
        /// <returns>请求内容的JSON字符串</returns>
        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{ ");
            sb.AppendFormat("\"startDate\":\"{0}\", ", StartDate);
            sb.AppendFormat("\"endDate\":\"{0}\", ", EndDate);
            sb.AppendFormat("\"granularity\":\"{0}\", ", Granularity);
            sb.AppendFormat("\"domains\":\"{0}\"", Domains);
            sb.Append(" }");

            return sb.ToString();
        }
    }
}
