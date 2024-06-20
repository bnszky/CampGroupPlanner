using System.ComponentModel.DataAnnotations;
using TripPlanner.Server.Models.Auth;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Models.DTOs.Outgoing
{
    public class ReviewGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public double Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorUsername { get; set; }
    }
}
