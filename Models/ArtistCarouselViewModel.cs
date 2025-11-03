namespace SkyTunesCsharp.Models
{
    public class ArtistCarouselViewModel
    {
 

        public List<DashResponse> Artists { get; set; } = new List<DashResponse>();
 

        public string FallbackImage { get; set; } = "/images/fallback.jpg";
    }
}

  