using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.IoC;
using web.Services;

namespace web.Filters
{
    public abstract class BaseFilter : ActionFilterAttribute
    {
        private ITokenCookieService _tokenCookieService;
        private ISessionService _sessionService;

        public BaseFilter()
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
                    _sessionService.SetToken(token);
                }

                if (string.IsNullOrEmpty(token.AccessToken)
                    && string.IsNullOrEmpty(token.RefreshToken))
                {
                    _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                    RedirectToAuthorize2(filterContext);
                }
                else if (string.IsNullOrEmpty(token.AccessToken)
                    && !string.IsNullOrEmpty(token.RefreshToken))
                {
                    token = RefreshToken(token.RefreshToken, Constants.ClientSecret);

                    if (string.IsNullOrEmpty(token.AccessToken))
                    {
                        _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                        RedirectToAuthorize2(filterContext);
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

        public void ForceReset(ActionExecutingContext filterContext)
        {
            _tokenCookieService.DeleteToken();

            _sessionService.DeleteToken();

            ForceReset2(_sessionService);

            RedirectToAuthorize2(filterContext);
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

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            return _tokenCookieService.RefreshToken(refreshToken, clientSecret);
        }

        public abstract void ForceReset2(ISessionService sessionService);
        public abstract void RedirectToAuthorize2(ActionExecutingContext filterContext);
    }
}