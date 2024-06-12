using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleRatingService
    {
        Task<int> RateArticleAsync(Article article);
        Task<bool> IsArticleValidAsync(Article article);
    }
}
