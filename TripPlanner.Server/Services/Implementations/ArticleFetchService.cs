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
        private readonly IArticleKeywordsMatchingService _articleKeywordsMatchingService;
        private readonly IImageService _imageService;
        private readonly TripDbContext _context;
        private readonly ILogger<ArticleFetchService> _logger;
        private readonly int MAX_NUMBER_ARTICLES_PER_REQUEST = 60;

        public ArticleFetchService(IEnumerable<IArticleSourceService> articleSourceServices, IArticleRatingService articleRatingService, IImageService imageService, TripDbContext context, ILogger<ArticleFetchService> logger, IArticleKeywordsMatchingService articleKeywordsMatchingService)
        {
            _articleSourceServices = articleSourceServices;
            _articleRatingService = articleRatingService;
            _context = context;
            _logger = logger;
            _imageService = imageService;
            _articleKeywordsMatchingService = articleKeywordsMatchingService;
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

        private List<Article> RemoveExistingArticlesAsync(List<Article> articles)
        {
            try
            {
                List<Article> verifiedArticles = new List<Article>();
                foreach (var article in articles)
                {
                    if (!_context.Articles.Where(a => a.Title.ToLower().Equals(article.Title.ToLower())).Any())
                    {
                        _context.Add(article);
                        _logger.LogInformation("Successfully verified {ArticleTitle} article in the database", article.Title);
                        verifiedArticles.Add(article);
                    }
                    else
                    {
                        _logger.LogWarning("Removed {ArticleTitle} because article exists in the database", article.Title);
                    }
                }

                _logger.LogInformation("Database Filtring: Removed {RemovedArticlesCount} articles, Not existing articles in db: {VerifiedArticlesCount}", articles.Count - verifiedArticles.Count, verifiedArticles.Count);
                return verifiedArticles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't check database");
                return [];
            }
        }

        private async Task<(List<string>, List<string>, List<string>)> GetAllRegionWithCountryNames()
        {
            try
            {
                List<string> regionWithCountries = _context.Regions.OrderBy(r => r.Id).Select(r => r.Name + " - " + r.Country).ToList();
                List<string> regions = _context.Regions.OrderBy(r => r.Id).Select(r => r.Name).ToList();
                List<string> countries = _context.Regions.OrderBy(r => r.Id).Select(r => r.Country).ToList();

                return (regions, countries, regionWithCountries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't fetch region names from database");
                return ([], [], []);
            }
        }

        private async Task<(List<string>, List<string>, List<string>)> GetRegionWithCountryName(string regionName)
        {
            try
            {
                List<string> regionWithCountries = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Name + " - " + r.Country).ToList();
                List<string> regions = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Name).ToList();
                List<string> countries = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Country).ToList();

                return (regions, countries, regionWithCountries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't fetch region names from database");
                return ([], [], []);
            }
        }

        private async Task<List<Article>> FilterOnlyWithGivenRegionName(List<Article> articles, string regionName)
        {
            List<Article> filteredArticles = [];

            foreach(var article in articles)
            {
                if (article.RegionName.ToLower().Equals(regionName.ToLower()))
                {
                    filteredArticles.Add(article);
                }
            }

            return filteredArticles;
        } 

        private void ChangeArticleFields(List<Article> articles)
        {
            foreach (var article in articles)
            {
                if(article.RegionName != null)
                {
                    article.Region = _context.Regions.Where(r => r.Name.ToLower().Equals(article.RegionName.ToLower())).FirstOrDefault();
                }
                article.IsVisible = true;
                article.EditedAt = DateTime.Now;
            }
        }

        private List<Article> TakeNRandomArticles(List<Article> articles, int N)
        {
            if (articles == null || articles.Count <= N) return articles;

            Random random = new Random();

            return articles.OrderBy(a => random.Next()).Take(N).ToList();
        }

        public async Task<List<Article>> FetchArticles()
        {
            _logger.LogInformation("Starting to fetch articles");
            var articles = await FetchArticlesFromSources(null);
            articles = RemoveExistingArticlesAsync(articles);
            var (regions, countries, regionWithCountriesNames) = await GetAllRegionWithCountryNames();

            // take max N articles, rate and assign to regions
            articles = TakeNRandomArticles(articles, MAX_NUMBER_ARTICLES_PER_REQUEST);

            bool isSuccess = await _articleRatingService.RateArticlesAsync(articles, regions, countries, regionWithCountriesNames);
            if (!isSuccess)
            {
                await _articleKeywordsMatchingService.RateArticles(articles);
                await _articleKeywordsMatchingService.AssignArticlesByRegionNames(articles, regions, countries);
            }

            // update articles field for isVisible and Region
            ChangeArticleFields(articles);
            _logger.LogInformation("Finished fetching articles. Total articles fetched: {ArticleCount}", articles.Count);

            await AddArticlesToDatabase(articles);
            return articles;
        }

        public async Task<List<Article>> FetchArticlesByRegionNameAsync(string regionName)
        {
            _logger.LogInformation("Starting to fetch articles for Region: {RegionName}", regionName);
            var articles = await FetchArticlesFromSources(regionName);
            articles = RemoveExistingArticlesAsync(articles);

            var (regions, countries, regionWithCountryName) = await GetRegionWithCountryName(regionName);

            // take max N articles, rate and assign to regions
            articles = TakeNRandomArticles(articles, MAX_NUMBER_ARTICLES_PER_REQUEST);

            bool isSuccess = await _articleRatingService.RateArticlesAsync(articles, regions, countries, regionWithCountryName);
            if (!isSuccess)
            {
                await _articleKeywordsMatchingService.RateArticles(articles);
                await _articleKeywordsMatchingService.AssignArticlesByRegionNames(articles, regions, countries);
            }

            articles = await FilterOnlyWithGivenRegionName(articles, regionName);

            // update articles field for isVisible and Region
            ChangeArticleFields(articles);
            _logger.LogInformation("Finished fetching articles. Total articles fetched: {ArticleCount}", articles.Count);

            await AddArticlesToDatabase(articles);
            return articles;
        }

        public async Task<List<Article>> TryAssignAndRateExistingArticles()
        {
            _logger.LogInformation("Starting rating and assigning articles from the database");
            List<Article> articles = _context.Articles.Where(a => a.PositioningRate == 0 || a.Region == null).ToList();
            articles = TakeNRandomArticles(articles, MAX_NUMBER_ARTICLES_PER_REQUEST);

            var (regions, countries, regionWithCountriesNames) = await GetAllRegionWithCountryNames();

            bool isSuccess = await _articleRatingService.RateArticlesAsync(articles, regions, countries, regionWithCountriesNames);
            if (!isSuccess)
            {
                await _articleKeywordsMatchingService.RateArticles(articles);
                await _articleKeywordsMatchingService.AssignArticlesByRegionNames(articles, regions, countries);
            }

            // update articles field for isVisible and Region
            ChangeArticleFields(articles);
            _logger.LogInformation("Finished editing articles. Total articles edited: {ArticleCount}", articles.Count);

            await _context.SaveChangesAsync();
            return articles;
        }

        private async Task<List<Article>> FetchArticlesFromSources(string regionName)
        {
            var allArticles = new ConcurrentBag<Article>();

            try
            {
                var fetchTasks = _articleSourceServices.Select(service => Task.Run(async () =>
                {
                    List<Article> articles = [];
                    if (regionName != null) articles = await service.GetArticlesByRegionNameAsync(regionName);
                    else articles = await service.GetArticlesAsync();
                    foreach (var article in articles)
                    {
                        if (await _articleRatingService.IsArticleValidAsync(article))
                        {
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
