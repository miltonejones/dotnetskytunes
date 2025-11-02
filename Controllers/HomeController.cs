using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SkyTunesCsharp.Models;
using SkyTunesCsharp.Services;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SkyTunesCsharp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDashService _dashService;
    private readonly ICompositeViewEngine _viewEngine;  
    private readonly string _longString;


    public HomeController(
        IDashService dashService, 
        ICompositeViewEngine viewEngine, 
        ILogger<HomeController> logger,
        IConfiguration configuration)  // Keep IConfiguration
    {
        _dashService = dashService;
        _viewEngine = viewEngine;
        _logger = logger;
        
        // Safe way to get the configuration value
        _longString = configuration.GetValue<string>("AppStrings:Fallback") ?? string.Empty; 
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            // _logger.LogInformation("HomeController Index method called");
            
            var dashItems = await _dashService.GetDashResponse(); 

            var carouselItems = dashItems?
                .Where(item => item.Type?.ToLower() == "artist" && item.ImageLg?.Length > 0)
                .OrderBy(x => Guid.NewGuid()) // Randomize
                .Take(12)
                .ToList() ?? new List<DashResponse>();
 


            var artistItems = dashItems?
                .Where(item => item.Type?.ToLower() == "artist")
                .OrderBy(x => Guid.NewGuid()) // Randomize
                .Take(12)
                .ToList() ?? new List<DashResponse>();

            var albumItems = dashItems?
                .Where(item => item.Type?.ToLower() == "album")
                .OrderBy(x => Guid.NewGuid()) // Randomize
                .Take(12)
                .ToList() ?? new List<DashResponse>();

            var playlistRes = await _dashService.GetPlaylistGrid();

            
            var listItems = playlistRes.Records?  
                .OrderBy(x => Guid.NewGuid()) // Randomize
                .Take(8)
                .ToList() ?? new List<PlaylistItem>();
 
 
            var carouselModel = new ArtistCarouselViewModel
            {
                Artists = carouselItems,
                FallbackImage = _longString
            };

            var model = new DashViewModel
            {
                ArtistItems = artistItems,
                AlbumItems = albumItems,
                PlaylistItems = listItems,
                CarouselItems = carouselModel
            };
            
            return View("Index", model);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error in Index method: {ex.Message}");
            throw;
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
