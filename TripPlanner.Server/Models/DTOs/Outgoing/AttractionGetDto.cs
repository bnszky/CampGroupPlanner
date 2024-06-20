using System.ComponentModel.DataAnnotations;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Models.DTOs.Outgoing
{
    public class AttractionGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string RegionName { get; set; }
    }
}
