using System.Collections.Generic;
using SpotifyAPI.Web.Models;

namespace web.Models
{
    public class CustomSimpleTrack
    {
        public CustomSimpleTrack()
        {
            this.Paging = new Paging<SimpleTrack>();
            this.Paging.Items = new System.Collections.Generic.List<SimpleTrack>();
            TrackAlbumIds = new Dictionary<string, SimpleAlbum>();
        }
        public Paging<SimpleTrack> Paging { get; set; }
        public Dictionary<string, SimpleAlbum> TrackAlbumIds { get; internal set; }
    }
}