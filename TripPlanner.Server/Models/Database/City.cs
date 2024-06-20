using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.Database
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public override string ToString()
        {
            return Name + " - " + Country;
        }
    }
}
