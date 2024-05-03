namespace TripPlanner.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Nick { get; set; }
        public string? ProfileImageUrl { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
