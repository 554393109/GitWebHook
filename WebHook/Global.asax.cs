namespace WebHook
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.Http;
    using System.Web.Routing;
    using WebHook.Utility;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            InitDirectory(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            LogHelper.Info("Application_Start");
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

        protected void Application_End(object sender, EventArgs e)
        {
            LogHelper.Info("Application_End");
        }


        /// <summary>
        /// 处理错误
        /// </summary>
        private void SystemError(Exception ex)
        {
            Response.Expires = -1;
            Response.CacheControl = "no-cache";
            Response.Charset = "utf-8";
            Response.ContentType = "application/json";

            if (ex is HttpRequestValidationException)
            {
                Response.Write(JSON.Serialize(new
                {
                    state = "FAIL",
                    msg = string.Format("请求的内容中包含不安全的字符！（如：html代码，脚本语言);{0}", ex.Message)
                }));
            }
            else if (ex is HttpCompileException)
            {
                Response.Write(JSON.Serialize(new
                {
                    state = "FAIL",
                    msg = ex.Message
                }));
            }
            else if (ex is SqlException)
            {
                Response.Write(JSON.Serialize(new
                {
                    state = "FAIL",
                    msg = ex.Message
                }));
            }
            else if (ex is ExternalException)
            {
                Response.Write(JSON.Serialize(new
                {
                    state = "FAIL",
                    msg = ex.Message
                }));
            }
            else
            {
                Response.Write(JSON.Serialize(new
                {
                    state = "FAIL",
                    msg = string.Format("服务器发生错误，请联系管理员！{0}", ex.Message)
                }));
            }
        }

        /// <summary>
        /// 初始化系统目录
        /// </summary>
        /// <param name="config"></param>
        private static void InitDirectory(HttpConfiguration config)
        {
            if (!Directory.Exists(ApplicationInfo.TempPath))
                Directory.CreateDirectory(ApplicationInfo.TempPath);
        }
    }
}
