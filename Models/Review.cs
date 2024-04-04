namespace CampGroupPlanner.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public Attraction Attraction { get; set; }
        public int AttractionId { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public int? Rate { get; set; }
        public string? Comment { get; set; }
    }
}
