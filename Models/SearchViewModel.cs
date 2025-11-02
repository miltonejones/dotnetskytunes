namespace SkyTunesCsharp.Models
{ 

    public class SearchViewModel
    { 
        public List<TrackDto> Tracks { get; set; } = new List<TrackDto>();
        public List<AlbumGridItem> Albums { get; set; } = new List<AlbumGridItem>(); 
        public List<ArtistGridItem> Artists { get; set; } = new List<ArtistGridItem>(); 
    }    
}