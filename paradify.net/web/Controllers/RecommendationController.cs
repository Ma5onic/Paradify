using System.Web.Mvc;
using web.Filters;

namespace web.Controllers
{
    [ParadifyAuthorization]

    public class RecommendationController : BaseController
    {

        public RecommendationController()
        {

        }

        public ActionResult Playlist()
        {
            return View();
        }

    }
}