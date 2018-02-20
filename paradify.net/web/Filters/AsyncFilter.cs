using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.IoC;
using web.Services;

namespace web.Filters
{
    public class AsyncFilter : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;


        public AsyncFilter()
        {
            var container = ContainerManager.Container;
            _tokenCookieService = container.Resolve<ITokenCookieService>();
            _sessionService = container.Resolve<ISessionService>();

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (CookieManager.GetCookieValue("resetedRefreshToken") != "1")
            {
                _tokenCookieService.DeleteToken();

               

               

            }
            else
            {
                var token = _tokenCookieService.Get();

                if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
                {
                    _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                   
                }
                else if (string.IsNullOrEmpty(token.AccessToken) && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                    }
                    else
                    {
                        _tokenCookieService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);
                    }
                }

                filterContext.Controller.ViewBag.Token = token;
            }






            base.OnActionExecuting(filterContext);
        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            return auth.RefreshToken(refreshToken, clientSecret);
        }
    }
}