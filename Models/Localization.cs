namespace CampGroupPlanner.Models
{
    public class Localization
    {
        public Guid Id { get; set; }
        public string? PlaceName { get; set; }
        public string? Country { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }
    }
}
