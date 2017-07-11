using System.Web;

namespace web.Services.Implementations
{
    public class SessionService : ISessionService
    {
        public SessionService()
        {
        }

        public void SetReturnUrl(string url)
        {
            HttpContext.Current.Session["returnUrl"] = url;
        }

        public string GetReturnUrl()
        {
            var session = HttpContext.Current.Session["returnUrl"];

            return session == null ? "" : session.ToString();
        }
    }
}