using System.Web.Mvc;
using web.IoC;
using web.Models;
using web.Services;
using static web.Models.CustomToken;

namespace web.Filters
{
    public class FilterUserToken : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;

        public FilterUserToken()
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

                if (!string.IsNullOrEmpty(token.AccessToken)
                    && token.tokenCredentialType == TokenCredentialType.Auth)
                    _sessionService.SetToken(token.ToCustomToken(token.tokenCredentialType));
            }

            if (token == null || (string.IsNullOrEmpty(token.AccessToken)
                && !string.IsNullOrEmpty(token.RefreshToken)))
            {
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);

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

            if (token.tokenCredentialType != TokenCredentialType.Auth)
            {
                filterContext.Controller.ViewBag.Token = null;
            }
            else
                filterContext.Controller.ViewBag.Token = token;

            base.OnActionExecuting(filterContext);
        }

        private CustomToken RefreshToken(string refreshToken, string clientSecret)
        {
            return _tokenCookieService.RefreshToken(refreshToken, clientSecret);
        }
    }
}