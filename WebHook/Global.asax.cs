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
                //�������������ͼƬ�������Ĵ���
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
        /// �������
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
                    msg = string.Format("����������а�������ȫ���ַ������磺html���룬�ű�����);{0}", ex.Message)
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
                    msg = string.Format("������������������ϵ����Ա��{0}", ex.Message)
                }));
            }
        }

        /// <summary>
        /// ��ʼ��ϵͳĿ¼
        /// </summary>
        /// <param name="config"></param>
        private static void InitDirectory(HttpConfiguration config)
        {
            if (!Directory.Exists(ApplicationInfo.TempPath))
                Directory.CreateDirectory(ApplicationInfo.TempPath);
        }
    }
}
