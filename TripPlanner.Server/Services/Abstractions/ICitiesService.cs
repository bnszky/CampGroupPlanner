using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface ICitiesService
    {
        public Task<City> FetchInformationAboutCityFromName(string cityName);
    }
}
