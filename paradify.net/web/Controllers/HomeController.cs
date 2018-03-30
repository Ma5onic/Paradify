using log4net;
using SpotifyAPI.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using web.Filters;
using web.Models;
using web.Services;

namespace web.Controllers
{
    [FilterClientToken]
    public class HomeController : CustomControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

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

        public ActionResult Index(string country = null)
        {
            HomeModel model = new HomeModel();

            CustomToken token = ViewBag.Token;

            if (token.IsTokenEmpty())
            {
                return View();
            }

            try
            {
                Task tasktNewReleasedTracks = Task.Factory.StartNew(() =>
                    {
                        model.NewReleasedTracks = GetNewReleasedTracks(token, GetCountryCodeOrDefault(country));

                        if (model.NewReleasedTracks != null && model.NewReleasedTracks.Paging.Items != null
                            && model.NewReleasedTracks.Paging.Items.Any())
                        {
                            var track = model.NewReleasedTracks.Paging.Items.First();

                            model.Recommendations = GetRecommendations(token, track.Id, track.Artists.First().Id);
                        }
                    });

                Task taskRecentlyPlayed = Task.Factory.StartNew(() =>
                {
                    model.RecentlyPlayedTracks = GetRecentlyPlayedTracks(token);
                });

                Task taskSavedTracks = Task.Factory.StartNew(() =>
                {
                    model.SavedTracks = GetSavedTracks(token);
                });

                Task.WaitAll(new[] { tasktNewReleasedTracks, taskRecentlyPlayed, taskSavedTracks });
            }
            catch (Exception ex)
            {
                log.Error("HomeController => Index()", ex);
            }

            return View(model);
        }

        public ActionResult Installed()
        {
            return View();
        }

        private string GetCountryCodeOrDefault(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Constants.DefaultCountryCode;
            }

            var firstCountry = Constants.CountryCodes.FirstOrDefault(c => c.Code == country.ToUpperInvariant());

            return firstCountry == null ? Constants.DefaultCountryCode : firstCountry.Code;
        }

        private CursorPaging<PlayHistory> GetRecentlyPlayedTracks(CustomToken token)
        {
            return _paradifyService.GetUsersRecentlyPlayedTracks(token, 10);
        }

        private Paging<SavedTrack> GetSavedTracks(CustomToken token)
        {
            return _paradifyService.GetSavedTracks(token, 10);
        }

        private CustomSimpleTrack GetNewReleasedTracks(CustomToken token, string countryCode)
        {
            return _paradifyService.GetNewReleasedTracks(token, countryCode);
        }

        private Recommendations GetRecommendations(CustomToken token, string trackId, string artistId)
        {
            return _paradifyService.GetRecommendations(token, trackId, artistId);
        }
    }
}