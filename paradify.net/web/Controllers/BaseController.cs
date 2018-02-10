using System.Web.Mvc;

namespace web.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult RedirectToAuthorization()
        {
            return RedirectToAction("Index", "Authorize");
        }
    }
}