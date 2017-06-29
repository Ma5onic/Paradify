using System;
using SpotifyAPI.Web.Models;
using web.IoC;

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
    }
}