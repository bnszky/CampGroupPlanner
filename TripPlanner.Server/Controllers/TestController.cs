using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Controllers
{
    public class TestController : ControllerBase
    {
        private readonly IArticleRatingService _articleRatingService;
        private readonly TripDbContext _context;
        public TestController(IArticleRatingService articleRatingService, TripDbContext context) {
            _articleRatingService = articleRatingService;
            _context = context;
        }

        [HttpGet("message")]
        public async Task<IActionResult> GetMessageForAllRegions()
        {
            List<string> regions = _context.Regions.OrderBy(r => r.Id).Select(r => r.Name).ToList();
            List<string> countries = _context.Regions.OrderBy(r => r.Id).Select(r => r.Country).ToList();
            List<string> regionWithCountries = _context.Regions.Select(r => r.Name + " - " + r.Country).ToList();

            List<Article> articles = _context.Articles.Take(50).ToList();

            await _articleRatingService.RateArticlesAsync(articles, regions, countries, regionWithCountries);
            return Ok();
        }

        [HttpGet("message/{regionName}")]
        public async Task<IActionResult> GetMessageForRegion(string regionName)
        {
            List<string> regions = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Name).ToList();
            List<string> countries = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Country).ToList();
            List<string> regionWithCountries = _context.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Select(r => r.Name + " - " + r.Country).ToList();

            List<Article> articles = _context.Articles.Take(50).ToList();

            await _articleRatingService.RateArticlesAsync(articles, regions, countries, regionWithCountries);
            return Ok();
        }
    }
}
