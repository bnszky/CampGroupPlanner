using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.DTOs.Incoming
{
    public class ReviewCreateDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Review text cannot exceed 500 characters")]
        public string? Text { get; set; }
        [Range(0, 5, ErrorMessage = "Rate should be between 0-5")]
        public double Rate { get; set; }
        public string RegionName { get; set; }
    }
}
