using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class ArticleCreate
    {

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required(ErrorMessage = "Source link is required")]
        public string SourceLink { get; set; }
        public int? RegionId { get; set; }
        public Region? Region { get; set; }
    }
}
