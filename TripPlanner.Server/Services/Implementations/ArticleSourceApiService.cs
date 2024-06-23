using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleSourceApiService : IArticleSourceService
    {
        private readonly ILogger<ArticleSourceApiService> _logger;
        private readonly string _apiKey;
        public ArticleSourceApiService(ILogger<ArticleSourceApiService> logger, IConfiguration configuration) {
            _logger = logger;
            _apiKey = configuration["NewsApi:Key"];
        }

        private async Task<List<Article>> ConvertJsonToArticlesListAsync(JObject json)
        {
            List<Article> articles = new List<Article>();

            try
            {
                foreach (JToken article in json["articles"])
                {
                    articles.Add(new Article
                    {
                        Title = article["title"].ToString(),
                        Description = article["description"].ToString(),
                        SourceLink = article["url"].ToString(),
                        ImageURL = article["urlToImage"] == null ? null : article["urlToImage"].ToString(),
                        CreatedAt = (DateTime)article["publishedAt"],
                    }) ;
                }
                _logger.LogInformation("Successfully fetched {ArticlesCount} from NewsApi", articles.Count);
                return articles;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Couldn't extract articles from json");
                return [];
            }
        }
        private async Task<JObject> FetchJsonFromUrlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(responseBody);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't fetch json from url");
                    return JObject.Parse("");
                }
            }
        }
        public async Task<List<Article>> GetArticlesAsync()
        {
            string url = $"https://newsapi.org/v2/everything?q=places&sortBy=popularity&apiKey={_apiKey}";
            JObject json = await FetchJsonFromUrlAsync(url);
            var articles = await ConvertJsonToArticlesListAsync(json);
            return articles;
        }

        public async Task<List<Article>> GetArticlesByRegionNameAsync(string regionName)
        {
            string url = $"https://newsapi.org/v2/everything?q={regionName}&sortBy=popularity&apiKey={_apiKey}";
            JObject json = await FetchJsonFromUrlAsync(url);
            var articles = await ConvertJsonToArticlesListAsync(json);
            return articles;
        }
    }
}
