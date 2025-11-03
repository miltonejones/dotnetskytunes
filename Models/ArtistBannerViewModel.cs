
namespace SkyTunesCsharp.Models
{
    public class ArtistBannerViewModel
    {
        public int ArtistId { get; set; }

        public string ImageUrl { get; set; }
        public string FallbackImage { get; set; } = "/images/fallback.jpg"; // Set your default fallback
        public string LabelName { get; set; }
        public string Caption { get; set; }
        public string TitleName { get; set; }
        public int TrackCount { get; set; }
        public bool Clickable { get; set; } = false;
    }
}   