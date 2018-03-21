using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.Filters;
using web.Models;
using web.Services;

namespace web.Controllers
{
    public class UsersController : Controller
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
        [FilterUserToken]
        public ActionResult Me()
        {
            CustomToken token = ViewBag.Token;

            return PartialView("~/Views/Shared/_Login.cshtml", token.IsTokenEmpty() ? null :
                _userService.GetMe(_tokenService));
        }

        [HttpGet]
        [Route("users/signout")]
        public bool Signout()
        {
            return _userService.Signout(_tokenService);
        }
    }
}
