using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        /// <summary>
        /// 路由映射
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            ////异常
            //routes.MapRoute(
            //    name: "Error",
            //    url: "Error/{id}",
            //    defaults: new { controller = "Error", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "YZQ.Controllers.OtherCtl" }
            //);

            //routes.MapRoute(
            //    name: "QrPay",
            //    url: "QrPay/{code_number}",
            //    defaults: new { controller = "UnifiedPay", action = "QrPay" },
            //    constraints: new { /*controller = @"\w+",*/code_number = @"\w+" },
            //    namespaces: new[] { "YZQ.Controllers.UnifiedPayCtl" }
            //);

            //routes.MapRoute(
            //    name: "OldPhp_Path",
            //    url: "api/pufa/{action}",
            //    defaults: new { controller = "PuFa", action = "H5_WECHAT" },
            //    constraints: new { /*controller = @"\w+",*/action = @"\w+" },
            //    namespaces: new[] { "YZQ.Controllers.UnifiedPayCtl" }
            //);

            ////云后台
            //routes.MapRoute(
            //    name: "Admin",
            //    url: "admin/{action}",
            //    defaults: new { controller = "Admin", action = "Login" },
            //    constraints: new { /*controller = @"\w+",*/action = @"\w+" },
            //    namespaces: new[] { "YZQ.Controllers.AdminCtl" }
            //);

            ////经销商后台
            //routes.MapRoute(
            //    name: "Dealer",
            //    //url: "Dealer/Index",
            //    url: "Dealer/{action}",
            //    defaults: new { controller = "Dealer", action = "Index" },
            //    constraints: new { /*controller = @"\w+",*/action = @"\w+" },
            //    namespaces: new[] { "YZQ.Controllers.DealerCtl" }
            //);

            // WebHook
            routes.MapRoute(
                name: "WebHook",
                url: "WebHook/{action}",
                defaults: new { controller = "WebHook", action = "GitHub" },
                constraints: new { /*controller = @"\w+",*/action = @"\w+" },
                namespaces: new[] { "YZQ.Controllers.WebHookCtl" }
            );


            //// 默认路由
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "WebHook", action = "GitHub" },
            //    constraints: new { controller = @"\w+", action = @"\w+" },
            //    namespaces: new[] { "YZQ.Controllers.WebHookCtl" }
            //);

        }
    }
}