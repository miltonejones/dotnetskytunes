namespace SkyTunesCsharp.Models
{

    public class TrackLookupModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DiscNumber { get; set; }
        public string TrackNumber { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string Genre { get; set; }
    }

    public class UpdateTrackModel
    {
        public ItunesTrack ItunesTrack { get; set; }
        public int TrackId { get; set; }
    }

    public class ItunesTrack



    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public string CollectionName { get; set; }
        public string ArtworkUrl100 { get; set; }
        public string PrimaryGenreName { get; set; }
        public int DiscNumber { get; set; }
        public int TrackNumber { get; set; }
        public long TrackTimeMillis { get; set; }
        public int TrackId { get; set; }
    }
}