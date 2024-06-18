namespace TripPlanner.Server.Models
{
    public class RegionMini
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
