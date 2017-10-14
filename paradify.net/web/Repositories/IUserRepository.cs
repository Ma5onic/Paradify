using SpotifyAPI.Web.Models;

namespace web.Repositories
{
    public interface IUserRepository
    {
        int AddUser(PrivateProfile profile);
        bool IsUserExist(string profileId);
    }
}