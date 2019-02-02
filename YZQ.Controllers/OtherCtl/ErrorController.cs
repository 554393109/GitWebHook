using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using YZQ.Controllers.Base;

namespace YZQ.Controllers.OtherCtl
{
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ErrorController : BaseController
    {
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                id = string.Empty;

            return View(id as object);
        }

        public ActionResult Mobile()
        {
            return View();
        }
    }
}