using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class Attraction
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
