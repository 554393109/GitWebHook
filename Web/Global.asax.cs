using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using YZQ.Utility;
using System.IO;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebApiConfig.Init(GlobalConfiguration.Configuration);

            LogHelper.Info("Application_Start");
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //集成模式可用
#if !DEBUG

            HttpApplication app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                app.Context.Response.Headers.Remove("Server");
            }

#endif

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = Server.GetLastError();
            if (lastError != null)
            {
                Exception ex = lastError.GetBaseException();
                Server.ClearError();
                //过滤浏览器请求图片所产生的错误
                if (ex is HttpException)
                {
                    string absolutePath = Request.Url.AbsolutePath.ToLower();
                    string ext = Path.HasExtension(absolutePath)
                        ? Path.GetExtension(absolutePath)
                        : string.Empty;
                    if (new string[] { ".ico", ".gif", ".jpg", ".png", ".bmp", ".cur", ".map" }.Contains(ext.ToLower()))
                        return;
                }

                LogHelper.Error(ex);
                this.SystemError(ex);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            LogHelper.Info("Application_End");
        }

        /// <summary>
        /// 判断是否Ajax请求
        /// </summary>
        /// 
        private bool IsAjaxRequest()
        {
            bool flag = false;
            if (Request == null)
                return flag;

            if ((Request.Headers != null
                && !string.IsNullOrWhiteSpace(Request.Headers["X-Requested-With"])
                && Request.Headers["X-Requested-With"].Equals("XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
                ||
                (Request != null
                && !string.IsNullOrWhiteSpace(Request["X-Requested-With"])
                && Request["X-Requested-With"].Equals("XMLHttpRequest", StringComparison.OrdinalIgnoreCase)))
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 处理错误
        /// </summary>
        private void SystemError(Exception ex)
        {
            bool isAjaxRequest = this.IsAjaxRequest();
            if (isAjaxRequest)
            {
                Response.Expires = -1;
                Response.CacheControl = "no-cache";
                Response.Charset = "utf-8";
                Response.ContentType = "application/json";
            }

            if (ex is HttpRequestValidationException)
            {
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(new {
                        state = "FAIL",
                        msg = string.Format("请求的内容中包含不安全的字符！（如：html代码，脚本语言);{0}", ex.Message)
                    }));
                }
                else
                {
                    Response.Redirect("/Error/hazard");
                }
            }
            else if (ex is HttpCompileException)
            {
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(new {
                        state = "FAIL",
                        msg = ex.Message
                    }));
                }
                else
                {
                    Response.Redirect("/Error/500");
                }
            }
            else if (ex is SqlException)
            {
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(new {
                        state = "FAIL",
                        msg = ex.Message
                    }));
                }
                else
                {
                    Response.Redirect("/Error/sql");
                }
            }
            else if (ex is ExternalException)
            {
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(new {
                        state = "FAIL",
                        msg = ex.Message
                    }));
                }
                else
                {
                    Response.Redirect("/Error/404");
                }
            }
            else
            {
                if (isAjaxRequest)
                {
                    Response.Write(JSON.Serialize(new {
                        state = "FAIL",
                        msg = string.Format("服务器发生错误，请联系管理员！{0}", ex.Message)
                    }));
                }
                else
                {
                    Response.Redirect("/Error/500");
                }
            }
        }
    }
}