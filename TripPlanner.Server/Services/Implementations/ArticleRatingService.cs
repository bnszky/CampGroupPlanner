using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleRatingService : IArticleRatingService
    {
        public async Task<bool> IsArticleValidAsync(Article article)
        {
            return true;
        }

        public async Task<int> RateArticleAsync(Article article)
        {
            return 0;
        }
    }
}
