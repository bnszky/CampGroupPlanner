using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly HashSet<string> _blacklist = new HashSet<string>();
        private readonly object _lock = new object();

        public Task AddTokenToBlacklistAsync(string token)
        {
            lock (_lock)
            {
                _blacklist.Add(token);
            }
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklistedAsync(string token)
        {
            lock (_lock)
            {
                return Task.FromResult(_blacklist.Contains(token));
            }
        }
    }
}
