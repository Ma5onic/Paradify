using System.Web.Mvc;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Services;

namespace web.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public CallbackController(ITokenService tokenService, IUserService userService, ISessionService sessionService)
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

            var returnUrl = _sessionService.GetReturnUrl();

            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);

            PrivateProfile profile = _userService.GetMe(token);

            _userService.AddUser(profile);

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
    }
}