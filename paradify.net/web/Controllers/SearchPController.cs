using System.Web.Mvc;
using SpotifyAPI.Web.Models;
using web.Services;
using web.Enums;
using web.Filters;
using web.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace web.Controllers
{
    
    public class SearchPController : CustomControllerBase
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

        [FilterClientToken]
        public ActionResult Index(string q)
        {
            _search = q;
            _trackId = "";

            SearchResult searchResult = new SearchResult();

            if (!_search.NullCheck())
            {
                _search = _search.Decode();

                ViewBag.Title = string.Format("{0} - {1}", Constants.SingleTitle, _search);

                CustomToken token = ViewBag.Token;

                //if (string.IsNullOrEmpty(token.AccessToken) && string.IsNullOrEmpty(token.RefreshToken))
                //{
                //    //SetSearchReturnUrl(_search);

                //    //return RedirectToAuthorization();
                //    return null;
                //}
                PrivateProfile profile = new PrivateProfile();

                if (token.tokenCredentialType == CustomToken.TokenCredentialType.Client)
                {
                    profile = GetMe(token);
                }

                if (profile.IsNotAuthorized())
                {
                   // return RedirectToAuthorization();
                }

                SearchItem searchItem = Search(_search, token);

                if (searchItem.IsNotAuthorized())
                {
                    //return RedirectToAuthorization();
                }

                if (searchItem != null && searchItem.Tracks != null && 
                    searchItem.Tracks.Items != null && searchItem.Tracks.Items.Count == 0)
                {
                    var tempSearch = Regex.Replace(_search, "\\([^\\]]*\\)", "");
                    searchItem = Search(tempSearch, token);
                }

                searchResult.SearchItem = searchItem;
                searchResult.query = _search;
                searchResult.track = _trackId;

                Task task = new Task(() =>
                {
                    AddSearchHistory(token, profile.Id);
                });

                task.Start();
            }

            return View("Index", searchResult);
        }

        [FilterClientToken]
        private PrivateProfile GetMe(Token token)
        {
            return _userService.GetMe(token);
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