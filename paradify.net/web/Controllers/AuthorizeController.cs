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

        public ActionResult Index(string url)
        {
            if (!string.IsNullOrEmpty(url))
                _sessionService.SetReturnUrl(url);

            return
                Redirect(
                    string.Format(SpotifyVariables.authorizeUrlFormat,
                    Constants.ClientId, Server.UrlEncode(Constants.RedirectUri), Constants.Scope, Constants.StateKey)
                    );
        }

        //public ActionResult LoginWithChromeExtension(string q, string t)
        //{
        //    return
        //        Redirect(
        //            string.Format(SpotifyVariables.authorizeUrlFormat,
        //            Constants.ClientId, Server.UrlEncode(Constants.RedirectUri), Constants.Scope, Constants.StateKey)
        //            );
        //}
    }
}