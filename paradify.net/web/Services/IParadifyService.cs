using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IParadifyService
    {
        SearchItem SearchResult(string query, Token token, int limit = 20, int offset = 0, string market = "");
        FullPlaylist CreatePlaylist(string id, string playlistName, Token token);
        Paging<SavedTrack> GetSavedTracks(Token token, int limit = 20, int offset = 0, string market = "");
        CursorPaging<PlayHistory> GetUsersRecentlyPlayedTracks(Token token, int limit = 20);
    }
}