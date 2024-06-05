namespace TripPlanner.Server.Models
{
    public class ReviewCreate
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public double Rate { get; set; }
        public int RegionId { get; set; }
    }
}
