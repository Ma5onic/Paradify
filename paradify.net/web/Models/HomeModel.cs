using SpotifyAPI.Web.Models;

namespace web.Models
{
    public class HomeModel
    {
        public CursorPaging<PlayHistory> RecentlyPlayedTracks { get; internal set; }
        public Paging<SavedTrack> SavedTracks { get; internal set; }
        public CustomSimpleTrack NewReleasedTracks { get; internal set; }
        public Recommendations Recommendations { get; internal set; }
        public string CountryCode { get; internal set; }
    }
}