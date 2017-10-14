using System.Web.Mvc;
using SpotifyAPI.Web.Models;
using web.Services;
using web.Enums;
using web.Models;

namespace web.Controllers
{
    public class SearchPController : Controller
    {
        private readonly IParadifyService _paradifyService;
        private readonly ITokenCookieService _tokenCookieService;
        private readonly IHistoryService _historyService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IPlaylistService _playlistService;

        public string _search { get; set; }
        public string _trackId { get; set; }

        public SearchPController(IParadifyService paradifyService, ITokenCookieService tokenCookieService,
            IHistoryService historyService, IUserService userService, ISessionService sessionService, IPlaylistService playlistService)
        {
            _paradifyService = paradifyService;
            _tokenCookieService = tokenCookieService;
            _historyService = historyService;
            _userService = userService;
            _sessionService = sessionService;
            _playlistService = playlistService;
        }

        public ActionResult Index(string q)
        {
            _search = q;
            _trackId = "";

            if (string.IsNullOrEmpty(_search))
            {
                return RedirectToAction("Index", "Home");
            }

            Token token = _tokenCookieService.Get();

            if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
            {
                _sessionService.SetReturnUrl("~/" + RouteData.Values["controller"] + "?q=" + _search);

                return RedirectToAction("Index", "Authorize");
            }

            SearchItem searchItem = Search(_search, token);

            SearchResult result = new SearchResult()
            {
                SearchItem = searchItem,
                query = _search,
                track = _trackId,
            };

            PrivateProfile profile = _userService.GetMe(_tokenCookieService);

            if (profile.Id != null)
            {
                _historyService.AddSearchHistory(_search, _trackId, profile.Id, AppSource.WebSite);
            }

            return View("Index2", result);
        }

        public ActionResult GetPlaylists()
        {
            PrivateProfile profile = _userService.GetMe(_tokenCookieService);
        
            var playlist = _playlistService.GetPlaylists(_tokenCookieService, profile.Id);

            if (playlist != null && playlist.Items.Count == 0)
            {
                FullPlaylist fullPlaylist = _paradifyService.CreatePlaylist(profile.Id, "Paradify Playlist", _tokenCookieService.Get());

                if (!string.IsNullOrEmpty(fullPlaylist.Id))
                {
                    playlist = _playlistService.GetPlaylists(_tokenCookieService, profile.Id);
                }
            }

            return PartialView("~/Views/SearchP/PlaylistPartial.cshtml", playlist.Items);
        }

        private SearchItem Search(string query, Token token)
        {
            return _paradifyService.SearchResult(query, token, 100);
        }
    }
}