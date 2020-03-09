using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using WebHook.Utility.Extension;

namespace WebHook.Utility
{
    public class LogHelper
    {
        #region 取配置

        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");                   //选择<logger name="logerror">的配置

        //private static readonly log4net.ILog logwarn = log4net.LogManager.GetLogger("logwarn");                     //选择<logger name="logwarn">的配置

        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");                     //选择<logger name="loginfo">的配置

        private static readonly log4net.ILog logdebug = log4net.LogManager.GetLogger("logdebug");                   //选择<logger name="logdebug">的配置

        #endregion 取配置

        public LogHelper()
        {
        }


        public static void SetConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void SetConfig(FileInfo configFile)
        {
            log4net.Config.XmlConfigurator.Configure(configFile);
        }


        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(string error)
        {
            if (logerror.IsErrorEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                logerror.Error($"【Target】：【{target}】\r\n{error}");
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                logerror.Error($"【Target】：【{target}】\r\n{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="info"></param>
        /// <param name="se"></param>
        public static void Error(string error, Exception ex)
        {
            if (logerror.IsErrorEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                logerror.Error($"【Target】：【{target}】\r\n{error}", ex);
            }
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="info"></param>
        public static void Info(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                loginfo.Info($"【Target】：【{target}】\r\n{info}");
            }
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void InfoFormat(string format, params object[] args)
        {
            if (loginfo.IsInfoEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                loginfo.Info($"【Target】：【{target}】\r\n{string.Format(format, args)}");
            }
        }

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="debug"></param>
        public static void Debug(string debug)
        {
            if (logdebug.IsDebugEnabled)
            {
                var mb = new StackTrace().GetFrame(1).GetMethod();
                var target = string.Format("/{0}/{1}", mb.DeclaringType, mb.Name);

                logdebug.Debug($"【Target】：【{target}】\r\n{debug}");
            }
        }
    }
}