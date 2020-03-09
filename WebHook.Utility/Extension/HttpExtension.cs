namespace WebHook.Utility.Extension
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Web;
    using WebHook.Utility;

    public static class HttpExtension
    {
        /// <summary>
        /// 从Stream中获得请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetStringFromBody(this HttpRequest request
            , string charset = "utf-8")
        {
            var body = string.Empty;

            if (request.InputStream.CanRead && request.InputStream.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    request.InputStream.CopyTo(memoryStream);
                    request.InputStream.Position = 0;
                    memoryStream.Position = 0;

                    var count = 0;
                    var buffer = new byte[1024];
                    var builder = new StringBuilder();

                    try
                    {
                        while ((count = memoryStream.Read(buffer, 0, 1024)) > 0)
                            builder.Append(Encoding.GetEncoding(charset).GetString(buffer, 0, count));

                        body = builder.ToString();         // 读出报文
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                    finally
                    {
                        memoryStream.Flush();
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
            }

            return body;
        }

        /// <summary>
        /// 获取Form全部参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="NeedUrlDecode">需要URL解码，默认：true</param>
        /// <returns></returns>
        public static string GetStringFromForm(this HttpRequest request
            , bool NeedUrlDecode = false)
        {
            if ((request.Form?.AllKeys?.Length ?? 0) == 0)
                return string.Empty;

            var sb = new StringBuilder();

            try
            {
                foreach (var key in request.Form.AllKeys)
                {
                    if (!key.IsNullOrWhiteSpace())
                    {
                        var val = request.Form[key].ValueOrEmpty();

                        sb.Append(key).Append("=").Append(NeedUrlDecode ? val.Trim().UrlUnescape() : val.Trim()).Append("&");
                    }
                }
            }
            catch { }

            return sb.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 从 Request.Body 中读取流，并复制到一个独立的 MemoryStream 对象中
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Stream GetStreamFromBody(this HttpRequest request
            , string charset = "utf-8")
        {
            Stream inputStream = default(MemoryStream);
            var body = GetStringFromBody(request);

            if (!body.IsNullOrWhiteSpace())
            {
                try
                {
                    var requestData = Encoding.GetEncoding(charset).GetBytes(body);
                    inputStream = new MemoryStream(requestData);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            return inputStream;
        }

        /// <summary>
        /// 从Stream获得请求参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestBodyType">请求Body类型</param>
        /// <returns></returns>
        public static Hashtable GetParametersFromBody(this HttpRequest request
            , RequestBodyTypeEnum requestBodyType = RequestBodyTypeEnum.JSON)
        {
            var param = new Hashtable();
            var body = GetStringFromBody(request);

            if (!body.IsNullOrWhiteSpace())
            {
                try
                {
                    if (RequestBodyTypeEnum.XML == requestBodyType)
                        param = XML.Deserialize<Hashtable>(body);
                    else if (RequestBodyTypeEnum.QueryString == requestBodyType)
                        param = body.QueryStringToHashtable(false);     // stream中传输不存在乱码问题，无需编解码
                    else
                        param = JSON.Deserialize<Hashtable>(body);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }

            return param;
        }

        /// <summary>
        /// 获取QueryString全部参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="NeedUrlDecode">需要URL解码，默认：true</param>
        /// <returns></returns>
        public static Hashtable GetParametersFromQueryString(this HttpRequest request
            , bool NeedUrlDecode = false)
        {
            var param = new Hashtable();
            if ((request.QueryString?.AllKeys?.Length ?? 0) == 0)
                return param;

            try
            {
                foreach (var key in request.QueryString.AllKeys)
                {
                    if (!key.IsNullOrWhiteSpace())
                    {
                        var val = request.QueryString[key].ValueOrEmpty();
                        param[key] = NeedUrlDecode
                            ? val.Trim().UrlUnescape()
                            : val.Trim();
                    }
                }
            }
            catch { }

            return param;
        }

        /// <summary>
        /// 获取Form全部参数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="NeedUrlDecode">需要URL解码，默认：true</param>
        /// <returns></returns>
        public static Hashtable GetParametersFromForm(this HttpRequest request
            , bool NeedUrlDecode = false)
        {
            var param = new Hashtable();
            if ((request.Form?.AllKeys?.Length ?? 0) == 0)
                return param;

            try
            {
                foreach (var key in request.Form.AllKeys)
                {
                    if (!key.IsNullOrWhiteSpace())
                    {
                        var val = request.Form[key].ValueOrEmpty();
                        param[key] = NeedUrlDecode
                            ? val.Trim().UrlUnescape()
                            : val.Trim();
                    }
                }
            }
            catch { }

            return param;
        }

        /// <summary>
        /// 获取QueryString和Form全部参数
        /// Form优先
        /// </summary>
        /// <param name="request"></param>
        /// <param name="NeedUrlDecode">需要URL解码，默认：true</param>
        /// <returns></returns>
        public static Hashtable GetParameters(this HttpRequest request
            , bool NeedUrlDecode = false)
        {
            var param = new Hashtable();

            try
            {
                if ((request.QueryString?.AllKeys?.Length ?? 0) > 0)
                {
                    foreach (var key in request.QueryString.AllKeys)
                    {
                        if (!key.IsNullOrWhiteSpace())
                        {
                            var val = request.QueryString[key].ValueOrEmpty();
                            param[key] = NeedUrlDecode
                                ? val.Trim().UrlUnescape()
                                : val.Trim();
                        }
                    }
                }
            }
            catch { }

            try
            {
                if ((request.Form?.AllKeys?.Length ?? 0) > 0)
                {
                    foreach (var key in request.Form.AllKeys)
                    {
                        if (!key.IsNullOrWhiteSpace())
                        {
                            var val = request.Form[key].ValueOrEmpty();
                            param[key] = NeedUrlDecode
                                ? val.Trim().UrlUnescape()
                                : val.Trim();
                        }
                    }
                }
            }
            catch { }

            return param;
        }

        /// <summary>
        /// 获取指定项参数值
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <param name="NeedUrlDecode">需要URL解码，默认：true</param>
        /// <returns></returns>
        public static string GetParameter(this HttpRequest request
            , string key
            , bool NeedUrlDecode = false)
        {
            var value = string.Empty;

            try
            {
                if ((request.QueryString?.AllKeys?.Length ?? 0) > 0)
                    value = request.QueryString[key].ValueOrEmpty();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            try
            {
                if ((request.Form?.AllKeys?.Length ?? 0) > 0)
                    value = request.Form[key].ValueOrEmpty();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return NeedUrlDecode
                ? value.Trim().UrlUnescape()
                : value.Trim();
        }
    }

    /// <summary>
    /// 请求Body类型
    /// </summary>
    public enum RequestBodyTypeEnum
    {
        JSON = 0,
        XML = 1,
        QueryString = 2
    }
}
