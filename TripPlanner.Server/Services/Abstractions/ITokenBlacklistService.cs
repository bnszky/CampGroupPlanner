namespace TripPlanner.Server.Services.Abstractions
{
    public interface ITokenBlacklistService
    {
        Task AddTokenToBlacklistAsync(string token);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
