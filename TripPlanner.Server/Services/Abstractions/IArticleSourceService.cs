using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleSourceService
    {
        Task<List<Article>> GetArticlesAsync();
        Task<List<Article>> GetArticlesByRegionNameAsync(string regionName);
    }
}
