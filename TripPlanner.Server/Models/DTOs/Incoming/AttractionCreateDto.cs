using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.DTOs.Incoming
{
    public class AttractionCreateDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string RegionName { get; set; }
    }
}
