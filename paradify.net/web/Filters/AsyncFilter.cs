using System.Web.Mvc;
using web.Services;

namespace web.Filters
{
    public class AsyncFilter : BaseFilter
    {
        public AsyncFilter()
        {
        }

        public override void ForceReset2(ISessionService sessionService)
        {
        }

        public override void RedirectToAuthorize2(ActionExecutingContext filterContext)
        {
        }    
    }
}