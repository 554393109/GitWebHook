/************************************************************************
 * 文件标识：  41788caa-8d4c-4e9d-a1c7-ceab30e893ec
 * 项目名称：  YZQ.Utility.Extension  
 * 项目描述：  
 * 类 名 称：  DateTimeExtensions
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/04/20 16:24:57
 * 更新时间：  2018/04/20 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace YZQ.Utility.Extension
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
