using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace WebHook.Utility.Extension
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 本地时间 -> 时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return DateTimeHelper.ToUnixTimestamp(dateTime);
        }

        /// <summary>
        /// 本地时间.ToUniversalTime() -> 时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixUTCTimestamp(this DateTime dateTime)
        {
            return DateTimeHelper.ToUnixUTCTimestamp(dateTime);
        }
    }
}
