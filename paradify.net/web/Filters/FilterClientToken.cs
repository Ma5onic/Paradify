using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.IoC;
using web.Models;
using web.Services;

namespace web.Filters
{
    public class FilterClientToken : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;

        public FilterClientToken()
        {
            var container = ContainerManager.Container;
            _tokenCookieService = container.Resolve<ITokenCookieService>();
            _sessionService = container.Resolve<ISessionService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CustomToken token = _sessionService.GetToken();

            if (token == null)
            {
                token = _tokenCookieService.Get();

                if (!string.IsNullOrEmpty(token.AccessToken))
                    _sessionService.SetToken(token.ToCustomToken(token.tokenCredentialType));
            }

            if (token == null || (string.IsNullOrEmpty(token.AccessToken)
                    && !string.IsNullOrEmpty(token.RefreshToken)))
            {
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
            }

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                token = GetClientToken();

                if (token == null || string.IsNullOrEmpty(token.AccessToken))
                {
                    //Just Die
                }
                else
                {
                    _tokenCookieService.SetToken(token);
                    _sessionService.SetToken(token);
                }
            }

            filterContext.Controller.ViewBag.Token = token;

            base.OnActionExecuting(filterContext);
        }

        private CustomToken GetClientToken()
        {
            SpotifyAPI.Web.Auth.ClientCredentialsAuth clientCredentialsAuth =
                   new SpotifyAPI.Web.Auth.ClientCredentialsAuth();

            clientCredentialsAuth.ClientId = Constants.ClientId;
            clientCredentialsAuth.ClientSecret = Constants.ClientSecret;
            Token response = clientCredentialsAuth.DoAuth();
            CustomToken token = response.ToCustomToken(CustomToken.TokenCredentialType.Client);

            return token;
        }

        private CustomToken RefreshToken(string refreshToken, string clientSecret)
        {
            return _tokenCookieService.RefreshToken(refreshToken, clientSecret);
        }
    }
}