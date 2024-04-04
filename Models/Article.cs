namespace CampGroupPlanner.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
        public string? Content { get; set; }
        public Localization? Localization { get; set; }
        public int? LocalizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public virtual List<PlaceImage>? PlaceImages { get; set; }
    }
}
