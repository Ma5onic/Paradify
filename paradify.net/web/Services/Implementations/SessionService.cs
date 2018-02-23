using SpotifyAPI.Web.Models;
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

        public string GetResetedRefreshToken()
        {
            var session = HttpContext.Current.Session["resetedRefreshToken"];

            return session == null ? null : session.ToString();
        }

        public void SetResetedRefreshToken(string value)
        {
            HttpContext.Current.Session["resetedRefreshToken"] = value;
        }

        public Token GetToken()
        {
            Token token = null;

            object session = HttpContext.Current.Session["token"];

            if (session != null)
            {
                token = (Token)session;

                Token cokkie = new Token
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    TokenType = token.TokenType
                };
            }

            return token;
        }

        public void SetToken(Token token)
        {
            HttpContext.Current.Session["token"] = token;
        }

        public void DeleteToken()
        {
            HttpContext.Current.Session["token"] = null;
        }
    }
}