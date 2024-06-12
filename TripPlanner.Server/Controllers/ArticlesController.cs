using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IErrorService _errorService;
        private readonly IArticleFetchService _articleFetchService;

        public ArticlesController(IArticleService articleService, IErrorService errorService, IArticleFetchService articleFetchService)
        {
            _articleService = articleService;
            _errorService = errorService;
            _articleFetchService = articleFetchService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Article>>> Index()
        {
            try
            {
                return await _articleService.GetAllAsync();
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't fetch articles from database");
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("region/{regionName}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Article>>> GetByRegion(string regionName)
        {
            try
            {
                var articles = await _articleService.GetAllByRegionAsync(regionName);
                if(articles == null)
                {
                    return NotFound(_errorService.CreateError("Region with this name doesn't exist", 404));
                }
                return Ok(articles);
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't fetch articles from database");
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Article>> Get(int id)
        {
            try
            {
                var article = await _articleService.GetAsync(id);
                if (article == null)
                {
                    throw new Exception("Couldn't find attraction with this id");
                }
                return Ok(article);
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't find article with this id");
                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ArticleCreate articleCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Article article = new Article();

            var regionError = await _articleService.ValidateAndSetRegionAsync(articleCreate, article);
            if (regionError != null)
            {
                return BadRequest(regionError);
            }

            var imageError = await _articleService.HandleImageUploadAsync(articleCreate, article);
            if (imageError != null)
            {
                return BadRequest(imageError);
            }

            var articleExistsError = _articleService.CheckArticleExists(articleCreate);
            if (articleExistsError != null)
            {
                return BadRequest(articleExistsError);
            }

            await _articleService.CreateOrUpdateArticleAsync(articleCreate, article, true);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var errorMessage = await _articleService.DeleteAsync(id);
            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [FromForm] ArticleCreate editedArticle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingArticle = await _articleService.GetAsync(id);
            if (existingArticle == null)
            {
                var errorResponse = _errorService.CreateError("Article not found", 404);
                _errorService.AddNewErrorMessageFor(errorResponse, "ArticleId", "Couldn't find article with this id");
                return NotFound(errorResponse);
            }

            var regionError = await _articleService.ValidateAndSetRegionAsync(editedArticle, existingArticle);
            if (regionError != null)
            {
                return BadRequest(regionError);
            }

            var imageError = await _articleService.HandleImageUploadAsync(editedArticle, existingArticle);
            if (imageError != null)
            {
                return BadRequest(imageError);
            }

            await _articleService.CreateOrUpdateArticleAsync(editedArticle, existingArticle, false);

            return Ok();
        }

        [HttpGet("fetch/{regionName}")]
        public async Task<ActionResult<List<Article>>> FetchArticles(string regionName)
        {
            return await _articleFetchService.FetchArticlesByRegionNameAsync(regionName);
        }
    }
}
