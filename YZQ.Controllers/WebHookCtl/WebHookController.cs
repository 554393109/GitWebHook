using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using YZQ.Controllers.Base;
using YZQ.Controllers.Filters;

namespace YZQ.Controllers.WebHookCtl
{
    [ValidateInput(false)]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class WebHookController : BaseController
    {
        [HttpPost, HttpGet]
        [InvokeActionUnifiedPayFilter]
        public ActionResult GitHub()
        {
            return CallBack(null);
        }

        [HttpPost, HttpGet]
        [InvokeActionUnifiedPayFilter]
        public ActionResult Gitee()
        {
            return CallBack(null);
        }
    }
}
