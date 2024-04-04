using System.ComponentModel.DataAnnotations.Schema;

namespace CampGroupPlanner.Models
{
    public class AttractionImage
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public DateTime PublicationDate { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public Attraction Attraction { get; set; }
        public int AttractionId { get; set; }
    }
}
