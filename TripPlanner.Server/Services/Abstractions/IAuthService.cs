﻿using Microsoft.AspNetCore.Identity;
using TripPlanner.Server.Models.Auth;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user, IList<string> userRoles);
        Task AddTokenToBlacklistAsync(string token);
    }
}
