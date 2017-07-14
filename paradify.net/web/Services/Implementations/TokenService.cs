using System;
using SpotifyAPI.Web.Models;
using web.IoC;
using SpotifyAPI.Web.Auth;

namespace web.Services.Implementations
{
    public class TokenService : ITokenService
    {
        public Token GetToken()
        {
            Token token = new Token
            {
                AccessToken = CookieManager.GetCookieValue("access_token"),
                RefreshToken = CookieManager.GetCookieValue("refresh_token"),
                TokenType = "Bearer"
            };
            return token;
        }

        public void SetToken(string accessToken, string refreshToken, int expiresIn)
        {
            CookieManager.WriteCookie("access_token", accessToken, DateTime.Now.AddSeconds(expiresIn));
            CookieManager.WriteCookie("refresh_token", refreshToken, DateTime.Now.AddYears(1));
        }

        public void SetToken(Token token)
        {
            SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn);
        }

        public Token Get()
        {
            var token = GetToken();

            if (string.IsNullOrEmpty(token.AccessToken) && !string.IsNullOrEmpty(token.RefreshToken))
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = token.RefreshToken;
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

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            return auth.RefreshToken(refreshToken, clientSecret);
        }
    }
}