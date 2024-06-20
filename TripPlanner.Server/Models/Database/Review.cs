using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TripPlanner.Server.Models.Auth;

namespace TripPlanner.Server.Models.Database
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "Review text cannot exceed 500 characters")]
        public string? Text { get; set; }
        [Range(0, 5, ErrorMessage = "Rate should be between 0-5")]
        public double Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public User Author { get; set; }
        public string AuthorId { get; set; }

        public override string ToString()
        {
            return Title + " - Stars: " + Rate;
        }

    }
}
