using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAttractionFetchService
    {
        Task<List<Attraction>> FetchAttractionsForGivenCities(List<City> cities, int maxNumberOfAttractions);
        Task<List<Attraction>> FetchAttractionsForGivenCity(City city);
    }
}
