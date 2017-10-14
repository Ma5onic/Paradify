using System;
using System.Web.Http;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using web.Models;
using web.Services;

namespace web.Controllers
{
    public class PlaylistController : ApiController
    {
        private readonly ITokenCookieService _tokenService;

        public PlaylistController(ITokenCookieService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        public ErrorResponse Post(PlaylistModel model)
        {

            Token token = GetToken();
            PrivateProfile profile = GetMe(token);

            if (profile.Id == null && token.RefreshToken != null)
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = oldRefreshToken;
                _tokenService.SetToken(token);
                profile = GetMe(token);
            }

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };
            ErrorResponse errorResponse = api.AddPlaylistTrack(profile.Id, model.playlistId, model.trackId);

            return errorResponse;

        }

        [HttpPost]
        [Route("api/createplaylist")]
        public FullPlaylist CreateAPlaylist(CreatePlaylistModel model)
        {

            Token token = GetToken();
            PrivateProfile profile = GetMe(token);

            if (profile.Id == null && token.RefreshToken != null)
            {
                string oldRefreshToken = token.RefreshToken;
                token = RefreshToken(token.RefreshToken, Constants.ClientSecret);
                token.RefreshToken = oldRefreshToken;
                _tokenService.SetToken(token);
                profile = GetMe(token);
            }

            SpotifyAPI.Web.SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };
            FullPlaylist fullPlaylist = api.CreatePlaylist(profile.Id, model.Name);
            if (fullPlaylist.HasError())
            {
                throw new Exception("Playlist can not be created");
            }

            return fullPlaylist;

        }

        private Token RefreshToken(string refreshToken, string clientSecret)
        {
            AutorizationCodeAuth auth = new AutorizationCodeAuth() { ClientId = Constants.ClientId, State = Constants.StateKey };

            return auth.RefreshToken(refreshToken, clientSecret);
        }

        private Token GetToken()
        {
            return _tokenService.Get();
        }

        private PrivateProfile GetMe(Token token)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };

            PrivateProfile profile = api.GetPrivateProfile();
            return profile;
        }
    }
}
