namespace SkyTunesCsharp.Models
{
    public class GenreDetail
    {
        public string ID { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int TrackCount { get; set; }
        public string AlbumImage { get; set; } = string.Empty;
    }

    public class GenreDetailResponse
    {
        public List<GenreDetail> Row { get; set; } = new List<GenreDetail>();
        public MusicResponse Related { get; set; } = new MusicResponse();
    }
}