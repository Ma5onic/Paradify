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
            string resetedRefreshToken = _sessionService.GetResetedRefreshToken();


            if (resetedRefreshToken == null || resetedRefreshToken != "1" || CookieManager.GetCookieValue("resetedRefreshToken") != "1")
            {
                _tokenCookieService.DeleteToken();
                _sessionService.DeleteToken();

            }
            else
            {
                Token token = _sessionService.GetToken();
 
                if (token == null)
                {
                    token = _tokenCookieService.Get();
                    _sessionService.SetToken(token);
                }

                if (string.IsNullOrEmpty(token.AccessToken) 
                    && string.IsNullOrEmpty(token.RefreshToken))
                {
                    _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());


                }
                else if (string.IsNullOrEmpty(token.AccessToken) 
                    && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = RefreshToken(token.RefreshToken, Constants.ClientSecret);

                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                    }
                    else
                    {
                        _sessionService.SetToken(token);
                        _tokenCookieService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);
                    }
                }

                filterContext.Controller.ViewBag.Token = token;
            }

            base.OnActionExecuting(filterContext);
        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            return _tokenCookieService.RefreshToken(refreshToken, clientSecret);
        }
    }
}