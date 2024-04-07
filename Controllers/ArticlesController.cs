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
			List<Article> articles = await _articleService.GetArticlesAsync();

			return View(articles);
		}

        public async Task<IActionResult> AggregateAsync(string? rssLink)
        {
            if(rssLink == null) rssLink = @"https://feeds.feedburner.com/breakingtravelnews/news/tourism";
            await _articleService.AggregateFromRSS(rssLink);

            return RedirectToAction("Index");
        }

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Article article)
		{
			if(ModelState.IsValid)
			{
				await _articleService.Create(article);
				return RedirectToAction("Index");
			} else
			{
				return View();
			}
		}
    }
}
