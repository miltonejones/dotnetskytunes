using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class TrackDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("fileKey")]
        public string FileKey { get; set; } = string.Empty;

        [JsonPropertyName("albumImage")]
        public string AlbumImage { get; set; } = string.Empty;

        [JsonPropertyName("albumName")]
        public string AlbumName { get; set; } = string.Empty;

        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("trackTime")]
        public string TrackTime { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        [JsonPropertyName("trackNumber")]
        public int TrackNumber { get; set; }

        [JsonPropertyName("albumFk")]
        public int AlbumFk { get; set; }

        [JsonPropertyName("artistFk")]
        public int ArtistFk { get; set; } 
        
        public bool Favorite { get; set; }

    }
}