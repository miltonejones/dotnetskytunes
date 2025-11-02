using System.Text.Json.Serialization;

namespace SkyTunesCsharp.Models
{
    public class TrackItem
    {
        [JsonPropertyName("ID")]
        public object? ID { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("FileKey")]
        public string FileKey { get; set; } = string.Empty;

        [JsonPropertyName("albumImage")]
        public string AlbumImage { get; set; } = string.Empty;

        [JsonPropertyName("trackId")]
        public object? TrackId { get; set; }

        [JsonPropertyName("Genre")]
        public string Genre { get; set; } = string.Empty;

        [JsonPropertyName("genreKey")]
        public object? GenreKey { get; set; }

        [JsonPropertyName("albumFk")]
        public int? AlbumFk { get; set; }

        [JsonPropertyName("albumArtistFk")]
        public object? AlbumArtistFk { get; set; }

        [JsonPropertyName("artistFk")]
        public int? ArtistFk { get; set; }

        [JsonPropertyName("discNumber")]
        public int? DiscNumber { get; set; }

        [JsonPropertyName("trackTime")]
        public object? TrackTime { get; set; }

        [JsonPropertyName("trackNumber")]
        public int? TrackNumber { get; set; }

        [JsonPropertyName("FileSize")]
        public object? FileSize { get; set; }

        [JsonPropertyName("explicit")]
        public object? Explicit { get; set; }

        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("albumName")]
        public string AlbumName { get; set; } = string.Empty;

        [JsonPropertyName("albumArtistName")]
        public string? AlbumArtistName { get; set; }

        [JsonPropertyName("favorite")]
        public object? Favorite { get; set; }

        // Helper properties for safe conversion
        public int SafeID => GetSafeInt(ID);
        public int SafeTrackNumber => GetSafeInt(TrackNumber);
        public int SafeDiscNumber => GetSafeInt(DiscNumber);
        public string SafeTrackTime => GetSafeTime(TrackTime);

        private int GetSafeInt(object? value)
        {
            if (value == null) return 0;
            if (value is int i) return i;
            if (int.TryParse(value?.ToString(), out int result)) return result;
            return 0;
        }

        private string GetSafeTime(object? value)
        {
            if (value == null) return "0:00";
            
            var stringValue = value.ToString();
            if (string.IsNullOrEmpty(stringValue)) return "0:00";
            
            // If it's already in time format, return as is
            if (stringValue.Contains(":")) return stringValue;
            
            // If it's milliseconds, convert to time format
            if (long.TryParse(stringValue, out long milliseconds))
            {
                var seconds = milliseconds / 1000;
                var minutes = seconds / 60;
                var remainingSeconds = seconds % 60;
                return $"{minutes}:{remainingSeconds:00}";
            }
            
            return stringValue;
        }
    }
}