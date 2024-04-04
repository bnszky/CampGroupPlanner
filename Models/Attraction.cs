namespace CampGroupPlanner.Models

{
    public class Attraction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public AttractionType Type { get; set; }
        public DateTime? CreationDate { get; set; }
        public virtual List<AttractionImage>? Images { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public Localization Localization { get; set; }
        public virtual List<Review>? Reviews { get; set; }
        public int PositionRatio { get; set; }
    }
}
