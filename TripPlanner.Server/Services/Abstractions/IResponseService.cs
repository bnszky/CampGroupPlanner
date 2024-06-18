using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Messages;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IResponseService
    {
        IActionResult Get(string message, int statusCode, IDictionary<string, string[]> errors = null);
    }
}
