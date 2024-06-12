using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleSourceService
    {
        Task<List<Article>> GetArticlesAsync(string regionName, string countryName, List<string> citiesNames);
    }
}
