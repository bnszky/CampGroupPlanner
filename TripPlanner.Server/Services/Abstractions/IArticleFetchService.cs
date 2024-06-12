using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleFetchService
    {
        Task<List<Article>> FetchArticlesByRegionDataAsync(string regionName, string countryName, List<string> citiesNames);
    }
}
