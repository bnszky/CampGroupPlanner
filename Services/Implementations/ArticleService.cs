using CampGroupPlanner.Data;
using CampGroupPlanner.Models;
using CampGroupPlanner.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel.Syndication;
using System.Xml;

namespace CampGroupPlanner.Services.Implementations
{
	public class ArticleService : IArticleService
	{
		private AppDbContext _dbContext;
		private ILocationDetectionService _locationDetectionService;

		public ArticleService(AppDbContext dbContext, ILocationDetectionService locationDetectionService)
		{
			_dbContext = dbContext;
			_locationDetectionService = locationDetectionService;
		}

		private string? GetImageFromXML(SyndicationItem? item)
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
			return null;
        }
		public async Task AggregateFromRSS(string link)
		{
			try
			{
				var reader = XmlReader.Create(link);
				var feed = SyndicationFeed.Load(reader);

				foreach (var item in feed.Items)
				{
					var article = new Article
					{
						Id = Guid.NewGuid(),
						Title = item.Title.Text,
						Description = item.Summary?.Text,
						CreatedAt = item.PublishDate.DateTime,
						Author = item.Authors.FirstOrDefault()?.Name,
						SourceLink = item.Links.FirstOrDefault()?.Uri?.AbsoluteUri,
						ImageLink = GetImageFromXML(item)
					};
					// only for testing Azure
					// _locationDetectionService.GetLocalizationsFromTextAsync(article.Title + "\n" + article.Description);


                    if (article != null && !_dbContext.Articles.Any(e => e.SourceLink == article.SourceLink))
					{
                        await _dbContext.AddAsync(article);
                    }
				}

				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error fetching RSS feed: {ex.Message}");
			}
		}

		public Task<List<Article>> GetArticlesAsync()
		{
			return _dbContext.Articles.ToListAsync();
		}
	}
}
