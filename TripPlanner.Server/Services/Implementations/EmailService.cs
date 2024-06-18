using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task<bool> IsEmailValid(string email)
        {
            return true;
        }

        public async Task SendMessageByEmailAsync(string email, string msg, string link)
        {
            return;
        }
    }
}
