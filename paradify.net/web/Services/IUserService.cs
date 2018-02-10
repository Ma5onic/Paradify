using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IUserService
    {
        bool AddUser(PrivateProfile profile);
        PrivateProfile GetMe(ITokenCookieService tokenCookieService);
        PrivateProfile GetMe(Token token);
        bool Signout(ITokenCookieService tokenCookieService);
        
    }
}