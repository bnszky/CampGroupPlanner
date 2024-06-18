using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ResponseService : IResponseService
    {
        public IActionResult Get(string message, int statusCode, IDictionary<string, string[]> errors = null)
        {
            var errorResponse = new ErrorResponse
            {
                Title = message,
                Status = statusCode,
                Errors = errors
            };

            return new JsonResult(errorResponse) { StatusCode = statusCode };
        }
    }
}
