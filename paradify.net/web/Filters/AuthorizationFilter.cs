using System.Web.Mvc;
using web.IoC;
using web.Services;

namespace web.Filters
{
    public class ParadifyAuthorization : BaseFilter
    {
        public override void ForceReset2(ISessionService sessionService)
        {
            sessionService.SetResetedRefreshToken("1");

            CookieManager.WriteCookie("resetedRefreshToken", "1");
        }

        public override void RedirectToAuthorize2(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { { "controller", "Authorize" },
                        { "action", "Index" },
                        { "url", filterContext.HttpContext.Request.Url.ToString() } });

        }
        //public ParadifyAuthorization(ITokenCookieService tokenCookieService,
        //    ISessionService sessionService) : base(tokenCookieService, sessionService)
        //{
        //    _tokenCookieService = tokenCookieService;
        //    _sessionService = sessionService;
        //}
    }
}