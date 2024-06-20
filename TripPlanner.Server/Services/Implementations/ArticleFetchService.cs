using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleFetchService : IArticleFetchService
    {
        private readonly IEnumerable<IArticleSourceService> _articleSourceServices;
        private readonly IArticleRatingService _articleRatingService;
        private readonly IImageService _imageService;
        private readonly TripDbContext _context;
        private readonly ILogger<ArticleFetchService> _logger;

        public ArticleFetchService(IEnumerable<IArticleSourceService> articleSourceServices, IArticleRatingService articleRatingService, IImageService imageService, TripDbContext context, ILogger<ArticleFetchService> logger)
        {
            _articleSourceServices = articleSourceServices;
            _articleRatingService = articleRatingService;
            _context = context;
            _logger = logger;
            _imageService = imageService;
        }

        private async Task AddArticlesToDatabase(List<Article> articles)
        {
            try
            {
                int cnt = 0;
                foreach(var article in articles)
                {
                    if(!_context.Articles.Where(a => a.Title.ToLower().Equals(article.Title.ToLower())).Any())
                    {
                        _context.Add(article);
                        _logger.LogInformation("Successfully added {ArticleTitle} article to the database", article.Title);
                        cnt++;
                    }
                    else
                    {
                        _logger.LogWarning("Couldn't add because {ArticleTitle} article exists in the database", article.Title);
                    }
                }
                await _context.SaveChangesAsync();
                _logger.LogInformation("Successfully added {cnt} articles to the database", cnt);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Couldn't add articles to the database");
            }
        }

        public async Task<List<Article>> FetchArticles()
        {
            _logger.LogInformation("Starting to fetch articles");
            var articles = await FetchArticlesFromSources(null, null, null);
            _logger.LogInformation("Finished fetching articles. Total articles fetched: {ArticleCount}", articles.Count);
            await AddArticlesToDatabase(articles);
            return articles;
        }

        public async Task<List<Article>> FetchArticlesByRegionNameAsync(string regionName)
        {
            _logger.LogInformation("Starting to fetch articles for Region: {RegionName}", regionName);

            Region region = await _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).FirstOrDefaultAsync();
            if (region == null)
            {
                _logger.LogError("Couldn't find region with name {RegionName}", regionName);
                return new List<Article>();
            }

            regionName = region.Name.ToLower();
            List<string> citiesNames = region.Cities.Select(c => c.Name.ToLower()).ToList();
            string countryName = region.Country.ToLower();

            _logger.LogInformation("Successfully fetched information about region: {RegionName}, Country: {CountryName}, Cities: {CitiesNames}",
                regionName, countryName, string.Join(", ", citiesNames));

            var articles = await FetchArticlesFromSources(regionName, countryName, citiesNames);
            foreach (var article in articles)
            {
                article.RegionId = region.Id;
                article.RegionName = region.Name;
                article.Region = region;
            }

            _logger.LogInformation("Finished fetching articles. Total articles fetched: {ArticleCount}", articles.Count);
            await AddArticlesToDatabase(articles);
            return articles;
        }

        private async Task<List<Article>> FetchArticlesFromSources(string regionName, string countryName, List<string> citiesNames)
        {
            var allArticles = new ConcurrentBag<Article>();

            try
            {
                var fetchTasks = _articleSourceServices.Select(service => Task.Run(async () =>
                {
                    var articles = await service.GetArticlesAsync();
                    foreach (var article in articles)
                    {
                        int? rate = null;
                        if (await _articleRatingService.IsArticleValidAsync(article))
                        {
                            if (regionName == null) rate = await _articleRatingService.RateArticleAsync(article, null, null, null);
                            else rate = await _articleRatingService.RateArticleAsync(article, regionName, countryName, citiesNames);
                        }

                        if (rate != null)
                        {
                            //string imageUrl = await TryUploadImage(article.ImageURL);
                            //article.ImageURL = imageUrl;
                            article.PositioningRate = (int)rate;
                            article.IsVisible = true;
                            if(article.Title.Length > 100) article.Title = article.Title[..100];
                            if (article.Description != null && article.Description.Length > 500) { article.Description = article.Description[..500]; }
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

            return allArticles.ToList();
        }

        private async Task<string> TryUploadImage(string imageUrl)
        {
            if (imageUrl == null) return null;

            try
            {
                return await _imageService.UploadImage(imageUrl);
            }
            catch
            {
                _logger.LogWarning("Couldn't upload image: {ArticleImageUrl}", imageUrl);
                return null;
            }
        }
    }
}
