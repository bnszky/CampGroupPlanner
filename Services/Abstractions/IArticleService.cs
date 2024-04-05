using CampGroupPlanner.Models;

namespace CampGroupPlanner.Services.Abstractions
{
	public interface IArticleService
	{
		public Task<List<Article>> GetArticlesAsync();
		public Task AggregateFromRSS(string link);
	}
}
