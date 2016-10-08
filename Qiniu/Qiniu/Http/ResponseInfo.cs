using System;

namespace Qiniu.Http
{
    /// <summary>
    /// 请求回复信息
    /// </summary>
    public class ResponseInfo
    {
        public const int InvalidRequest = -5;
        public const int InvalidArgument = -4;
        public const int InvalidFile = -3;
        public const int Cancelled = -2;
        public const int NetworkError = -1;

        public int StatusCode { set; get; }
        public string ReqId { set; get; }
        public string Xlog { set; get; }
        public string Xvia { set; get; }
        public string Error { set; get; }
        public double Duration { set; get; }
        public string Host { set; get; }
        public string Ip { set; get; }

        /// <summary>
        /// 构建请求回复对象
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="reqId">七牛头部ReqId</param>
        /// <param name="xlog">七牛头部Xlog</param>
        /// <param name="xvia">七牛头部Xvia</param>
        /// <param name="host">主机</param>
        /// <param name="ip">Ip地址</param>
        /// <param name="duration">持续时间</param>
        /// <param name="error">错误信息</param>
        public ResponseInfo(int statusCode, string reqId, string xlog, string xvia,
            string host, string ip, double duration, string error)
        {
            this.StatusCode = statusCode;
            this.ReqId = reqId;
            this.Xlog = xlog;
            this.Xvia = xvia;
            this.Host = host;
            this.Ip = ip;
            this.Duration = duration;
            this.Error = error;
        }

        /// <summary>
        /// 客户端取消
        /// </summary>
        /// <returns>回复对象</returns>
        public static ResponseInfo cancelled()
        {
            return new ResponseInfo(Cancelled, "", "", "", "", "", 0, "cancelled by user");
        }

        /// <summary>
        /// 网络故障
        /// </summary>
        public static ResponseInfo networkError(string message)
        {
            return new ResponseInfo(NetworkError, "", "", "", "", "", 0, message);
        }
        /// <summary>
        /// 参数不合法
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <returns>回复对象</returns>
        public static ResponseInfo invalidArgument(string message)
        {
            return new ResponseInfo(InvalidArgument, "", "", "", "", "", 0, message);
        }

        public static ResponseInfo invalidRequest(string message)
        {
            return new ResponseInfo(InvalidRequest, "", "", "", "", "", 0, message);
        }

        /// <summary>
        /// 客户端文件访问错误
        /// </summary>
        /// <param name="e">IO异常对象</param>
        /// <returns>回复对象</returns>
        public static ResponseInfo fileError(Exception e)
        {
            return new ResponseInfo(InvalidFile, "", "", "", "", "", 0, e.Message);
        }

        /// <summary>
        /// 判断是否客户端取消
        /// </summary>
        /// <returns>取消状态</returns>
        public bool isCancelled()
        {
            return StatusCode == Cancelled;
        }

        /// <summary>
        /// 文件上传请求是否完全成功
        /// </summary>
        /// <returns>成功状态</returns>
        public bool isOk()
        {
            return StatusCode == 200 && Error == null && ReqId != null;
        }

        /// <summary>
        /// 检测是否网络故障
        /// </summary>
        /// <returns>网络状态</returns>
        public bool isNetworkBroken()
        {
            return StatusCode == NetworkError;
        }

        /// <summary>
        /// 检测是否七牛服务器错误
        /// </summary>
        /// <returns>服务器状态</returns>
        public bool isServerError()
        {
            return (StatusCode >= 500 && StatusCode < 600 && StatusCode != 579) || StatusCode == 996;
        }

        /// <summary>
        /// 检测客户端是否需要重试上传请求
        /// </summary>
        /// <returns>是否重试</returns>
        public bool needRetry()
        {
            return isNetworkBroken() || isServerError() || StatusCode == 404 || StatusCode == 406 || (StatusCode == 200 && Error != null);
        }

        private string toStr(string val)
        {
            return (val == null) ? "null" : val;
        }

        public override string ToString()
        {
            return string.Format("ResponseInfo: status:{0}, reqId:{1}, xlog:{2}, xvia:{3}, host:{4}, ip:{5}, duration:{6} s, error:{7}",
                 StatusCode, toStr(ReqId), toStr(Xlog), toStr(Xvia), toStr(Host), toStr(Ip), Duration, toStr(Error));
        }
    }
}
