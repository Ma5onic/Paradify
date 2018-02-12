using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using web.Filters;
using web.Services;

namespace web.Controllers
{
    [ParadifyAuthorization]

    public class RecommendationController : BaseController
    {
        private readonly IParadifyService _paradifyService;
        private readonly ITokenCookieService _tokenCookieService;
        private readonly IHistoryService _historyService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IPlaylistService _playlistService;


        public RecommendationController(IParadifyService paradifyService, ITokenCookieService tokenCookieService,
            IHistoryService historyService, IUserService userService, ISessionService sessionService, IPlaylistService playlistService) : base(sessionService)
        {
            _paradifyService = paradifyService;
            _tokenCookieService = tokenCookieService;
            _historyService = historyService;
            _userService = userService;
            _sessionService = sessionService;
            _playlistService = playlistService;
        }

        public ActionResult Playlist(string trackId, string artistId)
        {
            Token token = ViewBag.Token;

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            var recommendations = api.GetRecommendations(
                new List<string>() { artistId }, null
                , new List<string>() { trackId }
                );

            if (recommendations.Tracks.Count > 0)
            {
                return PartialView("~/Views/SearchP/_RecommendedSongList.cshtml",
               recommendations.Tracks);
            }

            return null;

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

        private PrivateProfile GetMe(Token token)
        {
            return _userService.GetMe(token);
        }


    }
}