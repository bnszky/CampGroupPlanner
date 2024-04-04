using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampGroupPlanner.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Nick { get; set; } = string.Empty;
        [NotMapped]
        public IFormFile? ProfileImage { get; set; }

        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
        public virtual List<Attraction> Attractions { get; set; }
        public virtual List<AttractionImage> AttractionImages { get; set; }
        public virtual List<Review> Reviews { get; set; }
       
    }
}
