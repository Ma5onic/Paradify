using System.Web.Mvc;
using web.App_LocalResources;
using web.Services;

namespace web.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly ISessionService _sessionService;
        public readonly ITokenCookieService _tokenCookieService;
        public AuthorizeController(ISessionService sessionService, ITokenCookieService tokenCookieService)
        {
            _sessionService = sessionService;
            _tokenCookieService = tokenCookieService;
        }



        public ActionResult Index(string url, bool? fromIFrame)
        {
            if (fromIFrame.HasValue && fromIFrame.Value)
            {
                _sessionService.setSession<bool>("fromIframe", fromIFrame.Value);
            }

            if (!string.IsNullOrEmpty(url))
                _sessionService.SetReturnUrl(url);
            else
                if (Request.UrlReferrer != null)
                _sessionService.SetReturnUrl(Request.UrlReferrer.ToString());

            return
                Redirect(
                    string.Format(SpotifyVariables.authorizeUrlFormat,
                    Constants.ClientId, Server.UrlEncode(Constants.RedirectUri), Constants.Scope, Constants.StateKey)
                    );
        }

        public ActionResult Logout()
        {
            _tokenCookieService.Signout();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CloseIframe()
        {
            return View();
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