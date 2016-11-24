using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Util
{
    /// <summary>
    /// 上传策略扩展
    /// </summary>
    public static partial class PutPolicyExtension
    {
        /// <summary>
        /// 指定上传的目标资源空间 Bucket 和资源键 Key（key的长度最大为750字节）。有两种格式：
        /// ● <bucket>，表示允许用户上传文件到指定的 bucket。在这种格式下文件只能新增，若已存在同名资源上传则会失败。
        /// ● <bucket>:<key>，表示只允许用户上传指定 key 的文件。在这种格式下文件默认允许修改，若已存在同名资源则会被覆盖。如果只希望上传指定 key 的文件，并且不允许修改，那么可以将下面的 insertOnly 属性值设为 1。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="scope">上传的目标资源空间 Bucket 和资源键 Key（key的长度最大为750字节）</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithScope(this PutPolicy policy, string scope)
        {
            policy.Scope = scope;
            return policy;
        }
        /// <summary>
        /// 上传凭证有效截止时间。Unix时间戳，单位为秒。该截止时间为上传完成后，在七牛空间生成文件的校验时间，而非上传的开始时间，一般建议设置为上传开始时间 + 3600s，用户可根据具体的业务场景对凭证截止时间进行调整。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="deadline">上传凭证有效截止时间，Unix时间戳，单位为秒</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithDeadline(this PutPolicy policy, int deadline)
        {
            policy.Deadline = deadline;
            return policy;
        }
        /// <summary>
        /// 上传凭证有效截止时间。根据时间间隔自动计算对应的Unix时间戳，单位为秒。该截止时间为上传完成后，在七牛空间生成文件的校验时间，而非上传的开始时间，一般建议设置为上传开始时间 + 3600s，用户可根据具体的业务场景对凭证截止时间进行调整。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="deadline">上传凭证有效截止时间间隔</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithDeadline(this PutPolicy policy, TimeSpan deadline)
        {
            policy.SetExpires((int)deadline.TotalSeconds);
            return policy;
        }
        /// <summary>
        /// 上传凭证有效截止时间。根据时间自动计算对应的Unix时间戳，单位为秒。该截止时间为上传完成后，在七牛空间生成文件的校验时间，而非上传的开始时间，一般建议设置为上传开始时间 + 3600s，用户可根据具体的业务场景对凭证截止时间进行调整。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="deadline">上传凭证有效截止时间</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithDeadline(this PutPolicy policy, DateTime deadline)
        {
            return policy.WithDeadline(deadline - DateTime.Now);
        }
        /// <summary>
        /// 限定为新增语意。如果设置为false，则无论 scope 设置为什么形式，仅能以新增模式上传文件。默认为 true。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="insertOnly">是否限定为新增语意，默认为 true，限定为新增语义。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithInsertOnly(this PutPolicy policy, bool insertOnly = true)
        {
            policy.InsertOnly = insertOnly ? 1 : 0;
            return policy;
        }
        /// <summary>
        /// 唯一属主标识。特殊场景下非常有用，例如根据 App-Client 标识给图片或视频打水印。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="endUser">唯一属主标识</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithEndUser(this PutPolicy policy, string endUser)
        {
            policy.EndUser = endUser;
            return policy;
        }
        /// <summary>
        /// Web 端文件上传成功后，浏览器执行 303 跳转的 URL。通常用于 HTML Form 上传。文件上传成功后会跳转到 <returnUrl>?upload_ret=<queryString>，<queryString>包含 returnBody 内容。如不设置 returnUrl，则直接将 returnBody 的内容返回给客户端。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="returnUrl">Web 端文件上传成功后，浏览器执行 303 跳转的 URL</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithReturnUrl(this PutPolicy policy, string returnUrl)
        {
            policy.ReturnUrl = returnUrl;
            return policy;
        }
        /// <summary>
        /// 上传成功后，自定义七牛云最终返回給上传端（在指定 returnUrl 时是携带在跳转路径参数中）的数据。支持魔法变量和自定义变量。returnBody 要求是合法的 JSON 文本。例如 {"key": $(key), "hash": $(etag), "w": $(imageInfo.width), "h": $(imageInfo.height)}。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="returnBody">上传成功后，自定义七牛云最终返回給上传端（在指定 returnUrl 时是携带在跳转路径参数中）的数据</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithReturnBody(this PutPolicy policy, string returnBody)
        {
            policy.ReturnBody = returnBody;
            return policy;
        }
        /// <summary>
        /// 上传成功后，七牛云向 App-Server 发送 POST 请求的 URL。必须是公网上可以正常进行 POST 请求并能响应 HTTP/1.1 200 OK 的有效 URL。另外，为了给客户端有一致的体验，我们要求 callbackUrl 返回包 Content-Type 为 "application/json"，即返回的内容必须是合法的 JSON 文本。出于高可用的考虑，本字段允许设置多个 callbackUrl(用英文符号 ; 分隔)，在前一个 callbackUrl 请求失败的时候会依次重试下一个 callbackUrl。一个典型例子是 http://<ip1>/callback;http://<ip2>/callback，并同时指定下面的 callbackHost 字段。在 callbackUrl 中使用 ip 的好处是减少了对 dns 解析的依赖，可改善回调的性能和稳定性。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="callbackUrl">上传成功后，七牛云向 App-Server 发送 POST 请求的 URL</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithCallbackUrl(this PutPolicy policy, string callbackUrl)
        {
            policy.CallbackUrl = callbackUrl;
            return policy;
        }
        /// <summary>
        /// 上传成功后，七牛云向"App-Server"发送回调通知时的 Host 值。
        /// 与callbackUrl配合使用，仅当设置了 callbackUrl 时才有效。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="callbackHost">上传成功后，七牛云向"App-Server"发送回调通知时的 Host 值。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithCallbackHost(this PutPolicy policy, string callbackHost)
        {
            policy.CallbackHost = callbackHost;
            return policy;
        }
        /// <summary>
        /// 上传成功后，七牛云向"App-Server"发送Content-Type: application/x-www-form-urlencoded 的POST请求。
        /// 该字段"App-Server"可以通过直接读取请求的query来获得，支持魔法变量和自定义变量。callbackBody 要求是合法的 url query string。如：key=$(key)&hash=$(etag)&w=$(imageInfo.width)&h=$(imageInfo.height)。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="callbackBody">上传成功后，七牛云向"App-Server"发送Content-Type: application/x-www-form-urlencoded 的POST请求。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithCallbackBody(this PutPolicy policy, string callbackBody)
        {
            policy.CallbackBody = callbackBody;
            return policy;
        }
        /// <summary>
        /// 上传成功后，七牛云向"App-Server"发送回调通知callbackBody的Content-Type。
        /// 默认为application/x-www-form-urlencoded，也可设置为application/json。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="callbackBodyType">上传成功后，七牛云向"App-Server"发送回调通知callbackBody的Content-Type。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithCallbackBodyType(this PutPolicy policy, string callbackBodyType = "application/x-www-form-urlencoded")
        {
            policy.CallbackBodyType = callbackBodyType;
            return policy;
        }
        /// <summary>
        /// 是否启用fetchKey上传模式。
        /// false 为关闭，true 为启用。具体见callbackFetchKey详解。默认为 true。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="callbackFetchKey">是否启用fetchKey上传模式。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithCallbackFetchKey(this PutPolicy policy, bool callbackFetchKey = true)
        {
            policy.CallbackFetchKey = callbackFetchKey ? 1 : 0;
            return policy;
        }
        /// <summary>
        /// 资源上传成功后触发执行的预转持久化处理指令列表。
        /// 每个指令是一个API规格字符串，多个指令用;分隔。
        /// 请参阅persistentOps详解与示例。同时添加persistentPipeline字段，使用专用队列处理，请参阅persistentPipeline。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="persistentOps">资源上传成功后触发执行的预转持久化处理指令列表。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithPersistentOps(this PutPolicy policy, string persistentOps)
        {
            policy.PersistentOps = persistentOps;
            return policy;
        }
        /// <summary>
        /// 资源上传成功后触发执行的预转持久化处理指令列表。
        /// 请参阅persistentOps详解与示例。同时添加persistentPipeline字段，使用专用队列处理，请参阅persistentPipeline。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="persistentOps">资源上传成功后触发执行的预转持久化处理指令列表。详情参见：<seealso cref="Qiniu.Util.IPersistentOps"/></param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithPersistentOps(this PutPolicy policy, params IPersistentOps[] persistentOps)
        {
            if (persistentOps != null && persistentOps.Any())
            {
                return policy.WithPersistentOps(string.Join(";", persistentOps.Select(p => p.GetOpsString())));
            }
            return policy;
        }
        /// <summary>
        /// 接收持久化处理结果通知的URL。
        /// 必须是公网上可以正常进行POST请求并能响应"HTTP/1.1 200 OK"的有效URL。
        /// 该URL获取的内容和持久化处理状态查询(prefop)的处理结果一致。
        /// 发送body格式是Content-Type为application/json的POST请求，需要按照读取流的形式读取请求的body才能获取。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="persistentNotifyUrl">接收持久化处理结果通知的URL。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithPersistentNotifyUrl(this PutPolicy policy, string persistentNotifyUrl)
        {
            policy.PersistentNotifyUrl = persistentNotifyUrl;
            return policy;
        }
        /// <summary>
        /// 转码队列名。
        /// 资源上传成功后，触发转码时指定独立的队列进行转码。为空则表示使用公用队列，处理速度比较慢。建议使用专用队列。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="persistentPipeline">转码队列名。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithPersistentPipeline(this PutPolicy policy, string persistentPipeline)
        {
            policy.PersistentPipeline = persistentPipeline;
            return policy;
        }
        /// <summary>
        /// 自定义资源名。
        /// 支持魔法变量及自定义变量。这个字段仅当用户上传的时候没有主动指定key的时候起作用。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="saveKey">自定义资源名。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithSaveKey(this PutPolicy policy, string saveKey)
        {
            policy.SaveKey = saveKey;
            return policy;
        }
        /// <summary>
        /// 限定上传文件大小最小值，单位：字节（Byte）。设置为k，即k及k以上的文件可以上传。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="fsizeMin">限定上传文件大小最小值，单位：字节（Byte）</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithFsizeMin(this PutPolicy policy, int fsizeMin)
        {
            policy.FsizeMin = fsizeMin;
            return policy;
        }
        /// <summary>
        /// 限定上传文件大小最大值，单位：字节（Byte）。
        /// 超过限制上传文件大小的最大值会被判为上传失败，返回413状态码。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="fsizeLimit">限定上传文件大小最大值，单位：字节（Byte）。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithFsizeLimit(this PutPolicy policy, int fsizeLimit)
        {
            policy.FsizeLimit = fsizeLimit;
            return policy;
        }
        /// <summary>
        /// 开启MimeType侦测功能。
        /// 此函数默认设为 true 值，则忽略上传端传递的文件MimeType信息，使用七牛服务器侦测内容后的判断结果。
        /// 如不设置则默认等同 false 值，如上传端指定了MimeType则直接使用该值，否则按如下顺序侦测MimeType值：
        /// 1. 检查文件扩展名；
        /// 2. 检查Key扩展名；
        /// 3. 侦测内容。
        /// 如不能侦测出正确的值，会默认使用 application/octet-stream 。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="detectMime">是否开启MimeType侦测功能。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithDetectMime(this PutPolicy policy, bool detectMime = true)
        {
            policy.DetectMime = detectMime ? 1 : 0;
            return policy;
        }
        /// <summary>
        /// 限定用户上传的文件类型。
        /// 指定本字段值，七牛服务器会侦测文件内容以判断MimeType，再用判断值跟指定值进行匹配，匹配成功则允许上传，匹配失败则返回403状态码。
        /// 示例：
        /// ● image/*表示[[[只允许]]]上传图片类型 
        /// ● image/jpeg;image/png表示[[[只允许]]]上传jpg和png类型的图片 
        /// ● !application/json;text/plain表示[[[禁止]]]上传json文本和纯文本。注意最前面的感叹号！
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="mimeLimit">限定用户上传的文件类型。</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithMimeLimit(this PutPolicy policy, string mimeLimit)
        {
            policy.MimeLimit = mimeLimit;
            return policy;
        }
        /// <summary>
        /// 文件在多少天后被删除，七牛将文件上传时间与指定的deleteAfterDays天数相加，得到的时间入到后一天的午夜(CST,中国标准时间)，从而得到文件删除开始时间。例如文件在2015年1月1日上午10:00 CST上传，指定deleteAfterDays为3天，那么会在2015年1月5日00:00 CST之后当天内删除文件。
        /// </summary>
        /// <param name="policy">上传策略</param>
        /// <param name="deleteAfterDays">文件在多少天后被删除</param>
        /// <returns>上传策略</returns>
        public static PutPolicy WithDeleteAfterDays(this PutPolicy policy, int deleteAfterDays)
        {
            policy.DeleteAfterDays = deleteAfterDays;
            return policy;
        }
    }
}
