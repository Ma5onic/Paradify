using SpotifyAPI.Web.Models;
using SpotifyAPI.Web;
using web.Repositories;

namespace web.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool AddUser(PrivateProfile profile)
        {
            if (profile.NullOrEmptyCheck())
                return false;

            try
            {
                if (_userRepository.IsUserExist(profile.Id))
                    return false;

                return _userRepository.AddUser(profile) > 0;
            }
            catch
            {

            }

            return false;
        }

        public PrivateProfile GetMe(ITokenCookieService tokenCookieService)
        {
            Token token = tokenCookieService.Get();

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };

            return api.GetPrivateProfile();
        }

        public PrivateProfile GetMe(Token token)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };

            return api.GetPrivateProfile();
        }

        public bool Signout(ITokenCookieService tokenCookieService)
        {
            return tokenCookieService.Signout();
        }
    }
}