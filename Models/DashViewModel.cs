namespace SkyTunesCsharp.Models;
using System.Collections.Generic;
using System.Linq;


public class DashViewModel
{
        public List<DashResponse> ArtistItems { get; set; } = new List<DashResponse>();
        public List<DashResponse> AlbumItems { get; set; } = new List<DashResponse>();
        public List<PlaylistItem> PlaylistItems { get; set; } = new List<PlaylistItem>();
        
        public ArtistCarouselViewModel CarouselItems { get; set; }
}
