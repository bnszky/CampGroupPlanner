using System.ComponentModel.DataAnnotations;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Models.DTOs.Outgoing
{
    public class ArticleGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? EditedAt { get; set; }
        public string SourceLink { get; set; }
        public string? RegionName { get; set; }
        public int PositioningRate { get; set; }
        public bool IsVisible { get; set; }
    }
}
