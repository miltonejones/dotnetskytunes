using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class AudioPlayerViewModel
    {
        public TrackItem? CurrentTrack { get; set; }
        public List<TrackItem> TrackList { get; set; } = new List<TrackItem>();
    }
}