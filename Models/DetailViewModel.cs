using System.Text.RegularExpressions;

namespace SkyTunesCsharp.Models
{

    public class DetailViewModel
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? ListKey { get; set; }
        public List<TrackDto> Tracks { get; set; } = new List<TrackDto>();
        public int TrackCount => Tracks.Count;
        public int CurrentPage { get; set; } = 1;
        public int TotalCount { get; set; } = 0;

        public ArtistBannerViewModel? BannerModel { get; set; }

        public List<PlaylistItem>? PlayListItems { get; set; } = new List<PlaylistItem>();


        public string SafeKey => GetSafeListKey(Title);


        private string GetSafeListKey(string name)
        {
            return Regex.Replace(name, @"[\s&-]", "").ToLower();
        }


    }
}   