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
				SourceLink = article.SourceLink,
				ImageUrl = article.ImageLink
			}).ToList();

			return View(model);
		}

        public async Task<IActionResult> AggregateAsync(string? rssLink)
        {
            if(rssLink == null) rssLink = @"https://feeds.feedburner.com/breakingtravelnews/news/tourism";
            await _articleService.AggregateFromRSS(rssLink);

            return RedirectToAction("Index");
        }
    }
}
