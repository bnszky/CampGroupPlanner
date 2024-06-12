using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAttractionFetchService
    {
        Task<List<Attraction>> FetchAttractionsForGivenCities(List<City> cities, int maxNumberOfAttractions);
        Task<List<Attraction>> FetchAttractionsForGivenCity(City city);
    }
}
