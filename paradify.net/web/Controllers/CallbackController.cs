using System.Web.Mvc;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Services;
using static web.Models.CustomToken;

namespace web.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ITokenCookieService _tokenService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public CallbackController(ITokenCookieService tokenService, IUserService userService, ISessionService sessionService)
        {
            _tokenService = tokenService;
            _userService = userService;
            _sessionService = sessionService;
        }

        public ActionResult Index(string code = null)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth
            {
                ClientId = Constants.ClientId,
                RedirectUri = Constants.RedirectUri,
                State = Constants.StateKey
            };

            Token token = auth.ExchangeAuthCode(code, Constants.ClientSecret);

            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn, TokenCredentialType.Auth);
            _sessionService.SetToken(token.ToCustomToken(TokenCredentialType.Auth));

            bool chromeToken = _sessionService.getSession<bool>("ChromeToken");

            if (chromeToken)
            {
                _sessionService.DeleteSession("ChromeToken");

                return Redirect(string.Format("~/?access_token={0}&refresh_token={1}&expires_in={2}", token.AccessToken, token.RefreshToken, token.ExpiresIn));
            }

            if (_sessionService.getSession<bool>("fromIframe"))
            {
                _sessionService.DeleteSession("fromIframe");

                return RedirectToAction("CloseIframe", "Authorize");
            }

            var returnUrl = _sessionService.GetReturnUrl();

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl))
            {
                if (!returnUrl.Contains(Url.RouteUrl("Authorize")))
                    return Redirect(returnUrl);
            }

            return Redirect("~/");
        }
    }
}