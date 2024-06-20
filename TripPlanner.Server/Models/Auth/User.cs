using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Models.Auth
{
    public class User : IdentityUser
    {
        public ICollection<IdentityRole> Roles { get; set; } = new HashSet<IdentityRole>();
        [JsonIgnore]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
