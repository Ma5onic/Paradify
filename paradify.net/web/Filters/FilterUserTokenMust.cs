using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.IoC;
using web.Services;

namespace web.Filters
{

    public class FilterUserTokenMust : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;

        public FilterUserTokenMust()
        {
            var container = ContainerManager.Container;
            _tokenCookieService = container.Resolve<ITokenCookieService>();
            _sessionService = container.Resolve<ISessionService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsForceReset(filterContext))
            {
                ForceReset(filterContext);
            }
            else
            {
                Token token = _sessionService.GetToken();

                if (token == null)
                {
                    token = _tokenCookieService.Get();
                    _sessionService.SetToken(token.ToCustomToken());
                }

                if (string.IsNullOrEmpty(token.AccessToken)
                    && string.IsNullOrEmpty(token.RefreshToken))
                {
                    _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                    RedirectToAuthorize(filterContext);
                }
                else if (string.IsNullOrEmpty(token.AccessToken)
                    && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = RefreshToken(token.RefreshToken, Constants.ClientSecret);

                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                        _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                        RedirectToAuthorize(filterContext);
                    }
                    else
                    {
                        _sessionService.SetToken(token.ToCustomToken());

                        _tokenCookieService.SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn, Models.CustomToken.TokenCredentialType.Auth);
                    }
                }

                filterContext.Controller.ViewBag.Token = token;
            }

            base.OnActionExecuting(filterContext);
        }

        public void ForceReset(ActionExecutingContext filterContext)
        {
            _tokenCookieService.DeleteToken();

            _sessionService.DeleteToken();

            _sessionService.SetResetedRefreshToken("1");

            CookieManager.WriteCookie("resetedRefreshToken", "1");

            RedirectToAuthorize(filterContext);
        }

        private bool IsForceReset(ActionExecutingContext filterContext)
        {
            string value = "1";
            bool result = false;
            string sessionResetedRefreshToken = _sessionService.GetResetedRefreshToken();

            //if no session
            if (sessionResetedRefreshToken == null)
            {
                //check if it is in the cookie
                if (CookieManager.GetCookieValue("resetedRefreshToken") != value)
                {
                    result = true;
                }
            }
            else if (sessionResetedRefreshToken != value)
            {
                result = true;
            }

            return result;
        }

        private void RedirectToAuthorize(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { { "controller", "Authorize" },
                        { "action", "Index" },
                        { "url", filterContext.HttpContext.Request.Url.ToString() } });

        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            return _tokenCookieService.RefreshToken(refreshToken, clientSecret);
        }
    }
}