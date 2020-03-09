using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WebHook.Utility.Extension;

namespace WebHook.Utility.SignUtils
{
    public class SHA256
    {
        /// <summary>
        /// 使用UTF-8进行SHA256签名
        /// 忽略参数中存的sign和value为空的键值对
        /// 最后&key={key}
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sign_key">签名密钥</param>
        /// <param name="format">大小写格式化 默认：FromatSign.UPPER</param>
        /// <returns></returns>
        public static string Sign(IDictionary parameters, string sign_key, FromatSign format = FromatSign.UPPER)
        {
            if (parameters == null || parameters.Count == 0)
                return string.Empty;

            var parameters_filter = new Dictionary<string, string>();
            foreach (DictionaryEntry item in parameters)
            {
                if (item.Key.IsNullOrWhiteSpace()
                    || item.Value.IsNullOrWhiteSpace())
                    continue;

                parameters_filter.Add(item.Key.ToString(), item.Value.ToString());
            }

            var str_sign = BuildSortString(parameters_filter, sign_key, format);
            return str_sign;
        }

        /// <summary>
        /// key排序+拼接
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sign_key">签名密钥</param>
        /// <param name="format">大小写格式化 默认：FromatSign.UPPER</param>
        /// <returns></returns>
        private static string BuildSortString(IDictionary<string, string> parameters, string sign_key, FromatSign format)
        {
            // 把字典按Key的字母顺序排序
            var sortedParams = new SortedDictionary<string, string>(parameters);
            var query_string = new StringBuilder();

            var dem = sortedParams.GetEnumerator();
            while (dem.MoveNext())
            {
                var key = dem.Current.Key;
                var value = dem.Current.Value;
                if ("SIGN".Equals(key, StringComparison.OrdinalIgnoreCase))
                    continue;

                query_string.Append("&").Append(key).Append("=").Append(value);
            }

            query_string.Append("&key=").Append(sign_key.ValueOrEmpty());

            return Sign(query_string.ToString().TrimStart('&'), null, format);
        }

        /// <summary>
        /// 使用UTF-8进行SHA256签名
        /// 加盐&key={sign_key}，若sign_key.IsNullOrWhiteSpace()则不加盐
        /// </summary>
        /// <param name="content">签名原文</param>
        /// <param name="sign_key">签名密钥 为空则不加盐</param>
        /// <param name="format">大小写格式化 默认：FromatSign.UPPER</param>
        /// <returns></returns>
        public static string Sign(string content, string sign_key, FromatSign format = FromatSign.UPPER)
        {
            if (content.IsNullOrWhiteSpace())
                return string.Empty;

            if (!sign_key.IsNullOrWhiteSpace())
                content = $"{content}&key={sign_key}";

            var _format = format == FromatSign.UPPER
                        ? "X2"              /*大写*/
                        : "x2";             /*小写*/

            var result = new StringBuilder();
            using (var provider = new System.Security.Cryptography.SHA256CryptoServiceProvider())
            {
                var byte_sign = provider.ComputeHash(Encoding.UTF8.GetBytes(content));

                // 把二进制转化为十六进制
                for (int i = 0; i < byte_sign.Length; i++)
                    result.Append(byte_sign[i].ToString(format: _format));
            }

            return result.ToString();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="parameters">需验签内容</param>
        /// <param name="sign_key">签名密钥</param>
        /// <returns></returns>
        public static bool CheckSign(IDictionary parameters, string sign_key)
        {
            if (parameters == null
                || parameters.Count == 0
                || parameters["sign"].IsNullOrWhiteSpace())
                return false;

            // 接收到的签名
            var param_sign = parameters["sign"].ToString();
            var calc_sign = string.Empty;

            try
            {
                calc_sign = Sign(parameters, sign_key);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            var flag = false;
            if (calc_sign.Equals(param_sign, StringComparison.OrdinalIgnoreCase))
                flag = true;

            // 若sign == InternalSignKey，返回true
            if (param_sign.Equals(SystemKey.InternalSignKey, StringComparison.OrdinalIgnoreCase))
                flag = true;

            // IsDebugMode不验签，返回true
            if (SystemKey.IsDebugMode)
                flag = true;

            return flag;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="content">需验签内容</param>
        /// <param name="sign">签名</param>
        /// <param name="sign_key">签名密钥</param>
        /// <returns></returns>
        public static bool CheckSign(string content, string sign, string sign_key)
        {
            if (content.IsNullOrWhiteSpace()
                || sign.IsNullOrWhiteSpace()
                || sign_key.IsNullOrWhiteSpace())
                return false;

            var calc_sign = string.Empty;

            try
            {
                calc_sign = Sign(content, sign_key);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            bool flag = false;
            if (calc_sign.Equals(sign, StringComparison.OrdinalIgnoreCase))
                flag = true;

            // 若sign == InternalSignKey，返回true
            if (sign.Equals(SystemKey.InternalSignKey, StringComparison.OrdinalIgnoreCase))
                flag = true;

            // IsDebugMode不验签，返回true
            if (SystemKey.IsDebugMode)
                flag = true;

            return flag;
        }

    }
}
