namespace TripPlanner.Server.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendMessageByEmailAsync(string email, string msg, string link);
        Task<bool> IsEmailValid(string email);
    }
}
