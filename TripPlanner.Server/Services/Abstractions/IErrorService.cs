using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IErrorService
    {
        public ErrorResponse CreateError(string title, int code = 400);
        public void AddNewErrorMessageFor(ErrorResponse errorResponse, string errorFor, string message);
    }
}