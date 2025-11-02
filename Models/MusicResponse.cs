using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{ 

    public class MusicResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("records")]
        public List<TrackItem> Records { get; set; } = new List<TrackItem>();
    }
}