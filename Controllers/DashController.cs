using Microsoft.AspNetCore.Mvc;
using SkyTunesCsharp.Services;
using SkyTunesCsharp.Models;

namespace SkyTunesCsharp.Controllers
{
    public class DashController : Controller
    {
        private readonly IDashService _dashService;

        public DashController(IDashService dashService)
        {
            _dashService = dashService;
        }

        // Single grid action that handles all types
        public async Task<IActionResult> Grid(string type, int page = 1)
        {
            return type?.ToLower() switch
            {
                "artists" => await Artists(page),
                "albums" => await Albums(page),
                "genres" => await Genres(page),
                "playlists" => await Playlists(page),
                _ => NotFound($"Grid type '{type}' not found")
            };
        }

        public async Task<IActionResult> Index(string type, int page = 1)
        {
            var artistGrid = await _dashService.GetArtistPage(page);
            var model = new PageDef {
                PageType = type,
                PageNum = page
            };

            return View("Index", model);
        }

        private async Task<IActionResult> Artists(int page = 1)
        {
            var artistGrid = await _dashService.GetArtistPage(page);
            return View("Artists", artistGrid);
        }

        private async Task<IActionResult> Albums(int page = 1)
        {
            var albumGrid = await _dashService.GetAlbumPage(page);
            return View("Albums", albumGrid);
        }

        private async Task<IActionResult> Genres(int page = 1)
        {
            var genreGrid = await _dashService.GetGenrePage(page);
            return View("Genres", genreGrid);
        }

        private async Task<IActionResult> Playlists(int page = 1)
        {
            var playlistGrid = await _dashService.GetPlaylistPage(page);
            return View("Playlists", playlistGrid);
        }
    }
}