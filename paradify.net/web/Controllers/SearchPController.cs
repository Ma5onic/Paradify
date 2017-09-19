using System.Web.Mvc;
using SpotifyAPI.Web.Models;
using web.Services;
using web.Enums;
using SpotifyAPI.Web;
using web.Models;
using System.Linq;

namespace web.Controllers
{
    public class SearchPController : Controller
    {
        private readonly IParadifyService _paradifyService;
        private readonly ITokenService _tokenService;
        private readonly IHistoryService _historyService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;

        public string _search { get; set; }
        public string _trackId { get; set; }

        public SearchPController(IParadifyService paradifyService, ITokenService tokenService,
            IHistoryService historyService, IUserService userService, ISessionService sessionService)
        {
            _paradifyService = paradifyService;
            _tokenService = tokenService;
            _historyService = historyService;
            _userService = userService;
            _sessionService = sessionService;
        }

        public ActionResult Index(string q)
        {
            _search = q;
            _trackId = "";

            if (string.IsNullOrEmpty(_search))
            {
                return RedirectToAction("Index", "Home");
            }

            Token token = _tokenService.Get();

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

            PrivateProfile profile = _userService.GetMe(token);

            if (profile.Id != null)
            {
                _historyService.AddSearchHistory(_search, _trackId, profile.Id, AppSource.WebSite);
            }

            return View("Index2", result);
        }

        public ActionResult GetPlaylists()
        {
            Token token = _tokenService.Get();

            PrivateProfile profile = _userService.GetMe(token);

            var playlist = GetPlaylists(token, profile.Id);

            if (playlist != null && playlist.Items.Count == 0)
            {
                FullPlaylist fullPlaylist = _paradifyService.CreatePlaylist(profile.Id, "Paradify Playlist", token);

                if (!string.IsNullOrEmpty(fullPlaylist.Id))
                {
                    playlist = GetPlaylists(token, profile.Id);
                }
            }

            return PartialView("~/Views/SearchP/PlaylistPartial.cshtml", playlist.Items);
        }

        private SearchItem Search(string query, Token token)
        {
            return _paradifyService.SearchResult(query, token, 100);
        }

        private Paging<SimplePlaylist> GetPlaylists(Token token, string userId)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            Paging<SimplePlaylist> userPlaylists = api.GetUserPlaylists(userId, 50);
            if (userPlaylists != null)
            {
                userPlaylists.Items = userPlaylists.Items.Where(x => x.Owner.Id == userId).ToList();
            }
            return userPlaylists;
        }
    }
}