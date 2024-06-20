using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface ICityService
    {
        public Task<City> FetchInformationAboutCityFromName(string cityName);
    }
}
