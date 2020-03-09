using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;

namespace WebHook.Controllers
{
    public abstract class BaseController : ApiController
    {
        //private string _ControllerName = string.Empty;
        //private string _ActionName = string.Empty;

        ///// <summary>
        ///// 获得所有请求参数
        ///// </summary>
        ///// <param name="NeedUrlDecode">是否需要解码</param>
        ///// <returns></returns>
        //protected Hashtable GetParameters(bool NeedUrlDecode = true)
        //{
        //    var param = new Hashtable();
        //    foreach (string key in Request.QueryString.AllKeys)
        //    {
        //        try
        //        {
        //            if (!key.IsNullOrWhiteSpace()/* && !string.IsNullOrWhiteSpace(val)*/)
        //            {
        //                var val = Request.QueryString[key] ?? string.Empty;
        //                param[key] = NeedUrlDecode
        //                    ? val.UrlUnescape().Trim()
        //                    : val.Trim();
        //            }
        //        }
        //        catch { }
        //    }
        //    foreach (string key in Request.Form.AllKeys)
        //    {
        //        try
        //        {
        //            if (!key.IsNullOrWhiteSpace()/* && !string.IsNullOrWhiteSpace(val)*/)
        //            {
        //                var val = Request.Form[key] ?? string.Empty;
        //                param[key] = NeedUrlDecode
        //                    ? val.UrlUnescape().Trim()
        //                    : val.Trim();
        //            }
        //        }
        //        catch { }
        //    }

        //    return param;
        //}

        ///// <summary>
        ///// 获得指定请求参数
        ///// </summary>
        ///// <param name="key">键</param>
        ///// <param name="defaultValue">当值为null时使用的默认值。</param>
        ///// <returns>值</returns>
        //protected string GetParameter(string key, string defaultValue = null)
        //{
        //    var value = default(string);

        //    try
        //    {
        //        value = Request.Form[key] ?? Request.QueryString[key];
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex);
        //    }

        //    if (value == null)
        //        value = defaultValue;

        //    return value;
        //}

        ///// <summary>
        ///// 获得请求参数
        ///// </summary>
        //protected string GetStreamParameters()
        //{
        //    var str_param = string.Empty;

        //    if (Request.InputStream.CanRead && Request.InputStream.Length > 0)
        //    {
        //        // 接收从微信后台POST过来的数据
        //        using (Stream inputStream = Request.InputStream)
        //        {
        //            var count = 0;
        //            var buffer = new byte[1024];
        //            var builder = new StringBuilder();

        //            try
        //            {
        //                while ((count = inputStream.Read(buffer, 0, 1024)) > 0)
        //                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));

        //                str_param = builder.ToString();         //读出报文
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Error(ex);
        //            }
        //            finally
        //            {
        //                inputStream.Flush();
        //                inputStream.Close();
        //                inputStream.Dispose();
        //            }
        //        }
        //    }

        //    return str_param;
        //}

        ///// <summary>
        ///// 页面信息
        ///// </summary>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    _ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    _ActionName = filterContext.ActionDescriptor.ActionName;
        //}

        ///// <summary>
        ///// 页面信息
        ///// </summary>
        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    if (!(filterContext.Result is JsonResult) //Ajax请求
        //        && !(filterContext.Result is ContentResult) //字符串内容返回
        //        && !(filterContext.Result is RedirectToRouteResult) //重定向
        //        )
        //    {

        //    }
        //    //允许跨域访问  
        //    //filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //    //filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET, OPTIONS,DELETE,PUT");
        //    //filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers","x-requested-with,content-type");
        //    base.OnActionExecuted(filterContext);
        //}


        //protected ActionResult CallBackXML(object response)
        //{
        //    string result;

        //    if (response == null)
        //        result = string.Empty;
        //    else if (response.GetType().Equals(typeof(string)))
        //        result = response.ToString();
        //    else
        //        result = Utility.XML.Serialize(response);

        //    return Content(
        //        content: result,
        //        contentType: "application/xml",
        //        contentEncoding: Encoding.UTF8);
        //}


        ///// <summary>
        ///// 序列化返回
        ///// </summary>
        ///// <param name="response"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //protected ActionResult CallBack(object response, string type = "json")
        //{
        //    string result;
        //    string contentType = "text/plain";
        //    string _type = string.IsNullOrWhiteSpace(type)
        //        ? "json"
        //        : type.ToLower();

        //    if (response == null)
        //        result = string.Empty;
        //    else if (response.GetType().Equals(typeof(string)))
        //        result = response.ToString();
        //    else
        //    {
        //        switch (_type)
        //        {
        //            case "json":
        //                result = JSON.Serialize(response);
        //                break;
        //            case "xml":
        //                result = XML.Serialize(response);
        //                break;
        //            case "text":
        //            default:
        //                result = JSON.Serialize(response);
        //                break;
        //        }
        //    }

        //    switch (_type)
        //    {
        //        case "json":
        //            contentType = "application/json";
        //            break;
        //        case "xml":
        //            contentType = "application/xml";
        //            break;
        //        case "text":
        //        default:
        //            contentType = "text/plain";
        //            break;
        //    }

        //    return Content(
        //        content: result,
        //        contentType: contentType,
        //        contentEncoding: Encoding.UTF8);
        //}

    }
}
