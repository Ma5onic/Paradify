using log4net;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.Linq;
using web.Models;

namespace web.Services.Implementations
{
    class ParadifyService : IParadifyService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ParadifyService));

        public SearchItem SearchResult(string query, Token token, int limit = 10, int offset = 0, string market = "")
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            try
            {
                SearchItem searchItems = api.SearchItems(query, SpotifyAPI.Web.Enums.SearchType.Track, limit);
                return searchItems;
            }
            catch (System.Exception ex)
            {
                log.Error(
                   string.Format("CreatePlaylist()")
                   , ex);
            }
            return null;
        }

        public FullPlaylist CreatePlaylist(string id, string playlistName, Token token)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            try
            {
                return api.CreatePlaylist(id, playlistName);
            }
            catch (System.Exception ex)
            {
                log.Error(
                    string.Format("CreatePlaylist()")
                    , ex);
            }
            return null;
        }

        public Paging<SavedTrack> GetSavedTracks(CustomToken token, int limit = 10, int offset = 0, string market = "")
        {
            if (token.tokenCredentialType == CustomToken.TokenCredentialType.Client)
            {
                return null;
            }

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };
            try
            {
                Paging<SavedTrack> savedTracks = api.GetSavedTracks(limit, offset, market);
                return savedTracks;
            }
            catch (System.Exception ex)
            {
                log.Error(
                    string.Format("GetUsersRecentlyPlayedTracks()")
                    , ex);
            }
            return null;
        }

        public CursorPaging<PlayHistory> GetUsersRecentlyPlayedTracks(CustomToken token, int limit = 20)
        {
            if (token.tokenCredentialType == CustomToken.TokenCredentialType.Client)
            {
                return null;
            }

            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            try
            {
                return api.GetUsersRecentlyPlayedTracks(limit);
            }
            catch (System.Exception ex)
            {
                log.Error(
                    string.Format("GetUsersRecentlyPlayedTracks()")
                    , ex);
            }
            return null;
        }

        public CustomSimpleTrack GetNewReleasedTracks(CustomToken token, string countryCode)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            CustomSimpleTrack result = new CustomSimpleTrack();

            try
            {
                NewAlbumReleases newAlbumReleases = api.GetNewAlbumReleases(countryCode, limit: 5);

                if (newAlbumReleases.Albums != null && newAlbumReleases.Albums.Items != null)
                {
                    foreach (var album in newAlbumReleases.Albums.Items)
                    {
                        Paging<SimpleTrack> tracksOfTheAlbum = api.GetAlbumTracks(album.Id);

                        if (tracksOfTheAlbum.Items != null)
                        {
                            foreach (var track in tracksOfTheAlbum.Items)
                            {
                                result.TrackAlbumIds.Add(track.Id, album);

                                result.Paging.Items.Add(track);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                log.Error(
                    string.Format("GetNewReleasedTracks() => countrycode {0}", countryCode)
                    , ex);
            }

            return result;
        }

        public Recommendations GetRecommendations(CustomToken token, string trackId, string artistId)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, UseAuth = true, TokenType = token.TokenType };

            Recommendations recommendations = null;
            try
            {
                recommendations = api.GetRecommendations(
                         artistId.Split(',').ToList(),
                         null
                     , trackId.Split(',').ToList()
                     );
            }
            catch (System.Exception ex)
            {
                log.Error(
                   string.Format("GetRecommendations() => trackId {0}, artistId {1}", trackId, artistId)
                   , ex);
            }

            return recommendations;
        }
    }
}