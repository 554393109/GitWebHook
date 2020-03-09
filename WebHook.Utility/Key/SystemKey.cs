using System.Collections.Generic;
using System.Linq;
using WebHook.Utility.Extension;

namespace WebHook.Utility
{
    public class SystemKey
    {
        /// <summary>
        /// HttpClient TaskWait，
        /// 单位：ms，
        /// 默认20毫秒。
        /// </summary>
        public static readonly int HttpClient_TaskWait = AppConfig.GetValue("HttpClient_TaskWait").IsInt()
            ? AppConfig.GetValue("HttpClient_TaskWait").ToInt32()
            : 20;

        /// <summary>
        /// 系统内部通讯签名密钥
        /// </summary>
        public static readonly string InternalSignKey = AppConfig.GetValue("InternalSignKey");

        /// <summary>
        /// AES密钥
        /// </summary>
        public static readonly string AesKey = AppConfig.GetValue("AesKey");

        /// <summary>
        /// 是否调试模式，
        /// 1:是
        /// </summary>
        public static readonly bool IsDebugMode = "1".Equals(AppConfig.GetValue("IsDebugMode").ValueOrEmpty("0"), System.StringComparison.OrdinalIgnoreCase)
            ? true
            : false;
    }
}
