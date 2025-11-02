using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class ArtistDetail
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("iArtistID")]
        public object? IArtistID { get; set; } = string.Empty; // Changed to string

        [JsonPropertyName("amgArtistID")]
        public object? AmgArtistID { get; set; } = string.Empty; // Changed to string

        [JsonPropertyName("imageLg")]
        public string ImageLg { get; set; } = string.Empty;

        [JsonPropertyName("TrackCount")]
        public int TrackCount { get; set; }
    }

    public class ArtistDetailResponse
    {
        [JsonPropertyName("row")]
        public List<ArtistDetail> Row { get; set; } = new List<ArtistDetail>();

        [JsonPropertyName("related")]
        public MusicResponse Related { get; set; } = new MusicResponse();
    }
}