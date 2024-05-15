using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        [Required(ErrorMessage = "Source link is required")]
        public string SourceLink { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
