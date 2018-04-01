using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web.Models;
using static web.Models.CustomToken;

namespace web
{
    public static class Extensions
    {
        public static bool NullCheck<T>(this T obj)
        {

            return obj == null || (obj.GetType() == typeof(string) && string.IsNullOrEmpty(obj as string));
        }

        public static bool NullOrEmptyCheck(this PrivateProfile profile)
        {
            return (profile == null || string.IsNullOrEmpty(profile.Id));
        }

        public static bool IsNotAuthorized(this BasicModel model)
        {
            return (model != null && model.Error != null && model.Error.Status == 401);
        }

        public static string Decode(this string source)
        {
            source = HttpUtility.UrlDecode(HttpUtility.HtmlDecode(source));

            return source;
        }

        public static bool IsTokenEmpty(this Token token)
        {

            return token == null || string.IsNullOrEmpty(token.AccessToken);
        }

        public static CustomToken ToCustomToken(this Token token, TokenCredentialType tokenCredentialType = TokenCredentialType.Auth)
        {
            return new CustomToken()
            {
                AccessToken = token.AccessToken,
                CreateDate = token.CreateDate,
                TokenType = token.TokenType,
                Error = token.Error,
                ErrorDescription = token.ErrorDescription,
                ExpiresIn = token.ExpiresIn,
                RefreshToken = token.RefreshToken,
                tokenCredentialType = tokenCredentialType
            };
        }

        public static T RandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default(T);

            Random random = new Random(DateTime.Now.Millisecond);
            return list.ElementAt(random.Next(0, list.Count));
        }
    }
}