using Microsoft.AspNetCore.Identity;
using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user, IList<string> userRoles);
    }
}
