namespace CampGroupPlanner.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
        public int? Rate { get; set; }
        public string? Comment { get; set; }
    }
}
