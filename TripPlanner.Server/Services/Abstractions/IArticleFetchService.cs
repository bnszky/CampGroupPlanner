using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleFetchService
    {
        Task<List<Article>> FetchArticlesByRegionNameAsync(string regionName);
        Task<List<Article>> FetchArticles();
        Task<List<Article>> TryAssignAndRateExistingArticles();
    }
}
