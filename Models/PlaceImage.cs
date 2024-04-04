using System.ComponentModel.DataAnnotations.Schema;

namespace CampGroupPlanner.Models
{
    public class PlaceImage
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }
    }
}
