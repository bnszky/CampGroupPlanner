using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleRatingService : IArticleRatingService
    {
        public ArticleRatingService() { }
        public async Task<bool> IsArticleValidAsync(Article article)
        {
            if (article.Description == null) return false;
            if (article.CreatedAt < DateTime.UtcNow.AddYears(-5)) return false;
            return true;
        }

        private int CountOccurrences(string text, string keyword)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword)) return 0;

            int count = 0;
            int index = 0;

            while ((index = text.IndexOf(keyword, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += keyword.Length;
            }

            return count;
        }

        private int CountKeywordsOccurances(List<string> keywords, Article article)
        {
            int count = 0;
            foreach (string keyword in keywords)
            {
                if(article.Description != null) {
                    count += CountOccurrences(article.Description, keyword);
                }
                count += CountOccurrences(article.Title, keyword);
            }
            return count;
        }

        public async Task<int?> RateArticleAsync(Article article, string regionName, string countryName, List<string> cityNames)
        {
            if (regionName == null) return 0;

            int cnt = 0;
            List<string> keywords = [regionName, countryName];
            keywords.AddRange(cityNames);
            cnt += CountKeywordsOccurances(keywords, article);

            if(cnt == 0) return null;
            return cnt;
        }
    }
}
