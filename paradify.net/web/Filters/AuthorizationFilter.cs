using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using System.Web.Routing;
using web.IoC;
using web.Services;

namespace web.Filters
{
    public class ParadifyAuthorization : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;


        public ParadifyAuthorization()
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

                CookieManager.WriteCookie("resetedRefreshToken", "1");

                RedirectToAuthorize(filterContext);

            }
            else
            {
                var token = _tokenCookieService.Get();

                if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
                {
                    _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                    RedirectToAuthorize(filterContext);
                }
                else if (string.IsNullOrEmpty(token.AccessToken) && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                        _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                        RedirectToAuthorize(filterContext);
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

        private void RedirectToAuthorize(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Authorize" },
                        { "action", "Index" },
                        { "url", filterContext.HttpContext.Request.Url.ToString() } });
        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            return auth.RefreshToken(refreshToken, clientSecret);
        }
    }
}