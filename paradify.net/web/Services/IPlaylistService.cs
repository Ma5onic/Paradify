using System.Web.Mvc;
using SpotifyAPI.Web.Models;

namespace web.Services
{
    public interface IPlaylistService
    {
        Paging<SimplePlaylist> GetPlaylists(ITokenCookieService tokenCookieService, string profileId);
        Paging<SimplePlaylist> GetPlaylists(Token token, string profileId);
    }
}