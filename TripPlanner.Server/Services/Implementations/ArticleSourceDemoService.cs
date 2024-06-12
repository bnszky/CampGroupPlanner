using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleSourceDemoService : IArticleSourceService
    {
        private readonly ILogger<ArticleSourceDemoService> _logger;
        public ArticleSourceDemoService(ILogger<ArticleSourceDemoService> logger) { _logger = logger; }
        public async Task<List<Article>> GetArticlesAsync(string regionName, string countryName, List<string> citiesNames)
        {
            List<Article> articles = new List<Article>();
            articles.Add(new Article
            {
                Title = "Title",
                Description = "Description",
                SourceLink = "SourceLink",
                ImageURL = "123"
            });

            _logger.LogInformation("Added example article");

            return articles;
        }
    }
}
