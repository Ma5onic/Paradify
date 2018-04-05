using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IUserService
    {
        PrivateProfile GetMe(ITokenCookieService tokenCookieService);
        PrivateProfile GetMe(Token token);
        bool Signout(ITokenCookieService tokenCookieService);   
    }
}