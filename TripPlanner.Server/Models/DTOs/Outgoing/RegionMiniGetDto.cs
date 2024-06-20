namespace TripPlanner.Server.Models.DTOs.Outgoing
{
    public class RegionMiniGetDto
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
