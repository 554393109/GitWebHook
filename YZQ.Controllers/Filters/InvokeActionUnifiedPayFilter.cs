using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YZQ.Utility;
using YZQ.Utility.Extension;
using YZQ.Utility.HttpClientUtils;
using YZQ.Utility.UniqueID;

namespace YZQ.Controllers.Filters
{
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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Executing)
                return;

            this.dt_begin = DateTime.Now;
            this.ReqId = YZQ.Utility.UniqueID.Generate_19.GetReqId(this.dt_begin);
            var rq_Invoke = string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", this.dt_begin);                           // 调用时间

            var Scheme = this.GetScheme(filterContext);                                                            // 获取调用接口的协议
            var Authority = this.GetAuthority(filterContext);                                                      // 获取调用接口的域名系统 (DNS) 主机名或 IP 地址和端口号
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               // Controller
            var ActionName = filterContext.ActionDescriptor.ActionName;
            var Target = this.GetTarget(filterContext);                                                            // Target
            var HttpMethod = filterContext.HttpContext.Request.HttpMethod;                                         // 请求方式
            var UserAgent = filterContext.HttpContext.Request.UserAgent;                                           // 用户代理
            var Duration = "0";                                                                                    // 耗时
            var IP = Globals.ClientIP;                                                                             // IP
            var ContentLength = filterContext.HttpContext.Request.ContentLength == 0
                ? filterContext.HttpContext.Request.TotalBytes.ToString()
                : filterContext.HttpContext.Request.ContentLength.ToString();                                         // 请求内容长度

            var ContextContent = this.ExecutingContent
                ? this.GetRequestContent(filterContext)
                : "当前接口忽略记录请求报文详情";                                                                     // 请求报文

            var param = new Hashtable {
                //param.Add("Id", Generate_19.Generate().ToString());                                                       //后置
                { "ReqId", this.ReqId },
                { "Scheme", Scheme },
                { "Authority", Authority },
                { "ControllerName", ControllerName },
                { "Target", Target },
                { "HttpMethod", HttpMethod },
                { "Duration", Duration },
                { "ContentLength", ContentLength },
                { "IP", IP },
                //paraments.Add("ServerIP", this.ServerIP);
                { "ContextType", "0" },
                { "UserAgent", UserAgent },
                { "rq_Invoke", rq_Invoke },
                { "ContextContent", ContextContent }
            };

            this.Send(param);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!this.Executed)
                return;

            var dt_end = DateTime.Now;
            var Scheme = this.GetScheme(filterContext);                                                            // 获取调用接口的协议
            var Authority = this.GetAuthority(filterContext);                                                      // 获取调用接口的域名系统 (DNS) 主机名或 IP 地址和端口号
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               // Controller
            var ActionName = filterContext.ActionDescriptor.ActionName;                                            // Action
            var Target = this.GetTarget(filterContext);                                                            // Target
            var HttpMethod = filterContext.HttpContext.Request.HttpMethod;                                         // 请求方式
            var IP = Globals.ClientIP;                                                                             // IP
            var Duration = MathHelper.PointRight((dt_end - this.dt_begin).TotalMilliseconds, 0).ToString();        // 耗时
            var UserAgent = filterContext.HttpContext.Request.UserAgent;                                           // 用户代理
            var ContextContent = this.ExecutedContent
                ? this.GetResponseContent(filterContext)
                : "当前接口忽略记录响应报文详情";                                                                     //响应报文

            var param = new Hashtable {
                //param.Add("Id", Generate_19.Generate().ToString());                                                       // 后置
                { "ReqId", this.ReqId },
                { "Scheme", Scheme },
                { "Authority", Authority },
                { "ControllerName", ControllerName },
                { "Target", Target },
                { "HttpMethod", HttpMethod },
                { "Duration", Duration },
                { "ContentLength", ContextContent.Length.ToString() },
                { "IP", IP },
                //paraments.Add("ServerIP", this.ServerIP);
                { "ContextType", "1" },
                { "UserAgent", UserAgent },
                { "rq_Invoke", string.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", dt_end) },
                { "ContextContent", ContextContent }
            };

            this.Send(param);
        }



        #region private Methods

        /// <summary>
        /// 获取调用接口的协议
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetScheme(ControllerContext filterContext)
        {
            var _Scheme = string.Empty;
            HttpContextBase context = filterContext.HttpContext;

            try
            {
                _Scheme = context.Request.Url.Scheme;
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
        private string GetAuthority(ControllerContext filterContext)
        {
            var _Authority = string.Empty;
            HttpContextBase context = filterContext.HttpContext;

            try
            {
                _Authority = context.Request.Url.Authority;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return _Authority;
        }

        /// <summary>
        /// 获取的真实接口
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetTarget(ActionExecutingContext filterContext)
        {
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               //Controller
            var ActionName = filterContext.ActionDescriptor.ActionName;                                            //Action
            var api = string.Empty;

            try
            {
                var method = filterContext.HttpContext.Request["method"].ValueOrEmpty();

                if (method.IsNullOrWhiteSpace())
                    api = ActionName;
                else
                    api = method.Trim();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            var _Target = string.Format("{0}/{1}", ControllerName, api);
            return _Target;
        }

        /// <summary>
        /// 获取的真实接口
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetTarget(ActionExecutedContext filterContext)
        {
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               //Controller
            var ActionName = filterContext.ActionDescriptor.ActionName;                                            //Action
            var api = string.Empty;

            try
            {
                var method = filterContext.HttpContext.Request["method"].ValueOrEmpty();

                if (method.IsNullOrWhiteSpace())
                    api = ActionName;
                else
                    api = method.Trim();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            var _Target = string.Format("{0}/{1}", ControllerName, api);
            return _Target;
        }

        /// <summary>
        /// 获取请求内容
        /// Json序列化
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetRequestContent(ControllerContext filterContext)
        {
            HttpContextBase context = filterContext.HttpContext;
            var param = new Hashtable();

            try
            {
                //QueryString
                foreach (string item in context.Request.QueryString.AllKeys)
                {
                    if (string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(context.Request.QueryString.ToString()))
                        return context.Request.QueryString.ToString().UrlUnescape();
                    else
                        param[item] = context.Request.QueryString[item];
                }

                //Form
                foreach (string item in context.Request.Form.AllKeys)
                {
                    if (string.IsNullOrWhiteSpace(item) && !string.IsNullOrWhiteSpace(context.Request.Form.ToString()))
                        return context.Request.Form.ToString().UrlUnescape();
                    else
                        param[item] = context.Request.Form[item];
                }

                #region InputStream只能一次性读取，Filter中不能Read

                //InputStream只能一次性读取，Filter中不能Read
                //if (context.Request.InputStream.CanRead && context.Request.InputStream.Length > 0)
                //{
                //    string str_InputStream = null;
                //    //接收从微信后台POST过来的数据
                //    using (Stream inputStream = context.Request.InputStream)
                //    {
                //        int count = 0;
                //        byte[] buffer = new byte[1024];
                //        StringBuilder builder = new StringBuilder();
                //        try
                //        {
                //            while ((count = inputStream.Read(buffer, 0, 1024)) > 0)
                //                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                //            str_InputStream = builder.ToString();           //读出报文
                //        }
                //        catch (Exception ex)
                //        {
                //            LogHelper.Error(ex);
                //        }
                //        finally
                //        {
                //            inputStream.Flush();
                //            inputStream.Close();
                //            inputStream.Dispose();
                //        }
                //    }
                //    if (!string.IsNullOrWhiteSpace(str_InputStream))
                //        param["_InputStream_Value"] = str_InputStream;
                //}

                #endregion InputStream只能一次性读取，Filter中不能Read
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return JSON.Serialize(param);
        }

        /// <summary>
        /// 获取响应内容
        /// Json序列化
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private string GetResponseContent(ActionExecutedContext filterContext)
        {
            var response = string.Empty;
            if (filterContext.Result is ContentResult)
                response = (filterContext.Result as ContentResult).Content;

            return response;
        }

        private void Send(Hashtable paraments)
        {
            //var ReqId = HttpContext.Current.Session.SessionID;
            //var InvokeRecordUrl = AppConfig.GetValue_Cache("InvokeRecordUrl");                                   //接口调用记录Url
            //if (InvokeRecordUrl.IsNullOrWhiteSpace()
            //    || InvokeRecordUrl.Equals("local", StringComparison.OrdinalIgnoreCase)
            //    || InvokeRecordUrl.Equals(".", StringComparison.OrdinalIgnoreCase))
            //{
            //    Base.BusinessFactory.InvokeRecord.Add(paraments);
            //}
            //else
            //{
            //    var mq_msg = new MQMsg(UnifiedPayKey.MQ_Consumer_Log_Invoke_Uri) { id = ReqId, lable = "InvokeActionUnifiedPayFilter", data = JSON.Serialize(paraments.UrlEscape()) };
            //    RabbitMQHelper.PublishAsync(UnifiedPayKey.MQ_Log, mq_msg);
            //}

            LogHelper.Info(JSON.Serialize(paraments));
        }

        #endregion private Methods
    }
}
