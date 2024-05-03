namespace TripPlanner.Server.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public double Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }

    }
}
