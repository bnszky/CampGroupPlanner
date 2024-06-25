using HtmlAgilityPack;
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

        private string? GetImageFromXML(SyndicationItem item)
        {
            var mediaContent = item.ElementExtensions.ReadElementExtensions<XmlElement>("content", "http://search.yahoo.com/mrss/").FirstOrDefault();
            if (mediaContent != null)
            {
                var imageUrl = mediaContent.GetAttribute("url");
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    return imageUrl;
                }
            }

            var mediaThumbnail = item.ElementExtensions.ReadElementExtensions<XmlElement>("thumbnail", "http://search.yahoo.com/mrss/").FirstOrDefault();
            if (mediaThumbnail != null)
            {
                var thumbnailUrl = mediaThumbnail.GetAttribute("url");
                if (!string.IsNullOrEmpty(thumbnailUrl))
                {
                    return thumbnailUrl;
                }
            }

            var enclosure = item.Links.FirstOrDefault(l => l.RelationshipType == "enclosure" && l.MediaType.StartsWith("image"));
            if (enclosure != null)
            {
                return enclosure.Uri.ToString();
            }

            return GetImageFromContent(item);
        }

        private string? GetImageFromContent(SyndicationItem item)
        {
            var content = item.Content as TextSyndicationContent;
            if (content != null)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content.Text);
                var imgNode = doc.DocumentNode.SelectSingleNode("//img");
                if (imgNode != null)
                {
                    return imgNode.GetAttributeValue("src", null);
                }
            }
            return null;
        }

        private string? GetDescriptionFromContent(SyndicationItem item)
        {
            var content = item.Content as TextSyndicationContent;
            if (content != null)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content.Text);
                var pNode = doc.DocumentNode.SelectSingleNode("//p");
                if (pNode != null)
                {
                    return pNode.InnerText;
                }
            }
            return item.Summary?.Text;
        }

        private DateTime? GetCreatedDate(SyndicationItem item)
        {
            if (item.LastUpdatedTime != null) return item.LastUpdatedTime.DateTime;
            return item.PublishDate.DateTime;
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
                        int cnt = 0;
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
                                    Description = GetDescriptionFromContent(item),
                                    CreatedAt = GetCreatedDate(item),
                                    SourceLink = item.Links.FirstOrDefault()?.Uri.ToString(),
                                    ImageURL = imageUrl,
                                };
                                articles.Add(article);
                                cnt++;
                            }
                        }
                        if (cnt == 0)
                        {
                            _logger.LogWarning("Couldn't fetch any article from RSS: {Feed}", feed);
                        }
                        else
                        {
                            _logger.LogDebug("Fetched {cnt} articles from RSS: {Feed}", cnt, feed);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error fetching or parsing RSS feed: {Feed}", feed);
                    }
                }
            }

            return articles;
        }

        public async Task<List<Article>> GetArticlesByRegionNameAsync(string regionName)
        {
            return new List<Article>();
        }
    }
}