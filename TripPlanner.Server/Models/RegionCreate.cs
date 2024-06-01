using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class RegionCreate
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "You must add some cities")]
        public List<string> Cities { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
