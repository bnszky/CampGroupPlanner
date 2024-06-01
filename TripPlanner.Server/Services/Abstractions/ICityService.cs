using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface ICityService
    {
        public Task<City> FetchInformationAboutCityFromName(string cityName);
    }
}
