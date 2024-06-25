using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleRatingService
    {
        Task<bool> IsArticleValidAsync(Article article);
        Task<bool> RateArticlesAsync(List<Article> articles, List<string> regions, List<string> countries, List<string> regionWithCountryNames);
    }
}
