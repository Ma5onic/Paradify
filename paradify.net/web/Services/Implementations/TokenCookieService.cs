using System;
using SpotifyAPI.Web.Models;
using web.IoC;
using SpotifyAPI.Web.Auth;
using web.Models;
using static web.Models.CustomToken;

namespace web.Services.Implementations
{
    public class TokenCookieService : ITokenCookieService
    {
        public void SetToken(string accessToken, string refreshToken, int expiresIn, TokenCredentialType tokenCredentialType)
        {
            CookieManager.WriteCookie("access_token", accessToken, DateTime.Now.AddSeconds(expiresIn).AddSeconds(-60));
            CookieManager.WriteCookie("refresh_token", refreshToken, DateTime.Now.AddYears(1));
            CookieManager.WriteCookie("token_type", tokenCredentialType.ToString(), DateTime.Now.AddYears(1));
        }

        public void DeleteToken()
        {
            CookieManager.WriteCookie("access_token", null, DateTime.Now.AddSeconds(-1));
            CookieManager.WriteCookie("refresh_token", null, DateTime.Now.AddSeconds(-1));
            CookieManager.WriteCookie("token_type", null, DateTime.Now.AddSeconds(-1));
        }

        public void SetToken(CustomToken token)
        {
            SetToken(token.AccessToken, token.RefreshToken, token.ExpiresIn, token.tokenCredentialType);
        }

        public CustomToken Get()
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

        public CustomToken CustomClientCredentialToken()
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
            CookieManager.DeleteCookie("token_type");
            return true;
        }

        public CustomToken RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            Token response;
            CustomToken result = null;
            try
            {
                response = auth.RefreshToken(refreshToken, clientSecret);

                result = response.ToCustomToken();

                if (result != null)
                    result.RefreshToken = refreshToken;

            }
            catch (Exception ex)
            {


            }



            return result;
        }

        private CustomToken GetTokenFromCookie()
        {
            CustomToken token = new CustomToken
            {
                AccessToken = CookieManager.GetCookieValue("access_token"),
                RefreshToken = CookieManager.GetCookieValue("refresh_token"),
                TokenType = "Bearer"
            };

            TokenCredentialType type = TokenCredentialType.Auth;
            Enum.TryParse(CookieManager.GetCookieValue("token_type"), out type);
            token.tokenCredentialType = type;

            return token;
        }
    }
}