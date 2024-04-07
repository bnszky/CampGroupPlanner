namespace CampGroupPlanner.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
        public List<Localization>? Localizations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string? ImageLink { get; set; } 
        public string SourceLink { get; set; }
    }
}
