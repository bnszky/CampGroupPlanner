using TripPlanner.Server.Services.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace TripPlanner.Server.Services.Implementations
{
    public class EmailService : IEmailService
    {
        public async Task<bool> IsEmailValid(string email)
        {
            // check if email exists and its possible to send email
            return true;
        }

        public async Task SendMessageByEmailAsync(string email, string msg, string link)
        {
            // send message by email
            return;
        }
    }
}

