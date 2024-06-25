using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleKeywordsMatchingSevice : IArticleKeywordsMatchingService
    {
        private readonly List<string> _keywords;
        private readonly string[] filePaths = ["\\Keywords\\places_keywords.in", "\\Keywords\\travel_keywords.in"];
        private readonly ILogger<ArticleKeywordsMatchingSevice> _logger;
        public ArticleKeywordsMatchingSevice(ILogger<ArticleKeywordsMatchingSevice> logger, IConfiguration configuration) {
            _logger = logger;
            _keywords = LoadKeywordsFromFile(configuration["ProjectPath"] + filePaths[0]);
            _keywords.AddRange(LoadKeywordsFromFile(configuration["ProjectPath"] + filePaths[1]));
            _logger.LogDebug("Loaded all keywords from files, Keywords Count: {Keywords}", _keywords.Count);
        }

        public ArticleKeywordsMatchingSevice(ILogger<ArticleKeywordsMatchingSevice> logger, List<string> keywords)
        {
            _logger = logger;
            _keywords = keywords;
        }
        public List<string> LoadKeywordsFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            return File.ReadAllLines(filePath).ToList();
        }
        private int CountOccurrences(string text, string keyword)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            {
                return 0;
            }

            int count = 0;
            int index = 0;

            while ((index = text.IndexOf(keyword, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index ++;
            }

            return count;
        }

        public int SetRateBasedOnOccurances(int count)
        {
            if (count >= 20) return 100;
            return (int)(((float)count / 20) * 100);
        }

        public string? AssignArticleByRegion(Article article, string region, string country)
        {
            if(CountOccurrences(article.Title.ToLower(), region.ToLower()) > 0 || CountOccurrences(article.Title.ToLower(), country.ToLower()) > 0)
            {
                return region;
            }
            return null;
        }

        public async Task AssignArticlesByRegionNames(List<Article> articles, List<string> regionNames, List<string> countries)
        {
            _logger.LogDebug("Started assigning articles with keywords matching algorithm");
            await Task.Run(() =>
            {
                Parallel.ForEach(articles, article =>
                {
                    for (int i = 0; i < regionNames.Count; i++) {
                        var regionName = AssignArticleByRegion(article, regionNames[i], countries[i]);
                        article.RegionName = regionName;
                        if (regionName != null)
                        {
                            _logger.LogDebug("Article {ArticleTitle} assigned to Region: {RegionName}", article.Title, regionName);
                            break;
                        }   
                    }
                });
            });
        }

        public async Task RateArticles(List<Article> articles)
        {
            _logger.LogDebug("Started rating articles with keywords matching algorithm");
            await Task.Run(() =>
            {
                Parallel.ForEach(articles, article =>
                {
                    article.PositioningRate = SetRateBasedOnOccurances(_keywords.Sum(keyword => CountOccurrences(article.Title.ToLower(), keyword)));
                    _logger.LogDebug("Article: {ArticleTitle} - Rated: {Rate}", article.Title, article.PositioningRate);
                });
            });
        }
    }
}
