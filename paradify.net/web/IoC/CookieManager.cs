using System;
using System.Collections.Generic;
using System.Web;

namespace web.IoC
{
    public class CookieManager
    {
        private static readonly string CookieDomain = Constants.Domain;

        private static HttpCookieCollection CookiesRequest
        {
            get
            {
                return HttpContext.Current.Request.Cookies;
            }
        }


        private static HttpCookieCollection CookiesResponse
        {
            get
            {
                HttpContext.Current.Response.Buffer = true;
                return HttpContext.Current.Response.Cookies;
            }
        }

        private static string DecodeCookieValue(string value)
        {
            return Uri.UnescapeDataString(value);
        }

        public static void WriteCookie(string cookieName, string value, Dictionary<string, object> extraData = null, bool modifyName = true)
        {
            WriteCookie(cookieName, value, DateTime.Now.AddDays(7), extraData, modifyName);
        }

        public static void WriteCookie(string cookieName, string value, DateTime expireDate, Dictionary<string, object> extraData = null, bool modifyName = false)
        {
            HttpCookie cookie = new HttpCookie(cookieName)
            {
                Expires = expireDate,
                Path = "/"
            };

            cookie.Values.Add("Id", value);

            cookie.Domain = CookieDomain;

            CookiesResponse.Add(cookie);
        }

        public static bool IsCookieExist(string cookieName, bool modifyName = true)
        {

            bool isCookieExist = false;
            HttpCookie userCookie = CookiesRequest.Get(cookieName);

            if (userCookie != null)
            {
                isCookieExist = true;
            }

            return isCookieExist;
        }

        public static void DeleteCookie(string cookieName, bool modifyName = true)
        {

            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpCookie c = new HttpCookie(cookieName) {Expires = DateTime.Now.AddDays(-7)};
                c.Values.Add("Id", "---");
                CookiesResponse.Add(c);
            }

        }

        public static string GetCookieValue(string cookieName, string key = "Id", bool modifyName = true)
        {

            string cookieValue = string.Empty;

            if (IsCookieExist(cookieName, false))
            {
                HttpCookie cookie = CookiesRequest.Get(cookieName);
                if (cookie != null) cookieValue = cookie[key] ?? string.Empty;
            }

            cookieValue = DecodeCookieValue(cookieValue);

            return cookieValue;
        }
    }
}
