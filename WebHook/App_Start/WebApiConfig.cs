using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebHook
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            // *********************************************************************************

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}",
                defaults: new { controller = "GitHub", action = "EventNotify" },
                constraints: new { controller = @"\w+", action = @"\w+" }
            );
        }
    }
}
