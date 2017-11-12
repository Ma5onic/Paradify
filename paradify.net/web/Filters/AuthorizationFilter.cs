using System.Web.Mvc;
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
            var token = _tokenCookieService.Get();

            if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
            {
                _sessionService.SetReturnUrl(filterContext.HttpContext.Request.Url.ToString());

                filterContext.Result = new RedirectToRouteResult("Authorize", null);
            }

            filterContext.Controller.ViewBag.Token = token;

            base.OnActionExecuting(filterContext);
        }
    }
}