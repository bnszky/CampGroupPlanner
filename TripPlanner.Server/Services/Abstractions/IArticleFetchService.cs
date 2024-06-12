using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleFetchService
    {
        Task<List<Article>> FetchArticlesByRegionNameAsync(string regionName);
        Task<List<Article>> FetchArticles();
    }
}
