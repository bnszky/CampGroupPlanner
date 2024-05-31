using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ErrorService : IErrorService
    {
        public ErrorResponse CreateError(string title, int code = 400)
        {
            return new ErrorResponse
            {
                Title = title,
                Status = code,
                Errors = new Dictionary<string, string[]>()
            };
        }

        public void AddNewErrorMessageFor(ErrorResponse errorResponse, string errorFor, string message)
        {
            if (errorResponse.Errors.ContainsKey(errorFor))
            {
                var existingMessages = errorResponse.Errors[errorFor];
                var updatedMessages = existingMessages.Concat(new[] { message }).ToArray();
                errorResponse.Errors[errorFor] = updatedMessages;
            }
            else
            {
                errorResponse.Errors[errorFor] = new[] { message };
            }
        }
    }
}
