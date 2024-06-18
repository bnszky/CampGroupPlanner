using System.Text.Json.Serialization;

namespace TripPlanner.Server.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public double Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RegionId { get; set; }
        [JsonIgnore]
        public Region Region { get; set; }
        public string RegionName { get; set; }
        [JsonIgnore]
        public User Author { get; set; }
        public string AuthorId { get; set; }
        public string AuthorUsername { get; set; }

        public override string ToString()
        {
            return AuthorUsername + " - " + Title + " - Stars: " + Rate;
        }

    }
}
