using System.Web.Mvc;

namespace web.Controllers
{
    public class AuthorizeController : Controller
    {
        public ActionResult Index(string q, string t)
        {
            if (Request.UrlReferrer != null && Session["returnUrl"] == null) Session["returnUrl"] = Request.UrlReferrer.ToString();

            return
                Redirect(
                    string.Format(
                        @"https://accounts.spotify.com/en/authorize?client_id={0}&response_type=code&redirect_uri={1}&scope={2}&state={3}",
                    Constants.ClientId, Server.UrlEncode(Constants.RedirectUri), Constants.Scope, Constants.StateKey)
                    );
        }
    }
}