using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TripPlanner.Server.Models.Database
{
    public class Attraction
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        [Required(ErrorMessage = "Longitude is required")]
        public double Longitude { get; set; }
        [Required(ErrorMessage = "Latitude is required")]
        public double Latitude { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public string RegionName { get; set; }
        public string? FRS_ID { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
