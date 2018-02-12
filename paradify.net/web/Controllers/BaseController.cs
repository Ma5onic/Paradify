using System.Web.Mvc;
using web.Services;

namespace web.Controllers
{
    public class BaseController : Controller
    {
        private readonly ISessionService _sessionService;

        public BaseController(ISessionService sessionService)
        {
            this._sessionService = sessionService;
        }
        public ActionResult RedirectToAuthorization()
        {
            _sessionService.SetReturnUrl(HttpContext.Request.Url.ToString());

            return RedirectToAction("Index", "Authorize");
        }
    }
}