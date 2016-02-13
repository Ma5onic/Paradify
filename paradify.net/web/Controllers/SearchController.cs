using System.Web.Mvc;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Token = web.Models.Token;

namespace web.Controllers
{
    public class SearchController : Controller
    {
        public string _q { get; set; }
        public string _t { get; set; }
        public ActionResult Index(string q, string t)
        {
            SetQuery(q);
            SetTrackId(t);
            SetReturnUrl(q, t);

            Token token = GetToken();
            if (string.IsNullOrEmpty(q))
            {
                return RedirectToAction("Index", "Home");
            }

            SearchItem searchItem = Search(q, t, token.AccessToken);
            SearchResult searchResult = new SearchResult();
            //searchResult.dataMe = searchItem;
            //searchResult.dataTracks = searchItem;
            //searchResult.dataPlaylist = searchItem;
            //searchResult.err = searchItem;
            //searchResult.q = searchItem;
            //searchResult.t = searchItem;
            //searchResult.newCreatedPlaylist = searchItem;
                 
            return View(searchItem);
        }
        private void SetQuery(string q)
        {
            _q = q;
        }
        private void SetTrackId(string t)
        {
            _t = t;
        }
        private SearchItem Search(string s, string s1, string accessToken)
        {
            SpotifyWebAPI spotifyWebApi = new SpotifyWebAPI() { UseAuth = false};
            SearchItem searchItems = spotifyWebApi.SearchItems(s, SearchType.Track, 100, 0);
        }

        private Token GetToken()
        {
            Token token = new Token();
            var httpSessionStateBase = HttpContext.Session;
            if (httpSessionStateBase == null) return token;
            token.AccessToken = httpSessionStateBase["accessToken"] != null ? httpSessionStateBase["accessToken"].ToString() : null;
            token.RefreshToken = httpSessionStateBase["refreshToken"] != null ? httpSessionStateBase["refreshToken"].ToString() : null;
            return token;
        }

        private void SetReturnUrl(string s, string s1)
        {

        }




    }
}