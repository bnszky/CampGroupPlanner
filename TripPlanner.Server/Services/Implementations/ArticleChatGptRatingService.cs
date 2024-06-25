using Newtonsoft.Json.Linq;
using System.Text;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;
using System.Text.Json;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleChatGptRatingService : IArticleRatingService
    {
        private readonly ILogger<ArticleChatGptRatingService> _logger;
        private readonly IConfiguration _configuration;
        public ArticleChatGptRatingService(ILogger<ArticleChatGptRatingService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> IsArticleValidAsync(Article article)
        {
            if (article.Description == null) return false;
            if (article.CreatedAt < DateTime.UtcNow.AddYears(-5)) return false;
            if (article.Title.Length > 100) article.Title = article.Title[..100];
            if (article.Description.Length > 500) article.Description = article.Description[..500];
            return true;
        }

        private string CreateChatGptQuery(List<Article> articles, List<string> regionWithCountryNames)
        {
            StringBuilder sb = new StringBuilder();

            const string chatGptQuery = """
                for Given articles with id and article title (id, article.title)
                and given regions (id, region)
                assign article to region, if article is not related to any region please set to -1
                rate the article according to its relevance to the topic of traveling and visiting places (rate should be 0-100)
                answer should be ONLY list in json format without any additional human message from you
                output: [{articleId: article.id1, rate: rate1, regionId: region.id1}, {articleId: article.id2, rate: rate2, regionId: region.id2}, ...]
                """;

            sb.Append(chatGptQuery);

            sb.AppendLine("Articles: ");
            for (int i = 0; i < articles.Count && i < 30; i++) {
                sb.AppendLine("{" + i + "," + articles[i].Title + "}");
            }

            sb.AppendLine("Regions: ");
            for(int i = 0; i < regionWithCountryNames.Count; i++)
            {
                sb.AppendLine("{" + i + "," + regionWithCountryNames[i] + "}");
            }

            return sb.ToString();
        }

        private async Task<string> TryToAskChatGptWithApi(string query)
        {
            try
            {
                var apiKey = _configuration["OpenAI:Key"];
                var apiUrl = "https://api.openai.com/v1/chat/completions";
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                    var requestBody = new
                    {
                        model = _configuration["OpenAI:Model"],
                        messages = new[] { new { role = "user", content = query } }
                    };

                    var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, requestContent);

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);

                    var textContent = jsonResponse["choices"][0]["message"]["content"].ToString();

                    // remove "```json ```" lines
                    if (textContent != null && textContent[0] != '[')
                    {
                        var lines = textContent.Split('\n');
                        if (lines.Length <= 2)
                        {
                            return "";
                        }

                        var result = string.Join('\n', lines, 1, lines.Length - 2);
                        return result;
                    }
                    else
                    {
                        return textContent;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChatGpt sending request error");
                return null;
            }
        }

        public async Task<bool> RateArticlesAsync(List<Article> articles, List<string> regions, List<string> countries, List<string> regionWithCountryNames)
        {
            string query = CreateChatGptQuery(articles, regionWithCountryNames);
            _logger.LogDebug("ChatGptQuery: {Query}", query);
            try
            {
                string response = await TryToAskChatGptWithApi(query);
                _logger.LogDebug("ChatGptResponse {response}", response);


                JArray answersList = JArray.Parse(response);
                foreach (var answer in answersList)
                {
                    int id = (int)answer["articleId"];
                    int rate = (int)answer["rate"];
                    int regionId = (int)answer["regionId"];

                    if(regionId >= 0)
                    {
                        articles[id].RegionName = regions[regionId];
                        _logger.LogDebug("Article {ArticleTitle} assigned to Region {RegionName}", articles[id].Title, articles[id].RegionName);
                    }

                    articles[id].PositioningRate = rate;
                    _logger.LogDebug("Article {ArticleTitle} rated Rate = {Rate}", articles[id].Title, articles[id].PositioningRate);
                }

                _logger.LogDebug("Successfully rated and assigned articles");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ChatGpt query error");
                return false;
            }
        }
    }
}
