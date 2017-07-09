﻿using SpotifyAPI.Web.Models;

namespace web.Controllers
{
    public class SearchResult
    {
        public SearchItem SearchItem { get; set; }
        public string query { get; set; }
        public string track { get; set; }
        public Paging<SimplePlaylist> Playlists { get; set; }
    }
}
