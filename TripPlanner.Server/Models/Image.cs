namespace TripPlanner.Server.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}
