using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface ITokenCookieService
    {
        void SetToken(string accessToken, string refreshToken, int expiresIn);
        void SetToken(Token token);
        Token Get();
        bool Signout();
    }
}