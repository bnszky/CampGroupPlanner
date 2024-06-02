using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class ArticleCreate
    {

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required(ErrorMessage = "Source link is required")]
        public string SourceLink { get; set; }
        public string? RegionName { get; set; }
        [Range(0, 100, ErrorMessage = "PositioningRate must be between 0 and 100.")]
        public int? PositioningRate { get; set; }
        public bool IsVisible { get; set; }
    }
}
