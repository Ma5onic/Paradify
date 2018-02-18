using System.Linq;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web;

namespace web.Services.Implementations
{
    public class PlaylistService : IPlaylistService
    {
        public PlaylistService()
        {

        }

        public Paging<SimplePlaylist> GetPlaylists(ITokenCookieService tokenCookieService, string profileId)
        {
            var token = tokenCookieService.Get();

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            Paging<SimplePlaylist> userPlaylists = api.GetUserPlaylists(profileId, 50);

            if (userPlaylists != null)
            {
                userPlaylists.Items = userPlaylists.Items.Where(x => x.Owner.Id == profileId).ToList();
            }
            
            return userPlaylists;
        }

        public Paging<SimplePlaylist> GetPlaylists(Token token, string profileId)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            Paging<SimplePlaylist> userPlaylists = api.GetUserPlaylists(profileId, 50);
            
            if (userPlaylists != null)
            {
                userPlaylists.Items = userPlaylists.Items.Where(x => x.Owner.Id == profileId).ToList();
            }

            return userPlaylists;
        }
    }
}