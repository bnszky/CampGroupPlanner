using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Country { get; set; }
        public ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public override string ToString()
        {
            return Name + " - " + Country;
        }
    }
}
