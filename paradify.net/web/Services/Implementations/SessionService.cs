using System.Web;
using web.Models;

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
            return null;
            //var session = HttpContext.Current.Session["resetedRefreshToken"];

            //return session == null ? null : session.ToString();
        }

        public void SetResetedRefreshToken(string value)
        {
            HttpContext.Current.Session["resetedRefreshToken"] = value;
        }

        public CustomToken GetToken()
        {
            return null;
            //CustomToken token = null;

            //object session = HttpContext.Current.Session["token"];

            //if (session != null)
            //{
            //    token = (CustomToken)session;

            //    CustomToken cokkie = new CustomToken
            //    {
            //        AccessToken = token.AccessToken,
            //        RefreshToken = token.RefreshToken,
            //        TokenType = token.TokenType,
            //        tokenCredentialType = token.tokenCredentialType
            //    };
            //}

            //return token;
        }

        public void SetToken(CustomToken token)
        {
            //HttpContext.Current.Session["token"] = token;
        }

        public void DeleteToken()
        {
            // HttpContext.Current.Session["token"] = null;
        }

        public void setSession<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public T getSession<T>(string key)
        {
            var sessionValue = HttpContext.Current.Session[key];

            if (sessionValue is T)
            {
                return (T)sessionValue;
            }

            return default(T);
        }

        public void DeleteSession(string key)
        {
            HttpContext.Current.Session[key] = null;
        }
    }
}