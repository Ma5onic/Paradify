using System;
using System.Linq;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web;
using System.Web;

namespace web.Services.Implementations
{
    public class SessionService : ISessionService
    {
        public SessionService()
        {
        }

        public void SetReturnUrl(string q)
        {
            HttpContext.Current.Session["returnUrl"] = "~/Search/" + q;
        }

        public string GetReturnUrl()
        {
            var session = HttpContext.Current.Session["returnUrl"];

            return session == null ? "" : session.ToString();
        }
    }
}