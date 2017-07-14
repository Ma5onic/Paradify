using System.Web.Mvc;
using web.App_LocalResources;
using web.Services;

namespace web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ISessionService _sessionService;

        public AuthorizeController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ActionResult Index(string q, string t)
        {
            if (Request.UrlReferrer != null && string.IsNullOrEmpty(_sessionService.GetReturnUrl()))
                _sessionService.SetReturnUrl(Request.UrlReferrer.ToString());

            return
                Redirect(
                    string.Format(SpotifyVariables.authorizeUrlFormat,
                    Constants.ClientId, Server.UrlEncode(Constants.RedirectUri), Constants.Scope, Constants.StateKey)
                    );
        }
    }
}