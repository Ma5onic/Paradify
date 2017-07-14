using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface ISessionService
    {
        void SetReturnUrl(string url);
        string GetReturnUrl();
    }
}