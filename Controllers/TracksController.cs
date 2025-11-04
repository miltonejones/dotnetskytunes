using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SkyTunesCsharp.Models;
using SkyTunesCsharp.Services;

namespace SkyTunesCsharp.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITuneService _tuneService;

        public TracksController(ITuneService tuneService)
        {
            _tuneService = tuneService;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the track from your API 
            var trackItem = await _tuneService.GetTrackById(id);
            
            if (trackItem == null)
            {
                // Handle case where track isn't found
                return NotFound();
            }
            
            return PartialView("_TrackEdit", trackItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AppleLookup([FromBody] TrackLookupModel model)
        {
            try
            {
                    return Json(new { success = false, message = "Please enter a title to search" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lookup failed: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateTrack([FromBody] UpdateTrackModel model)
        {
            try
            {
               
                return Json(new { success = true, message = "Texting"});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}