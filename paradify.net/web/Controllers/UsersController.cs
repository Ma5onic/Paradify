using SpotifyAPI.Web.Models;
using System.Web.Http;
using web.Services;

namespace web.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public UsersController(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpGet]
        [Route("users/me")]
        public PrivateProfile Me()
        {
            Token token = _tokenService.Get();

            PrivateProfile profile = _userService.GetMe(token);

            return profile;
        }

        [HttpGet]
        [Route("users/signout")]
        public bool Signout()
        {
            return _tokenService.Signout();
        }
    }
}
