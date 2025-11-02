using SkyTunesCsharp.Models;

namespace SkyTunesCsharp.Services
{
    public interface IDashService
    {
        Task<List<DashResponse>> GetDashResponse();
        Task<AlbumDetailResponse> GetAlbumDetail(int id);
        Task<ArtistDetailResponse> GetArtistDetail(int id);
        Task<PlaylistDetailResponse> GetPlaylistDetail(string id);
        Task<PlaylistDashResponse> GetPlaylistGrid();
        Task<GenreDetailResponse> GetGenreDetail(string genre, int page);

        Task<ArtistGridResponse> GetArtistPage(int pageNum, string sortBy = "Name");
        Task<AlbumGridResponse> GetAlbumPage(int pageNum);
        Task<GenreGridResponse> GetGenrePage(int pageNum);
        Task<PlaylistGridResponse> GetPlaylistPage(int pageNum);
        Task<MusicResponse> GetLibraryPage(int pageNum);
        Task<T> GetSearchPage<T>(string searchType, string searchParam) where T : class, new(); 
        Task<List<string>> GetAllRelatedStringsFromPlaylists();
    }
}