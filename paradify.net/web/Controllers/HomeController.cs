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
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        private readonly IPlaylistService _playlistService;

        public HomeController(IParadifyService paradifyService, ITokenCookieService tokenCookieService,
            IUserService userService, ISessionService sessionService, IPlaylistService playlistService) : base(paradifyService, tokenCookieService,
            userService, sessionService, playlistService)
        {
            _paradifyService = paradifyService;
            _tokenCookieService = tokenCookieService;

            _userService = userService;
            _sessionService = sessionService;
            _playlistService = playlistService;
        }

        public ActionResult Index(string country = null)
        {
            return View();
        }

        private static string GetCountryOfProfile(PrivateProfile profile, string profileCountryCode)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.Country))
            {
                profileCountryCode = profile.Country;
            }

            return profileCountryCode;
        }

        public ActionResult Installed()
        {
            return View();
        }

        private string GetCountryCodeOrDefault(string country, string profileCountry)
        {
            string result = Constants.DefaultCountryCode;

            string countryCode = string.IsNullOrEmpty(country) ? profileCountry : country;

            if (countryCode == null)
            {
                return result;
            }

            var firstCountry = Constants.CountryCodes.FirstOrDefault(c => c.Code == countryCode.ToUpperInvariant());

            if (firstCountry == null)
            {
                return result;
            }

            result = firstCountry.Code;

            return result;
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