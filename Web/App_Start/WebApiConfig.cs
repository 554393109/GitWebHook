using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;
using System.IO;
using YZQ.Utility;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        /// <summary>
        /// 初始化系统目录
        /// </summary>
        /// <param name="config"></param>
        public static void Init(HttpConfiguration config)
        {
            if (!Directory.Exists(ApplicationInfo.TempPath))
                Directory.CreateDirectory(ApplicationInfo.TempPath);
        }
    }
}
