using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IUserService
    {
        bool AddUser(PrivateProfile profile);
        PrivateProfile GetMe(ITokenCookieService tokenCookieService);
        bool Signout(ITokenCookieService tokenCookieService);
        
    }
}