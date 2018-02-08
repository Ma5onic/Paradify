using System.Web.Mvc;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Services;

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

            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);

            PrivateProfile profile = _userService.GetMe(_tokenService);

            _userService.AddUser(profile);

            var returnUrl = _sessionService.GetReturnUrl();

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        //public ActionResult CallbackWithChromeExtension(string code = null)
        //{
        //    AutorizationCodeAuth auth = new AutorizationCodeAuth
        //    {
        //        ClientId = Constants.ClientId,
        //        RedirectUri = Constants.RedirectUri,
        //        State = Constants.StateKey
        //    };

        //    Token token = auth.ExchangeAuthCode(code, Constants.ClientSecret);

        //    _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);

        //    PrivateProfile profile = _userService.GetMe(_tokenService);

        //    _userService.AddUser(profile);

        //    return Redirect(string.Format("/?access_token={0}&refresh_token={1}", token.AccessToken, token.RefreshToken));
        //}
    }
}