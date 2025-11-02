using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    // public class PlaylistItem
    // {
    //     [JsonPropertyName("Title")]
    //     public string Title { get; set; } = string.Empty;

    //     [JsonPropertyName("listKey")]
    //     public string ListKey { get; set; } = string.Empty;

    //     [JsonPropertyName("related")]
    //     public List<string> Related { get; set; } = new List<string>();

    //     [JsonPropertyName("image")]
    //     public string Image { get; set; } = string.Empty;

    //     [JsonPropertyName("trackCount")]
    //     public string TrackCount { get; set; } = string.Empty;

    //     // // Add this property to handle the capital T version
    //     // [JsonPropertyName("TrackCount")]
    //     // public int TrackCountNumber { get; set; }

    //     [JsonPropertyName("selected")]
    //     public bool Selected { get; set; } 

    //     [JsonIgnore] // Ignore the track property completely
    //     public TrackItem Track { get; set; } = new TrackItem();
    // }


    public class PlaylistItem
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("listKey")]
        public string ListKey { get; set; } = string.Empty;

        [JsonPropertyName("related")]
        public List<string> Related { get; set; } = new List<string>();

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;
 
        [JsonIgnore] 
        public string TrackCount { get; set; } = string.Empty;

        [JsonPropertyName("selected")]
        public bool Selected { get; set; }

        [JsonIgnore] 
        public TrackItem Track { get; set; } = new TrackItem();

        [JsonPropertyName("playing")]
        public bool Playing { get; set; }

        // Capture all other properties including TrackCount (capital T)
        [JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; } = new Dictionary<string, object>();
    }

    public class PlaylistDetailResponse
    {
        [JsonPropertyName("row")]
        public List<PlaylistItem> Row { get; set; } = new List<PlaylistItem>();

        [JsonPropertyName("related")]
        public MusicResponse Related { get; set; } = new MusicResponse();
    }

    public class PlaylistDashResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; } 
        
        [JsonPropertyName("records")]
        public List<PlaylistItem> Records { get; set; } = new List<PlaylistItem>();
    }


}

 
 