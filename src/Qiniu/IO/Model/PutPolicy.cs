using System;
using System.Text;

namespace Qiniu.IO.Model
{
    /// <summary>
    /// 上传策略
    /// 另请参阅 http://developer.qiniu.com/article/developer/security/put-policy.html
    /// </summary>
    public class PutPolicy
    {
        /// <summary>
        /// [必需]bucket或者bucket:key
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// [必需]上传策略失效时刻，请使用SetExpire来设置它
        /// </summary>
        public int Deadline { get; private set; }

        /// <summary>
        /// [可选]"仅新增"模式
        /// </summary>
        public int? InsertOnly { get; set; }

        /// <summary>
        /// [可选]保存文件的key
        /// </summary>
        public string SaveKey { get; set; }

        /// <summary>
        /// [可选]终端用户
        /// </summary>
        public string EndUser { get; set; }

        /// <summary>
        /// [可选]返回URL
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// [可选]返回内容
        /// </summary>
        public string ReturnBody { get; set; }

        /// <summary>
        /// [可选]回调URL
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// [可选]回调内容
        /// </summary>
        public string CallbackBody { get; set; }

        /// <summary>
        /// [可选]回调内容类型
        /// </summary>
        public string CallbackBodyType { get; set; }

        /// <summary>
        /// [可选]回调host
        /// </summary>
        public string CallbackHost { get; set; }

        /// <summary>
        /// [可选]回调fetchkey
        /// </summary>
        public int? CallbackFetchKey { get; set; }

        /// <summary>
        /// [可选]上传预转持久化
        /// </summary>
        public string PersistentOps { get; set; }

        /// <summary>
        /// [可选]持久化结果通知
        /// </summary>
        public string PersistentNotifyUrl { get; set; }

        /// <summary>
        /// [可选]私有队列
        /// </summary>
        public string PersistentPipeline { get; set; }

        /// <summary>
        /// [可选]上传文件大小限制：最小值
        /// </summary>
        public int? FileSizeMin { get; set; }

        /// <summary>
        /// [可选]上传文件大小限制：最大值
        /// </summary>
        public int? FileSizeLimit { get; set; }

        /// <summary>
        /// [可选]上传时是否自动检测MIME
        /// </summary>
        public int? DetectMime { get; set; }

        /// <summary>
        /// [可选]上传文件MIME限制
        /// </summary>
        public string MimeLimit { get; set; }

        /// <summary>
        /// [可选]文件上传后多少天后自动删除
        /// </summary>
        public int? DeleteAfterDays { get; set; }

        /// <summary>
        /// 设置上传凭证有效期
        /// </summary>
        /// <param name="expireInSeconds"></param>
        public void SetExpires(int expireInSeconds)
        {
            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Deadline = (int)ts.TotalSeconds + expireInSeconds;
        }

        /// <summary>
        /// 转换为JSON字符串
        /// </summary>
        /// <returns>JSON字符串</returns>
        public string ToJsonString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            
            sb.AppendFormat("\"scope\": \"{0}\"", Scope); //必需

            sb.Append(", "); 
            sb.AppendFormat("\"deadline\": {0}", Deadline);  //必需

            if (InsertOnly.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"inserOnly\": {0}", InsertOnly);
            }

            if (!string.IsNullOrEmpty(SaveKey))
            {
                sb.Append(", ");
                sb.AppendFormat("\"saveKey\": \"{0}\"", SaveKey);
            }

            if (!string.IsNullOrEmpty(EndUser))
            {
                sb.Append(", ");
                sb.AppendFormat("\"endUser\": \"{0}\"", EndUser);
            }

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                sb.Append(", ");
                sb.AppendFormat("\"returnUrl\": \"{0}\"", ReturnUrl);
            }

            if (!string.IsNullOrEmpty(ReturnBody))
            {
                sb.Append(", ");
                sb.AppendFormat("\"returnBody\": \"{0}\"", ReturnBody);
            }

            if (!string.IsNullOrEmpty(CallbackUrl))
            {
                sb.Append(", ");
                sb.AppendFormat("\"callbackUrl\": \"{0}\"", CallbackUrl);
            }

            if (!string.IsNullOrEmpty(CallbackBody))
            {
                sb.Append(", ");
                sb.AppendFormat("\"callbackBody\": \"{0}\"", CallbackBody);
            }

            if (!string.IsNullOrEmpty(CallbackBodyType))
            {
                sb.Append(", ");
                sb.AppendFormat("\"callbackBodyType\": \"{0}\"", CallbackBodyType);
            }

            if (!string.IsNullOrEmpty(CallbackHost))
            {
                sb.Append(", ");
                sb.AppendFormat("\"calbackHost\": \"{0}\"", CallbackHost);
            }

            if (CallbackFetchKey.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"callbackFetchKey\": {0}", CallbackFetchKey);
            }

            if (!string.IsNullOrEmpty(PersistentOps))
            {
                sb.Append(", ");
                sb.AppendFormat("\"persistentOps\": \"{0}\"", PersistentOps);
            }

            if (!string.IsNullOrEmpty(PersistentNotifyUrl))
            {
                sb.Append(", ");
                sb.AppendFormat("\"persistentNotifyUrl\": \"{0}\"", PersistentNotifyUrl);
            }

            if (!string.IsNullOrEmpty(PersistentPipeline))
            {
                sb.Append(", ");
                sb.AppendFormat("\"persistentPipeline\": \"{0}\"", PersistentPipeline);
            }

            if (FileSizeMin.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"fsizeMin\": {0}", FileSizeMin);
            }

            if (FileSizeLimit.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"fsizeLimit\": {0}", FileSizeLimit);
            }

            if (DetectMime.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"detectMime\": {0}", DetectMime);
            }

            if (!string.IsNullOrEmpty(MimeLimit))
            {
                sb.Append(", ");
                sb.AppendFormat("\"mimeLimit\": \"{0}\"", MimeLimit);
            }

            if (DeleteAfterDays.HasValue)
            {
                sb.Append(", ");
                sb.AppendFormat("\"deleteAfterDays\": {0}", DeleteAfterDays);
            }

            sb.Append(" }");

            return sb.ToString();
        }

    }
}
