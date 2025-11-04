
using SkyTunesCsharp.Models;
using System.Text.Json;


namespace SkyTunesCsharp.Services
{

    public interface ITuneService
    {
        // Task<object> GetAppleLookup(string searchTerm);
        // Task<object> GetAlbumorArtistId(string type, string name, string image);
        // Task<object> UpdateTrack(TrackItem track);
        // TrackItem ConvertFromItunes(ItunesTrack itunesTrack, int trackId);
        // Task<object> GetOrCreateAlbum(string albumName, string imageUrl);
        // Task<object> GetOrCreateArtist(string artistName, string imageUrl);
        Task<TrackItem> GetTrackById(int id); // Add this method
    }

    public class TuneService : ITuneService
    {
        private readonly string API_URL = "https://u8m0btl997.execute-api.us-east-1.amazonaws.com";
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public TuneService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<TrackItem> GetTrackById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{API_URL}/track/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var trackItem = JsonSerializer.Deserialize<TrackItem>(jsonString, _jsonOptions);
                    return trackItem;
                }
                return null;
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching track {id}: {ex.Message}");
                return null;
            }
        }

        // ... rest of your methods
    }
} 