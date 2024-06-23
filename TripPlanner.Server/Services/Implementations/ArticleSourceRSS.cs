using Microsoft.Extensions.Configuration;
using System.ServiceModel.Syndication;
using System.Xml;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleSourceRSS : IArticleSourceService
    {
        private readonly ILogger<ArticleSourceRSS> _logger;
        private readonly IConfiguration _configuration;
        public ArticleSourceRSS(ILogger<ArticleSourceRSS> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private string? GetImageFromXML(SyndicationItem? item)
        {
            // Check for Media RSS content
            var mediaContent = item.ElementExtensions.ReadElementExtensions<XmlElement>("content", "http://search.yahoo.com/mrss/").FirstOrDefault();
            if (mediaContent != null)
            {
                var imageUrl = mediaContent.GetAttribute("url");
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    return imageUrl;
                }
            }

            // Check for Media RSS thumbnail
            var mediaThumbnail = item.ElementExtensions.ReadElementExtensions<XmlElement>("thumbnail", "http://search.yahoo.com/mrss/").FirstOrDefault();
            if (mediaThumbnail != null)
            {
                var thumbnailUrl = mediaThumbnail.GetAttribute("url");
                if (!string.IsNullOrEmpty(thumbnailUrl))
                {
                    return thumbnailUrl;
                }
            }

            // Check for enclosure element
            var enclosure = item.Links.FirstOrDefault(l => l.RelationshipType == "enclosure" && l.MediaType.StartsWith("image"));
            if (enclosure != null)
            {
                return enclosure.Uri.ToString();
            }

            return null;
        }
        public async Task<List<Article>> GetArticlesAsync()
        {
            var articles = new List<Article>();
            var feeds = _configuration.GetSection("RssFeeds:Links").Get<List<string>>();

            using (var httpClient = new HttpClient())
            {
                foreach (var feed in feeds)
                {
                    try
                    {
                        var response = await httpClient.GetStringAsync(feed);
                        using (var stringReader = new StringReader(response))
                        using (var xmlReader = XmlReader.Create(stringReader))
                        {
                            var rssFeed = SyndicationFeed.Load(xmlReader);
                            foreach (var item in rssFeed.Items)
                            {
                                string? imageUrl = GetImageFromXML(item);
                                var article = new Article
                                {
                                    Title = item.Title.Text,
                                    Description = item.Summary?.Text,
                                    CreatedAt = item.PublishDate.DateTime,
                                    SourceLink = item.Links.FirstOrDefault()?.Uri.ToString(),
                                    ImageURL = imageUrl,
                                };
                                articles.Add(article);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error fetching or parsing RSS feed: {feed}");
                        return [];
                    }
                }
            }

            return articles;
        }

        public async Task<List<Article>> GetArticlesByRegionNameAsync(string regionName)
        {
            return [];
        }
    }
}
