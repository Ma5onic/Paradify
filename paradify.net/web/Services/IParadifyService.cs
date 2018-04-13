using SpotifyAPI.Web.Models;
using web.Models;

namespace web.Services
{
    public interface IParadifyService
    {
        SearchItem SearchResult(string query, Token token, int limit = 10, int offset = 0, string market = "");
        FullPlaylist CreatePlaylist(string id, string playlistName, Token token);
        Paging<SavedTrack> GetSavedTracks(CustomToken token, int limit = 10, int offset = 0, string market = "");
        CursorPaging<PlayHistory> GetUsersRecentlyPlayedTracks(CustomToken token, int limit = 20);
        CustomSimpleTrack GetNewReleasedTracks(CustomToken token, string countryCode);
        Recommendations GetRecommendations(CustomToken token, string trackId, string artistId);
    }
}