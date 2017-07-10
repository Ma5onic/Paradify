using System.Web.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Enums;
using web.Services;

namespace web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IParadifyService _paradifyService;
        private readonly ITokenService _tokenService;
        private readonly IHistoryService _historyService;
        private readonly IUserService _userService;

        public string _search { get; set; }
        public string _trackId { get; set; }

        public SearchController(IParadifyService paradifyService, ITokenService tokenService,
            IHistoryService historyService, IUserService userService)
        {
            _paradifyService = paradifyService;
            _tokenService = tokenService;
            _historyService = historyService;
            _userService = userService;
        }

        public ActionResult Index(string q, string t)
        {
            _search = q;
            _trackId = t;

            SetReturnUrl(_search, _trackId);

            if (string.IsNullOrEmpty(_search))
            {
                return RedirectToAction("Index", "Home");
            }

            Token token = _tokenService.GetToken();

            if (string.IsNullOrEmpty(token.AccessToken) && !string.IsNullOrEmpty(token.RefreshToken))
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = oldRefreshToken;
                _tokenService.SetToken(token);
            } else if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
            {
                Session["returnUrl"] = "~/Search?q=" + q + "&t=" + t;
                return RedirectToAction("Index", "Authorize");
            }

            SearchItem searchItem = Search(_search, token);

            PrivateProfile profile = GetMe(token);

            if (profile.Id == null && token.RefreshToken != null)
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = oldRefreshToken;
                _tokenService.SetToken(token);
                profile = GetMe(token);
            }
            SearchResult result = new SearchResult()
            {
                SearchItem = searchItem,
                query = _search,
                track = _trackId,
                Profile = profile,

            };

            if (profile.Id != null)
            {
                result.Playlists = GetPlaylists(token, profile.Id);
                if (result.Playlists.Items.Count == 0)
                {
                    FullPlaylist fullPlaylist = _paradifyService.CreatePlaylist(profile.Id, "Paradify Playlist", token);

                    if (!string.IsNullOrEmpty(fullPlaylist.Id))
                    {
                        result.Playlists = GetPlaylists(token, profile.Id);
                    }
                }
            }

            
            _userService.AddUser(profile);

            _historyService.AddSearchHistory(_search, _trackId, profile.Id, AppSource.WebSite);

            return View(result);
        }

        private void SetReturnUrl(string search, string trackId)
        {
            //TODO: Create Cookie
        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            return auth.RefreshToken(refreshToken, clientSecret);
        }

        private Paging<SimplePlaylist> GetPlaylists(Token token, string userId)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            Paging<SimplePlaylist> userPlaylists = api.GetUserPlaylists(userId, 50);
            return userPlaylists;
        }

        private PrivateProfile GetMe(Token token)
        {

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };

            PrivateProfile profile = api.GetPrivateProfile();
            return profile;
        }
     
        private SearchItem Search(string query, Token token)
        {
            return _paradifyService.SearchResult(query,token, 100);
        }
    }
}