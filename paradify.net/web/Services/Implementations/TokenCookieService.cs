using System;
using SpotifyAPI.Web.Models;
using web.IoC;
using SpotifyAPI.Web.Auth;

namespace web.Services.Implementations
{
    public class TokenCookieService : ITokenCookieService
    {
        public void SetToken(string accessToken, string refreshToken, int expiresIn)
        {
            CookieManager.WriteCookie("access_token", accessToken, DateTime.Now.AddSeconds(expiresIn));
            CookieManager.WriteCookie("refresh_token", refreshToken, DateTime.Now.AddYears(1));
        }

        public void DeleteToken()
        {
            CookieManager.WriteCookie("access_token", null, DateTime.Now.AddSeconds(-1));
            CookieManager.WriteCookie("refresh_token", null, DateTime.Now.AddSeconds(-1));
        }

        public void SetToken(Token token)
        {
            SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);
        }

        public Token Get()
        {
            var token = GetTokenFromCookie();

            if (string.IsNullOrEmpty(token.AccessToken) && !string.IsNullOrEmpty(token.RefreshToken))
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = oldRefreshToken;
                SetToken(token);
            }

            return token;
        }

        public bool Signout()
        {
            CookieManager.DeleteCookie("access_token");
            CookieManager.DeleteCookie("refresh_token");
            return true;
        }

        public Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            var result = auth.RefreshToken(refreshToken, clientSecret);
            if (result != null)
                result.RefreshToken = refreshToken;

            return result;
        }

        private Token GetTokenFromCookie()
        {
            Token token = new Token
            {
                AccessToken = CookieManager.GetCookieValue("access_token"),
                RefreshToken = CookieManager.GetCookieValue("refresh_token"),
                TokenType = "Bearer"
            };

            return token;
        }
    }
}