using System.Web.Mvc;
using SpotifyAPI.Web.Models;
using web.Services;
using web.Enums;
using web.Filters;
using web.Models;
using System.Threading.Tasks;

namespace web.Controllers
{
    [ParadifyAuthorization]

    public class SearchPController : BaseController
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
            IHistoryService historyService, IUserService userService, ISessionService sessionService, IPlaylistService playlistService) : base(sessionService)
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

            SearchResult searchResult = new SearchResult();

            if (!_search.NullCheck())
            {

                Token token = ViewBag.Token;

                if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
                {
                    SetSearchReturnUrl(_search);

                    return RedirectToAuthorization();
                }

                PrivateProfile profile = GetMe(token);

                if (profile.IsNotAuthorized())
                {
                    return RedirectToAuthorization();
                }

                SearchItem searchItem = Search(_search, token);

                if (searchItem.IsNotAuthorized())
                {
                    return RedirectToAuthorization();
                }

                searchResult.SearchItem = searchItem;
                searchResult.query = _search;
                searchResult.track = _trackId;

                searchResult.Playlists = GetPlaylists(token, profile.Id);

                Task task = new Task(() =>
                {
                    AddSearchHistory(token, profile.Id);
                });

                task.Start();
            }

            return View("Index", searchResult);
        }

        private PrivateProfile GetMe(Token token)
        {
            return _userService.GetMe(token);
        }



        private Paging<SimplePlaylist> GetPlaylists(Token token, string profileId)
        {
            var playlist = _playlistService.GetPlaylists(token, profileId);

            if (playlist != null && playlist.Items.Count == 0)
            {
                FullPlaylist fullPlaylist = _paradifyService.CreatePlaylist(profileId, "Paradify Playlist", _tokenCookieService.Get());

                if (!string.IsNullOrEmpty(fullPlaylist.Id))
                {
                    playlist = _playlistService.GetPlaylists(_tokenCookieService, profileId);
                }
            }

            return playlist;
        }

        private void AddSearchHistory(Token token, string profileId)
        {
            if (profileId != null)
            {
                _historyService.AddSearchHistory(_search, _trackId, profileId, AppSource.WebSite);
            }
        }

        private SearchItem Search(string query, Token token)
        {
            return _paradifyService.SearchResult(query, token);
        }

        private void SetSearchReturnUrl(string search)
        {
            var returnUrl = Helper.SetSearchReturnUrl(RouteData.Values["controller"].ToString(), search);

            _sessionService.SetReturnUrl(returnUrl);
        }
    }
}