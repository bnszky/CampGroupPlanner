namespace TripPlanner.Server.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string SourceLink { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
