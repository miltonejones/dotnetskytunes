using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SkyTunesCsharp.Models
{  
    public class PageDef
    {
        public int PageNum { get; set; } = 0;
        public string PageType { get; set; } = string.Empty;
    }

    public class LibraryResponse
    {
        public int Count { get; set; }
        public List<PlaylistGridItem> Records { get; set; } = new List<PlaylistGridItem>();
    }

    public class PlaylistGridResponse
    {
        public int Count { get; set; }
        public List<PlaylistGridItem> Records { get; set; } = new List<PlaylistGridItem>();
    }

    public class PlaylistGridItem
    {
        public string ListKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int TrackCount { get; set; }
        public string[] Related { get; set; } = Array.Empty<string>();

        public string SafeKey => GetSafeListKey(Title);


        private string GetSafeListKey(string name)
        {
            return Regex.Replace(name, @"[\s&-]", "").ToLower();
        }
 

    }

    public class ArtistGridResponse
    {
        public int Count { get; set; }
        public List<ArtistGridItem> Records { get; set; } = new List<ArtistGridItem>();
    }

    public class ArtistGridItem
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public int TrackCount { get; set; }
    }

    public class AlbumGridResponse
    {
        public int Count { get; set; }
        public List<AlbumGridItem> Records { get; set; } = new List<AlbumGridItem>();
    }

    public class AlbumGridItem
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public int TrackCount { get; set; }
    }

    public class GenreGridResponse
    {
        public int Count { get; set; }
        public List<GenreGridItem> Records { get; set; } = new List<GenreGridItem>();
    }

    public class GenreGridItem
    {
        public string ID { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int TrackCount { get; set; }
        public string AlbumImage { get; set; } = string.Empty;
    }
}