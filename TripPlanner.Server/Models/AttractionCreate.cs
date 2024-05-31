using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class AttractionCreate
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "Latitude is required")]
        public double Latitude { get; set; }
        public string RegionName { get; set; }
    }
}
