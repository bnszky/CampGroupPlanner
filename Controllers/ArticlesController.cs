using CampGroupPlanner.Models;
using CampGroupPlanner.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace CampGroupPlanner.Controllers
{
	public class ArticlesController : Controller
	{
		public IArticleService _articleService { get; set; }
		public ArticlesController(IArticleService articleService) {
			_articleService = articleService;
		}
		public async Task<IActionResult> Index()
		{
			List<ArticleModel> model = (await _articleService.GetArticlesAsync()).Select(article => new ArticleModel
			{
				Id = article.Id,
				Title = article.Title,
				Description = article.Description,
				PublishedDate = article.CreatedAt,
				SourceLink = article.SourceLink
			}).ToList();

			return View(model);
		}

        public async Task<IActionResult> AggregateAsync(string? rssLink)
        {
            //split our aggregation to 3 steps 
            //rss get some common data to take source url 
            //take article 'body' and copy to our storage 
            // after having an article twxt try to rate it

            if(rssLink == null) rssLink = @"https://feeds.feedburner.com/breakingtravelnews/news/tourism";
            await _articleService.AggregateFromRSS(rssLink);

            return RedirectToAction("Index");
        }
    }
}
