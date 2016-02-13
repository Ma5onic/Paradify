using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IUserService
    {
        int AddUser(PrivateProfile profile);
    }
}