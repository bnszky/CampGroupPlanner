using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.DTOs.Incoming
{
    public class RegionCreateDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage = "You must add some cities")]
        public List<string> Cities { get; set; }
    }
}
