using System.Web.Mvc;
using web.IoC;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!CookieManager.IsCookieExist("firstVisit"))
            {
                CookieManager.WriteCookie("firstVisit", "1");
                ViewBag.firstVisit = 1;
            }

            return View();
        }
    }
}