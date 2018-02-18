using SpotifyAPI.Web.Models;
using System.Web.Mvc;
using web.Filters;
using web.IoC;
using web.Services;

namespace web.Controllers
{

    [ParadifyAuthorization]
    public class HomeController : CustomControllerBase
    {
        private readonly IParadifyService _paradifyService;
        private readonly ITokenCookieService _tokenCookieService;
        private readonly IHistoryService _historyService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IPlaylistService _playlistService;
        public HomeController(IParadifyService paradifyService, ITokenCookieService tokenCookieService,
            IHistoryService historyService, IUserService userService, ISessionService sessionService, IPlaylistService playlistService) : base(paradifyService, tokenCookieService,
            historyService, userService, sessionService, playlistService)
        {
            _paradifyService = paradifyService;
            _tokenCookieService = tokenCookieService;
            _historyService = historyService;
            _userService = userService;
            _sessionService = sessionService;
            _playlistService = playlistService;
        }
        public ActionResult Index()
        {
            if (!CookieManager.IsCookieExist("firstVisit"))
            {
                CookieManager.WriteCookie("firstVisit", "1");
                ViewBag.firstVisit = 1;
            }

            Token token = ViewBag.Token;

            PrivateProfile profile = GetMe(token);

            if (profile.IsNotAuthorized())
            {
                return RedirectToAuthorization();
            }

            ViewBag.SavedTracks = GetSavedTracks(token);
            ViewBag.RecentlyPlayedTracks = GetUsersRecentlyPlayedTracks(token);

            return View();
        }

        [HttpGet]
        public ActionResult GetPlaylists()
        {
            Token token = ViewBag.Token;

            PrivateProfile profile = GetMe(token);

            var playlists = base.GetPlaylists(token, profile.Id);

            if (playlists != null && playlists.Items != null && playlists.Items.Count > 0)
            {
                return PartialView("~/Views/SearchP/_PlaylistList.cshtml", playlists.Items);
            }

            return null;
        }

        public ActionResult Installed()
        {
            return View();
        }

        private Paging<SavedTrack> GetSavedTracks(Token token)
        {
            return _paradifyService.GetSavedTracks(token);
        }
        private CursorPaging<PlayHistory> GetUsersRecentlyPlayedTracks(Token token)
        {
            return _paradifyService.GetUsersRecentlyPlayedTracks(token);
        }
        private PrivateProfile GetMe(Token token)
        {
            return _userService.GetMe(token);
        }
    }
}