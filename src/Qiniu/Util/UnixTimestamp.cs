using System;

namespace Qiniu.Util
{
    /// <summary>
    ///     时间戳与日期时间转换
    /// </summary>
    public class UnixTimestamp
    {
        /// <summary>
        ///     时间戳末尾7位(补0或截断)
        /// </summary>
        private const long TickBase = 10000000;

        /// <summary>
        ///     基准时间
        /// </summary>
        private static readonly DateTime DtBase = new DateTime(1970, 1, 1).ToLocalTime();

        /// <summary>
        ///     从现在(调用此函数时刻)起若干秒以后那个时间点的时间戳
        /// </summary>
        /// <param name="secondsAfterNow">从现在起多少秒以后</param>
        /// <returns>Unix时间戳</returns>
        public static long GetUnixTimestamp(long secondsAfterNow)
        {
            var dt = DateTime.Now.AddSeconds(secondsAfterNow).ToLocalTime();
            var tsx = dt.Subtract(DtBase);
            return tsx.Ticks / TickBase;
        }

        /// <summary>
        ///     日期时间转换为时间戳
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns>时间戳</returns>
        public static long ConvertToTimestamp(DateTime dt)
        {
            var tsx = dt.Subtract(DtBase);
            return tsx.Ticks / TickBase;
        }

        /// <summary>
        ///     从UNIX时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">时间戳字符串</param>
        /// <returns>日期时间</returns>
        public static DateTime ConvertToDateTime(string timestamp)
        {
            var ticks = long.Parse(timestamp) * TickBase;
            return DtBase.AddTicks(ticks);
        }

        /// <summary>
        ///     从UNIX时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns>日期时间</returns>
        public static DateTime ConvertToDateTime(long timestamp)
        {
            var ticks = timestamp * TickBase;
            return DtBase.AddTicks(ticks);
        }

        /// <summary>
        ///     检查Ctx是否过期，我们给当前时间加上一天来看看是否超过了过期时间
        ///     而不是直接比较是否超过了过期时间，是给这个文件最大1天的上传持续时间
        /// </summary>
        /// <param name="expiredAt"></param>
        /// <returns></returns>
        public static bool IsContextExpired(long expiredAt)
        {
            if (expiredAt == 0)
            {
                return false;
            }

            var expired = false;
            var now = DateTime.Now.AddDays(1);
            var nowTs = ConvertToTimestamp(now);
            if (nowTs > expiredAt)
            {
                expired = true;
            }

            return expired;
        }
    }
}
