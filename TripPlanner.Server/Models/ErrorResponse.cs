namespace TripPlanner.Server.Models
{
    public class ErrorResponse
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
