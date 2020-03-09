namespace WebHook.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using WebHook.Controllers.Filters;
    using WebHook.Utility;
    using WebHook.Utility.Extension;

    public class GitHubController : BaseController
    {
        [HttpGet, HttpPost]
        [InvokeActionFilter]
        public HttpResponseMessage EventNotify()
        {
            var Body = HttpContext.Current.Request.GetParametersFromBody();

            LogHelper.Debug(JSON.Serialize(Body));

            return base.CallBack(Body);
        }
    }
}
