using System.Web.Mvc;

namespace web
{
    public class MvcHelper : Controller
    {
        public RedirectToRouteResult RedirectToAuthorization(string controllerName, string searhQuery)
        {
            return RedirectToAction("Index", "Authorize");
        }
    }
}