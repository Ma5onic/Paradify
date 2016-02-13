using System.Web.Mvc;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Services;

namespace web.Controllers
{
    public class CallbackController : Controller
    {
        private readonly ITokenService _tokenService;

        public CallbackController(ITokenService tokenService)
        {
            _tokenService = tokenService;
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

            var returnUrl = Session["returnUrl"];


            _tokenService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);
            
            if (returnUrl != null && !string.IsNullOrEmpty(returnUrl.ToString()))
            {
                return Redirect(returnUrl.ToString());
            }

            return Redirect("/");
        }
    }
}