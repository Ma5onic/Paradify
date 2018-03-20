using web.Models;

namespace web.Services
{
    public interface ISessionService
    {
        void SetReturnUrl(string url);
        string GetReturnUrl();
        string GetResetedRefreshToken();
        void SetResetedRefreshToken(string value);
        CustomToken GetToken();
        void SetToken(CustomToken token);
        void DeleteToken();
    }
}