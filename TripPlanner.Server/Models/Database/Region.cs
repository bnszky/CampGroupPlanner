using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Server.Models.Database
{
    public class Region
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        [Required]
        public string Country { get; set; }
        [Required(ErrorMessage = "You must add some cities")]
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
