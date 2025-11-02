using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class DashResponse  
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("ID")]
        public int ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("imageLg")]
        public string ImageLg { get; set; } = string.Empty;
 
        [JsonPropertyName("Caption")]
        public string Caption { get; set; } = string.Empty;
 
        [JsonPropertyName("Thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;
    }
}