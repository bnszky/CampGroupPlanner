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

        public ArticleFetchService(IEnumerable<IArticleSourceService> articleSourceServices, IArticleRatingService articleRatingService, TripDbContext context)
        {
            _articleSourceServices = articleSourceServices;
            _articleRatingService = articleRatingService;
            _context = context;
        }

        public async Task<List<Article>> FetchArticlesByRegionDataAsync(string regionName, string countryName, List<string> citiesNames)
        {
            var allArticles = new ConcurrentBag<Article>();
            Region region = await _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).FirstOrDefaultAsync();

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
                    }
                }
            }));

            await Task.WhenAll(fetchTasks);

            return allArticles.ToList();
        }
    }

}
