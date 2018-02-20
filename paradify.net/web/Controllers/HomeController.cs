using System.Web.Mvc;
using web.Filters;

namespace web.Controllers
{
    [ParadifyAuthorization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Installed()
        {
            return View();
        }
    }
}