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

		public ArticleService(AppDbContext dbContext)
		{
			_dbContext = dbContext;
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
						Content = item.Content?.ToString(),
						CreatedAt = item.PublishDate.DateTime,
						Author = item.Authors.FirstOrDefault()?.Name,
						SourceLink = item.Links.FirstOrDefault()?.Uri?.AbsoluteUri
					};

					if(article != null && !_dbContext.Articles.Any(e => e.SourceLink == article.SourceLink))
					{
                        await _dbContext.AddAsync(article);
                    }
				}

				await _dbContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				// Handle any exceptions (e.g., invalid feed URL, parsing errors)
				Console.WriteLine($"Error fetching RSS feed: {ex.Message}");
			}
		}

		public Task<List<Article>> GetArticlesAsync()
		{
			return _dbContext.Articles.ToListAsync();
		}
	}
}
