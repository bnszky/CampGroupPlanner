using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleRatingService
    {
        Task<bool> IsArticleValidAsync(Article article);
        Task<int?> RateArticleAsync(Article article, string regionName, string countryName, List<string> cityNames);
    }
}
