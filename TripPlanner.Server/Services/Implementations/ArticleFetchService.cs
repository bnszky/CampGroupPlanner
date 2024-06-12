using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleFetchService : IArticleFetchService
    {
        private readonly IEnumerable<IArticleSourceService> _articleSourceServices;
        private readonly IArticleRatingService _articleRatingService;
        private readonly TripDbContext _context;
        private readonly ILogger<ArticleFetchService> _logger;

        public ArticleFetchService(IEnumerable<IArticleSourceService> articleSourceServices, IArticleRatingService articleRatingService, TripDbContext context, ILogger<ArticleFetchService> logger)
        {
            _articleSourceServices = articleSourceServices;
            _articleRatingService = articleRatingService;
            _context = context;
            _logger = logger;
        }

        public async Task<List<Article>> FetchArticlesByRegionNameAsync(string regionName)
        {
            var allArticles = new ConcurrentBag<Article>();
            _logger.LogInformation("Starting to fetch articles for Region: {RegionName}", regionName);

            Region region = await _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).FirstOrDefaultAsync();
            if (region == null)
            {
                _logger.LogError("Couldn't find region with name {RegionName}", regionName);
            }

            regionName = region.Name.ToLower();
            List<string> citiesNames = region.Cities.Select(c => c.Name.ToLower()).ToList();
            string countryName = region.Country.ToLower();

            _logger.LogInformation("Successfully fetched information about region: {RegionName}, Country: {CountryName}, Cities: {CitiesNames}",
            regionName, countryName, string.Join(", ", citiesNames));

            try
            {

                // Fetch articles from all sources
                var fetchTasks = _articleSourceServices.Select(service => Task.Run(async () =>
                {
                    var articles = await service.GetArticlesAsync(regionName, countryName, citiesNames);
                    foreach (var article in articles)
                    {
                        // Rate and validate articles
                        if (await _articleRatingService.IsArticleValidAsync(article))
                        {
                            article.PositioningRate = await _articleRatingService.RateArticleAsync(article);
                            article.RegionId = region.Id;
                            article.RegionName = region.Name;
                            article.Region = region;
                            article.IsVisible = true;
                            allArticles.Add(article);
                            _logger.LogInformation("Article added: {ArticleTitle} ", article.Title);
                        }
                        else
                        {
                            _logger.LogWarning("Article not valid: {ArticleTitle} ", article.Title);
                        }
                    }
                }));

                await Task.WhenAll(fetchTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching articles");
            }

            _logger.LogInformation("Finished fetching articles. Total articles fetched: {ArticleCount}", allArticles.Count);

            return allArticles.ToList();
        }
    }

}
