using System.Text.Json;
using SkyTunesCsharp.Models;

namespace SkyTunesCsharp.Services
{
    public class DashService : IDashService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DashService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public DashService(HttpClient httpClient, ILogger<DashService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://u8m0btl997.execute-api.us-east-1.amazonaws.com");

            // Configure JSON options for flexible deserialization
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip

            };
        }

        public async Task<List<DashResponse>> GetDashResponse()
        {
            try
            {
                var response = await _httpClient.GetAsync("/dash");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Dash response: {content}");

                var dashItems = JsonSerializer.Deserialize<List<DashResponse>>(content, _jsonOptions);

                return dashItems ?? new List<DashResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching dash items: {ex.Message}");
                throw;
            }
        }

        public async Task<AlbumDetailResponse> GetAlbumDetail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/discNumber,trackNumber/ASC/1/album/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Album detail response: {content}");

                var albumDetail = JsonSerializer.Deserialize<AlbumDetailResponse>(content, _jsonOptions);

                return albumDetail ?? new AlbumDetailResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching album detail for ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<ArtistDetailResponse> GetArtistDetail(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/albumFk,%20discNumber,%20trackNumber/ASC/1/artist/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Artist detail response: {content}");

                var artistDetail = JsonSerializer.Deserialize<ArtistDetailResponse>(content, _jsonOptions);

                return artistDetail ?? new ArtistDetailResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching artist detail for ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<PlaylistDetailResponse> GetPlaylistDetail(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/trackNumber/DESC/1/playlist/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Playlist detail response: {content}");

                var playlistDetail = JsonSerializer.Deserialize<PlaylistDetailResponse>(content, _jsonOptions);

                return playlistDetail ?? new PlaylistDetailResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching playlist detail for ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<PlaylistDashResponse> GetPlaylistGrid()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/Title/DESC/1/playlist");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Playlist grid response: {content}");

                var playlistDetail = JsonSerializer.Deserialize<PlaylistDashResponse>(content, _jsonOptions);

                return playlistDetail ?? new PlaylistDashResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching playlist grid: {ex.Message}");
                throw;
            }
        }

        public async Task<GenreDetailResponse> GetGenreDetail(string genre, int page)
        {
            try
            {
                var genreName = genre.Replace(" ", "%20");
                var response = await _httpClient.GetAsync($"/request/artistName/ASC/{page}/genre/{genreName}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Genre detail response: {content}");

                var genreDetail = JsonSerializer.Deserialize<GenreDetailResponse>(content, _jsonOptions);

                return genreDetail ?? new GenreDetailResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching genre detail for genre {genre}: {ex.Message}");
                throw;
            }
        }

        public async Task<ArtistGridResponse> GetArtistPage(int pageNum, string sortBy = "Name")
        {
            try
            {

                var response = await _httpClient.GetAsync($"/request/{sortBy}/ASC/{pageNum}/artist");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Artist grid response: {content}");

                var artistGrid = JsonSerializer.Deserialize<ArtistGridResponse>(content, _jsonOptions);

                return artistGrid ?? new ArtistGridResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching artist grid for page {pageNum}: {ex.Message}");
                throw;
            }
        }

        public async Task<AlbumGridResponse> GetAlbumPage(int pageNum)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/Name/ASC/{pageNum}/album");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Album grid response: {content}");

                var albumGrid = JsonSerializer.Deserialize<AlbumGridResponse>(content, _jsonOptions);

                return albumGrid ?? new AlbumGridResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching album grid for page {pageNum}: {ex.Message}");
                throw;
            }
        }

        public async Task<GenreGridResponse> GetGenrePage(int pageNum)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/Genre/ASC/{pageNum}/genre");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Genre grid response: {content}");

                var genreGrid = JsonSerializer.Deserialize<GenreGridResponse>(content, _jsonOptions);

                return genreGrid ?? new GenreGridResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching genre grid for page {pageNum}: {ex.Message}");
                throw;
            }
        }

        public async Task<PlaylistGridResponse> GetPlaylistPage(int pageNum)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/ID/DESC/{pageNum}/playlist");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Playlist grid response: {content}");

                var playlistGrid = JsonSerializer.Deserialize<PlaylistGridResponse>(content, _jsonOptions);

                return playlistGrid ?? new PlaylistGridResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching playlist grid for page {pageNum}: {ex.Message}");
                throw;
            }
        }

        public async Task<MusicResponse> GetLibraryPage(int pageNum)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/request/ID/DESC/{pageNum}/music");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Playlist grid response: {content}");

                var playlistGrid = JsonSerializer.Deserialize<MusicResponse>(content, _jsonOptions);

                return playlistGrid ?? new MusicResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching playlist grid for page {pageNum}: {ex.Message}");
                throw;
            }
        }


        public async Task<T> GetSearchPage<T>(string searchType, string searchParam) where T : class, new()
        {
            try
            {
                var response = await _httpClient.GetAsync($"search/1/{searchType}/{searchParam}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"{searchType} grid response: {content}");

                var gridResponse = JsonSerializer.Deserialize<T>(content, _jsonOptions);

                return gridResponse ?? new T();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching search for type {searchType} param {searchParam}: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetAllRelatedStringsFromPlaylists()
        {
            try
            {
                var playlistResponse = await GetPlaylistPage(1);

                var allRelated = new List<string>();
                foreach (var playlistItem in playlistResponse.Records)
                {
                    // Assuming PlaylistGridItem has a Related property that's a List<string>
                    if (playlistItem.Related != null && playlistItem.Related.Any())
                    {
                        allRelated.AddRange(playlistItem.Related);
                    }
                }

                _logger.LogInformation($"All related strings: {JsonSerializer.Serialize(allRelated)}");
                return allRelated;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching all related strings from playlists: {ex.Message}");
                throw;
            }
        }


 

    }
}