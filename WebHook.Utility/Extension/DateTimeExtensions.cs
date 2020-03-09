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
        /// <param name="accuracy">精度</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime, TimestampAccuracy accuracy = TimestampAccuracy.Seconds)
        {
            return DateTimeHelper.ToUnixTimestamp(dateTime, accuracy);
        }

        /// <summary>
        /// 本地时间.ToUniversalTime() -> 时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="accuracy">精度</param>
        /// <returns></returns>
        public static long ToUnixUTCTimestamp(this DateTime dateTime, TimestampAccuracy accuracy = TimestampAccuracy.Seconds)
        {
            return DateTimeHelper.ToUnixUTCTimestamp(dateTime, accuracy);
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format">yyyy-MM-dd HH:mm:ss.fff</param>
        /// <returns></returns>
        public static string ToFormatString(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss.fff")
        {
            return dateTime.ToString(format: format.ValueOrEmpty("yyyy-MM-dd HH:mm:ss.fff"));
        }
    }
}
