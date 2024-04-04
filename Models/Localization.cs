namespace CampGroupPlanner.Models
{
    public class Localization
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? Address { get; set; }
        public Attraction Attraction { get; set; }
        public int AttractionId { get; set; }
    }
}
