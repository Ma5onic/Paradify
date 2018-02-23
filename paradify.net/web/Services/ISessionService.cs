using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface ISessionService
    {
        void SetReturnUrl(string url);
        string GetReturnUrl();
        string GetResetedRefreshToken();
        void SetResetedRefreshToken(string value);
        Token GetToken();
        void SetToken(Token token);
        void DeleteToken();
    }
}