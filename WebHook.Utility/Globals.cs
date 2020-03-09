using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebHook.Utility.Extension;

namespace WebHook.Utility
{
    public static class Globals
    {
        /// <summary>
        /// 获得当前客户端的IP
        /// </summary>
        /// <returns>当前客户端的IP</returns>
        public static string ClientIP
        {
            get {
                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrWhiteSpace(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrWhiteSpace(result))
                    result = HttpContext.Current.Request.UserHostAddress;

                if (string.IsNullOrWhiteSpace(result) || !IsIP(result))
                    return "127.0.0.1";

                return result;
            }
        }

        /// <summary>
        /// 获取服务端的IP
        /// </summary>
        public static string ServerIP
        {
            get {
                string result = string.Empty;

                result = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];

                if (!result.IsIP())
                    result = "127.0.0.1";

                return result;
            }
        }



        /// <summary>
        /// IList -> List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list_old"></param>
        /// <returns></returns>
        public static List<T> ConvertToCollectionList<T>(IList<T> list_old)
            where T : new()
        {
            List<T> list_new = new List<T>();
            if (list_old == null)
                return list_new;

            list_new.AddRange(list_old);
            return list_new;
        }

        /// <summary>
        /// 截取字符串
        /// 减3位 拼省略号【...】
        /// </summary>
        /// <param name="str_original"></param>
        /// <param name="len">最大长度</param>
        /// <returns></returns>
        public static string CutString(string str_original, int len)
        {
            if (!string.IsNullOrWhiteSpace(str_original) && str_original.Length > len)
                return str_original.Remove(len - 3) + "...";
            else
                return str_original;
        }


        #region Url编解码

        public static string UrlDecode(string str)
        {
            return UrlDecode(str, Encoding.UTF8);
        }

        public static string UrlDecode(string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }

        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.UTF8);
        }

        public static string UrlEncode(string str, Encoding encoding)
        {
            return HttpUtility.UrlEncode(str, encoding);
        }

        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jerry，如果不编码，可能得到的网址： http://a.com/?tvshow=Tom&Jerry&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jerry&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UrlEscape(string source)
        {
            if (source == null)
                return null;

            return Uri.EscapeDataString(source);
        }

        public static string UrlUnescape(string source)
        {
            if (source == null)
                return null;

            return Uri.UnescapeDataString(source);
        }

        #endregion Url编解码

        #region 类型转换

        /// <summary>
        /// String -> Int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static int ConvertStringToInt(string str, int defaultvalue = 0)
        {
            int result = 0;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (int.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Long
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static long ConvertStringToLong(string str, long defaultvalue = 0)
        {
            long result = 0;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (long.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Double
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static double ConvertStringToDouble(string str, double defaultvalue = 0.00)
        {
            double result = 0.00;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (double.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Decimal
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static decimal ConvertStringToDecimal(string str, decimal defaultvalue = 0.00M)
        {
            decimal result = 0.00M;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (decimal.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }
        /// <summary>
        /// String -> Bool
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns></returns>
        public static bool ConvertStringToBool(string str, bool defaultvalue = false)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(str))
                return defaultvalue;
            else if (bool.TryParse(str, out result))
                return result;
            else
                return defaultvalue;
        }

        public static DateTime ConvertStringToDateTime(string str, string defaultvalue = "1900-01-01 00:00:00.000")
        {
            DateTime result;

            if (str.IsDateTime())
                result = DateTime.Parse(str);
            else if (defaultvalue.IsDateTime())
                result = DateTime.Parse(defaultvalue);
            else
                result = new DateTime(1900, 1, 1);

            return result;
        }

        #endregion 类型转换

        #region 类型判断

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(object obj)
        {
            bool isCorrect = false;
            string ip = string.Empty;

            if (obj != null)
                ip = obj.ToString();

            isCorrect = Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
            return isCorrect;
        }


        /// <summary>
        /// 是否为Url
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsURL(object obj)
        {
            bool isCorrect = false;
            string url = string.Empty;

            if (obj != null)
                url = obj.ToString();

            isCorrect = Regex.IsMatch(url, @"^((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
            return isCorrect;
        }


        /// <summary>
        /// 判断是否Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsInt(object obj)
        {
            bool isCorrect = false;
            int val = 0;

            if (obj != null)
                isCorrect = int.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否Long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsLong(object obj)
        {
            bool isCorrect = false;
            long val = 0;

            if (obj != null)
                isCorrect = long.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否Decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDecimal(object obj)
        {
            bool isCorrect = false;
            Decimal val = 0;

            if (obj != null)
                isCorrect = Decimal.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        /// <summary>
        /// 判断是否DateTime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsDateTime(object obj)
        {
            bool isCorrect = false;
            DateTime val;

            if (obj != null)
                isCorrect = DateTime.TryParse(obj.ToString(), out val);

            return isCorrect;
        }

        #endregion 类型判断
    }
}
