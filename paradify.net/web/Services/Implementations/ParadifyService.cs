using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;

namespace web.Services.Implementations
{
    class ParadifyService : IParadifyService
    {
        public SearchItem SearchResult(string query, Token token, int limit = 20, int offset = 0, string market = "")
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            SearchItem searchItems = api.SearchItems(query, SpotifyAPI.Web.Enums.SearchType.Track, limit);
            return searchItems;
        }

        public FullPlaylist CreatePlaylist(string id, string playlistName, Token token)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            
            return api.CreatePlaylist(id, playlistName);
        }
    }
}