using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class AlbumDetail
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("artistFk")]
        public string ArtistFk { get; set; } = string.Empty;

        // [JsonPropertyName("collectionId")]
        // public int CollectionId { get; set; } = 0;

         [JsonPropertyName("collectionId")]
        public string CollectionId { get; set; }
        
        // If you need it as int elsewhere, add a computed property
        [JsonIgnore]
        public int CollectionIdInt => int.TryParse(CollectionId, out int result) ? result : 0;


        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("TrackCount")]
        public int TrackCount { get; set; }
    }

    public class AlbumDetailResponse
    {
        [JsonPropertyName("row")]
        public List<AlbumDetail> Row { get; set; } = new List<AlbumDetail>();

        [JsonPropertyName("related")]
        public MusicResponse Related { get; set; } = new MusicResponse();
    }
}