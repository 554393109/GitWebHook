namespace WebHook.Controllers.Filters
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using WebHook.Utility;
    using WebHook.Utility.Extension;
    using WebHook.Utility.HttpClientUtils;
    using WebHook.Utility.UniqueID;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    /// <summary>
    /// 访问日志过滤器
    /// </summary>
    public class InvokeActionUnifiedPayFilter : ActionFilterAttribute
    {
        private string ReqId = default(string);
        private DateTime dt_begin = DateTime.MinValue;
        private bool Executing, ExecutingContent,
            Executed, ExecutedContent;

        /// <summary>
        /// 访问日志过滤器
        /// </summary>
        /// <param name="_Executing">是否记录请求</param>
        /// <param name="_ExecutingContent">是否记录请求报文详情</param>
        /// <param name="_Executed">是否记录响应</param>
        /// <param name="_ExecutedContent">是否记录响应报文详情</param>
        public InvokeActionUnifiedPayFilter(bool _Executing = true, bool _ExecutingContent = true, bool _Executed = true, bool _ExecutedContent = true)
        {
            this.Executing = _Executing;
            this.ExecutingContent = _ExecutingContent;
            this.Executed = _Executed;
            this.ExecutedContent = _ExecutedContent;
        }

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            if (!this.Executing)
                return;

            //this.dt_begin = DateTime.Now;
            //this.ReqId = YZQ.Utility.UniqueID.Generate_19.GetReqId(this.dt_begin);
            //var rq_Invoke = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", this.dt_begin);                           // 调用时间

            //var Scheme = this.GetScheme(filterContext);                                                            // 获取调用接口的协议
            //var Authority = this.GetAuthority(filterContext);                                                      // 获取调用接口的域名系统 (DNS) 主机名或 IP 地址和端口号
            //var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               // Controller
            //var ActionName = filterContext.ActionDescriptor.ActionName;
            //var Target = this.GetTarget(filterContext);                                                            // Target
            //var HttpMethod = filterContext.HttpContext.Request.HttpMethod;                                         // 请求方式
            //var UserAgent = filterContext.HttpContext.Request.UserAgent;                                           // 用户代理
            //var Duration = "0";                                                                                    // 耗时
            //var IP = Globals.ClientIP;                                                                             // IP
            //var ContentLength = filterContext.HttpContext.Request.ContentLength == 0
            //    ? filterContext.HttpContext.Request.TotalBytes.ToString()
            //    : filterContext.HttpContext.Request.ContentLength.ToString();                                         // 请求内容长度

            //var ContextContent = this.ExecutingContent
            //    ? this.GetRequestContent(filterContext)
            //    : "当前接口忽略记录请求报文详情";                                                                     // 请求报文

            //var param = new Hashtable {
            //    //param.Add("Id", Generate_19.Generate().ToString());                                                       //后置
            //    { "ReqId", this.ReqId },
            //    { "Scheme", Scheme },
            //    { "Authority", Authority },
            //    { "ControllerName", ControllerName },
            //    { "Target", Target },
            //    { "HttpMethod", HttpMethod },
            //    { "Duration", Duration },
            //    { "ContentLength", ContentLength },
            //    { "IP", IP },
            //    //paraments.Add("ServerIP", this.ServerIP);
            //    { "ContextType", "0" },
            //    { "UserAgent", UserAgent },
            //    { "rq_Invoke", rq_Invoke },
            //    { "ContextContent", ContextContent }
            //};

            //this.Send(param);
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            if (!this.Executed)
                return;

            //var dt_end = DateTime.Now;
            //var Scheme = this.GetScheme(filterContext);                                                            // 获取调用接口的协议
            //var Authority = this.GetAuthority(filterContext);                                                      // 获取调用接口的域名系统 (DNS) 主机名或 IP 地址和端口号
            //var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               // Controller
            //var ActionName = filterContext.ActionDescriptor.ActionName;                                            // Action
            //var Target = this.GetTarget(filterContext);                                                            // Target
            //var HttpMethod = filterContext.HttpContext.Request.HttpMethod;                                         // 请求方式
            //var IP = Globals.ClientIP;                                                                             // IP
            //var Duration = MathHelper.PointRight((dt_end - this.dt_begin).TotalMilliseconds, 0).ToString();        // 耗时
            //var UserAgent = filterContext.HttpContext.Request.UserAgent;                                           // 用户代理
            //var ContextContent = this.ExecutedContent
            //    ? this.GetResponseContent(filterContext)
            //    : "当前接口忽略记录响应报文详情";                                                                     //响应报文

            //var param = new Hashtable {
            //    //param.Add("Id", Generate_19.Generate().ToString());                                                       // 后置
            //    { "ReqId", this.ReqId },
            //    { "Scheme", Scheme },
            //    { "Authority", Authority },
            //    { "ControllerName", ControllerName },
            //    { "Target", Target },
            //    { "HttpMethod", HttpMethod },
            //    { "Duration", Duration },
            //    { "ContentLength", ContextContent.Length.ToString() },
            //    { "IP", IP },
            //    //paraments.Add("ServerIP", this.ServerIP);
            //    { "ContextType", "1" },
            //    { "UserAgent", UserAgent },
            //    { "rq_Invoke", string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", dt_end) },
            //    { "ContextContent", ContextContent }
            //};

            //this.Send(param);
        }



        #region private Methods

        /// <summary>
        /// 获取调用接口的协议
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetScheme(HttpActionContext filterContext)
        {
            var _Scheme = string.Empty;

            try
            {
                _Scheme = filterContext.Request.RequestUri.Scheme;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return _Scheme;
        }

        /// <summary>
        /// 获取调用接口的域名系统 (DNS) 主机名或 IP 地址和端口号
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetAuthority(HttpActionContext filterContext)
        {
            var _Authority = filterContext.Request.RequestUri.Authority;
            return _Authority;
        }

        /// <summary>
        /// 获取的真实接口
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetTarget(HttpActionContext filterContext)
        {
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               //Controller
            var ActionName = filterContext.ActionDescriptor.ActionName;                                            //Action
            var _Target = string.Format("{0}/{1}", ControllerName, ActionName);
            return _Target;
        }

        /// <summary>
        /// 获取的真实接口
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetTarget(HttpActionExecutedContext filterContext)
        {
            //var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               //Controller
            //var ActionName = filterContext.ActionDescriptor.ActionName;                                            //Action

            //var _Target = string.Format("{0}/{1}", ControllerName, ActionName);
            //return _Target;
            return string.Empty;
        }

        /// <summary>
        /// 获取请求内容
        /// Json序列化
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetRequestContent(HttpActionContext filterContext)
        {
            var param = new Hashtable();

            //try
            //{
            //    //QueryString
            //    foreach (string item in filterContext.Request.QueryString.AllKeys)
            //    {
            //        if (string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(filterContext.Request.QueryString.ToString()))
            //            return filterContext.Request.QueryString.ToString().UrlUnescape();
            //        else
            //            param[item] = filterContext.Request.QueryString[item];
            //    }

            //    //Form
            //    foreach (string item in filterContext.Request.Form.AllKeys)
            //    {
            //        if (string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(filterContext.Request.Form.ToString()))
            //            return filterContext.Request.Form.ToString().UrlUnescape();
            //        else
            //            param[item] = filterContext.Request.Form[item];
            //    }


            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(ex);
            //}

            return JSON.Serialize(param);
        }

        /// <summary>
        /// 获取响应内容
        /// Json序列化
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetResponseContent(HttpActionExecutedContext filterContext)
        {
            var response = string.Empty;
            //if (filterContext.Response.Content is ContentResult)
            //    response = (filterContext.Response.Content as ContentResult).Content;

            return response;
        }

        private void Send(Hashtable paraments)
        {
            LogHelper.Info(JSON.Serialize(paraments));
        }

        #endregion private Methods
    }
}
