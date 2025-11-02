using Microsoft.AspNetCore.Mvc;
using SkyTunesCsharp.Services;
using SkyTunesCsharp.Models;

namespace SkyTunesCsharp.Controllers
{
    public class DetailController : Controller
    {
        private readonly IDashService _dashService;
        private readonly ILogger<DetailController> _logger;

        public DetailController(IDashService dashService, ILogger<DetailController> logger)
        {
            _dashService = dashService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string param = "")  
        {
                var favoriteFileKeys = await _dashService.GetAllRelatedStringsFromPlaylists(); 
                var musicResponse = await _dashService.GetSearchPage<MusicResponse>("music", param);
                var artistResponse = await _dashService.GetSearchPage<ArtistGridResponse>("artist", param);
                var albumResponse = await _dashService.GetSearchPage<AlbumGridResponse>("album", param);
 

                var viewModel = new SearchViewModel { 
                    Tracks = ConvertToTrackDtos(musicResponse.Records, favoriteFileKeys),
                    Albums = albumResponse.Records,
                    Artists =  artistResponse.Records
                };
                return View("Search", viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Library(int page = 1)  
        {
                var listItems = await _dashService.GetPlaylistGrid();
                var favoriteFileKeys = await _dashService.GetAllRelatedStringsFromPlaylists();
                var viewModel = new DetailViewModel { Type = "music", CurrentPage = page ,
                    PlayListItems = listItems.Records};
                var musicDetail = await _dashService.GetLibraryPage(page);

                viewModel.Title = "Your Library";
                viewModel.ImageUrl = musicDetail.Records[0].AlbumImage;
                viewModel.Subtitle = $"{musicDetail.Count} tracks";
                viewModel.Tracks = ConvertToTrackDtos(musicDetail.Records, favoriteFileKeys);

                return View("Index", viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Index(string type, string id, int page = 1)  
        {
            try
            {
                _logger.LogInformation($"Detail page requested - Type: {type}, ID: {id}, Page: {page}");

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(id))
                {
                    return BadRequest("Invalid parameters");
                }

                // Validate ID based on type
                if (type.ToLower() != "playlist" && type.ToLower() != "genre")
                {
                    if (!int.TryParse(id, out int numericId) || numericId <= 0)
                    {
                        return BadRequest("Invalid ID format");
                    }
                }

                var listItems = await _dashService.GetPlaylistGrid();
                var viewModel = new DetailViewModel
                {
                    Type = type.ToLower(),
                    PlayListItems = listItems.Records
                };
                var favoriteFileKeys = await _dashService.GetAllRelatedStringsFromPlaylists();


                switch (type.ToLower())
                {
                    case "artist":
                        if (int.TryParse(id, out int artistId))
                        {
                            var artistDetail = await _dashService.GetArtistDetail(artistId);
                            if (artistDetail?.Row?.FirstOrDefault() != null)
                            {
                                var artist = artistDetail.Row.First();
                                viewModel.Title = artist.Name ?? "Unknown Artist";
                                viewModel.ImageUrl = artist.ImageLg ?? artist.Thumbnail;
                                viewModel.Subtitle = $"{artist.TrackCount} tracks";
                                viewModel.Tracks = ConvertToTrackDtos(artistDetail.Related?.Records, favoriteFileKeys);
                            }
                        }
                        break;

                    case "album":
                        if (int.TryParse(id, out int albumId))
                        {
                            var albumDetail = await _dashService.GetAlbumDetail(albumId);
                            if (albumDetail?.Row?.FirstOrDefault() != null)
                            {
                                var album = albumDetail.Row.First();
                                viewModel.Title = album.Name ?? "Unknown Album";
                                viewModel.ImageUrl = album.Thumbnail;
                                viewModel.Subtitle = $"By {album.ArtistName} • {album.TrackCount} tracks";
                                viewModel.Tracks = ConvertToTrackDtos(albumDetail.Related?.Records, favoriteFileKeys);
                            }
                        }
                        break;

                    case "playlist":
                        // Playlist ID can be string or numeric
                        var playlistDetail = await _dashService.GetPlaylistDetail(id);
                        if (playlistDetail?.Row?.FirstOrDefault() != null)
                        {
                            var playlist = playlistDetail.Row.First();
                            viewModel.Title = playlist.Title ?? "Unknown Playlist";
                            viewModel.ImageUrl = playlist.Image;
                            viewModel.Subtitle = $"{playlist.Related.Count} tracks";
                            viewModel.Tracks = ConvertToTrackDtos(playlistDetail.Related?.Records, favoriteFileKeys);
                            viewModel.ListKey = playlist.ListKey;
                        }
                        break;

                    case "genre":
                        // Genre uses string ID and page parameter
                        var genreDetail = await _dashService.GetGenreDetail(id, page);
                        if (genreDetail?.Row?.FirstOrDefault() != null)
                        {
                            var genre = genreDetail.Row.First();
                            viewModel.Title = genre.Genre ?? "Unknown Genre";
                            viewModel.ImageUrl = genre.AlbumImage;
                            viewModel.Subtitle = $"{genre.TrackCount} tracks • Page {page}";
                            viewModel.Tracks = ConvertToTrackDtos(genreDetail.Related?.Records, favoriteFileKeys);
                            viewModel.CurrentPage = page;
                            viewModel.TotalCount = genre.TrackCount;
                        }
                        break;

                    default:
                        return NotFound($"Unknown type: {type}");
                }

                if (string.IsNullOrEmpty(viewModel.Title))
                {
                    return NotFound($"{type} with ID {id} not found");
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Detail Index: {ex.Message}");
                return StatusCode(500, "An error occurred while loading the detail page");
            }
        }

        private List<TrackDto> ConvertToTrackDtos(List<TrackItem>? trackItems, List<string>? favoriteItems = null)
        {
            if (trackItems == null) return new List<TrackDto>();

            return trackItems.Select(track => new TrackDto
            {
                Id = track.SafeID,
                Title = track.Title ?? "Unknown Track",
                FileKey = track.FileKey ?? string.Empty,
                AlbumImage = track.AlbumImage ?? string.Empty,
                AlbumName = track.AlbumName ?? string.Empty,
                ArtistName = track.ArtistName ?? string.Empty,
                Genre = track.Genre ?? string.Empty,
                TrackTime = track.SafeTrackTime,
                AlbumFk = track.AlbumFk ?? 0,
                ArtistFk = track.ArtistFk ?? 0,
                TrackNumber = track.SafeTrackNumber,
                Favorite = favoriteItems?.Contains(track.FileKey ?? string.Empty) == true
            }).ToList();
        }
    }
}