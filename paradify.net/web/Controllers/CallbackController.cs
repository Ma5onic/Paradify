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
            var client_id = Constants.ClientId;
            var client_secret = Constants.ClientSecret;
            var redirect_uri = Constants.RedirectUri;
            var stateKey = Constants.StateKey;

            AutorizationCodeAuth auth = new AutorizationCodeAuth
            {
                ClientId = client_id,
                RedirectUri = redirect_uri,
                State = stateKey
            };

            Token token = auth.ExchangeAuthCode(code, client_secret);

            var returnUrl = _sessionService.GetReturnUrl();

            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);

            PrivateProfile profile =_userService.GetMe(token);

            _userService.AddUser(profile);

            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl.ToString()))
            {
                return Redirect(returnUrl.ToString());
            }

            return Redirect("/");
        }
    }
}