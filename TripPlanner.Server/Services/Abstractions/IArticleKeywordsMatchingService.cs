using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleKeywordsMatchingService
    {
        Task RateArticles(List<Article> articles);
        Task AssignArticlesByRegionNames(List<Article> articles, List<string> regionNames, List<string> countries);
    }
}
