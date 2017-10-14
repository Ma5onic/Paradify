using SpotifyAPI.Web.Models;
using System.Web.Http;
using web.Services;

namespace web.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ITokenCookieService _tokenService;
        private readonly IUserService _userService;

        public UsersController(ITokenCookieService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpGet]
        [Route("users/me")]
        public PrivateProfile Me()
        {
            return _userService.GetMe(_tokenService);
        }

        [HttpGet]
        [Route("users/signout")]
        public bool Signout()
        {
            return _userService.Signout(_tokenService);
        }
    }
}
