using System.ComponentModel.DataAnnotations;

namespace CampGroupPlanner.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
        public List<Localization>? Localizations { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public string? ImageLink { get; set; } 
        public string SourceLink { get; set; }
    }
}
