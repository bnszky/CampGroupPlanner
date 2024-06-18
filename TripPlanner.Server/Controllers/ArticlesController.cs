using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IErrorService _errorService;
        private readonly IArticleFetchService _articleFetchService;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService articleService, IErrorService errorService, IArticleFetchService articleFetchService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _errorService = errorService;
            _articleFetchService = articleFetchService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Article>>> Index()
        {
            try
            {
                var articles = await _articleService.GetAllAsync();
                _logger.LogInformation("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articles.Count);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.UnexpectedError);
                var errorResponse = _errorService.CreateError(ResponseMessages.UnexpectedError);
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
                if (articles == null || articles.Count == 0)
                {
                    _logger.LogError("{Message} Region: {RegionName}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articles.Count);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
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
                    _logger.LogError("{Message} Article ID: {ArticleId}", ResponseMessages.ArticleNotFound, id);
                    return NotFound(_errorService.CreateError(ResponseMessages.ArticleNotFound));
                }
                _logger.LogInformation("{Message} Article: {Article}", ResponseMessages.ArticlesFetched, article);
                return Ok(article);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ArticleCreate articleCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var article = new Article();

                var regionError = await _articleService.ValidateAndSetRegionAsync(articleCreate, article);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _articleService.HandleImageUploadAsync(articleCreate, article);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                var articleExistsError = _articleService.CheckArticleExists(articleCreate);
                if (articleExistsError != null)
                {
                    _logger.LogError("{Message} Error: {ArticleExistsError}", ResponseMessages.ArticleExists, articleExistsError);
                    return BadRequest(articleExistsError);
                }

                await _articleService.CreateOrUpdateArticleAsync(articleCreate, article, true);

                _logger.LogInformation("{Message} Article: {Article}", ResponseMessages.ArticleCreated, article);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleCreateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleCreateError);
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var errorMessage = await _articleService.DeleteAsync(id);
                if (errorMessage != null)
                {
                    _logger.LogError("{Message} Error: {DeleteError}", ResponseMessages.ArticleDeleteError, errorMessage);
                    return BadRequest(errorMessage);
                }

                _logger.LogInformation("{Message} Article ID: {ArticleId}", ResponseMessages.ArticleDeleted, id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleDeleteError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleDeleteError);
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [FromForm] ArticleCreate editedArticle)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var existingArticle = await _articleService.GetAsync(id);
                if (existingArticle == null)
                {
                    var errorResponse = _errorService.CreateError(ResponseMessages.ArticleNotFound, StatusCodes.Status404NotFound);
                    _errorService.AddNewErrorMessageFor(errorResponse, "ArticleId", ResponseMessages.ArticleNotFound);
                    _logger.LogError("{Message} Article ID: {ArticleId}", ResponseMessages.ArticleNotFound, id);
                    return NotFound(errorResponse);
                }

                var regionError = await _articleService.ValidateAndSetRegionAsync(editedArticle, existingArticle);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _articleService.HandleImageUploadAsync(editedArticle, existingArticle);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _articleService.CreateOrUpdateArticleAsync(editedArticle, existingArticle, false);
                _logger.LogInformation("{Message} Article: {Article}", ResponseMessages.ArticleUpdated, existingArticle);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleUpdateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleUpdateError);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("fetch/{regionName}")]
        public async Task<ActionResult<List<Article>>> FetchArticlesForRegion(string regionName)
        {
            try
            {
                var articles = await _articleFetchService.FetchArticlesByRegionNameAsync(regionName);
                _logger.LogInformation("{Message} Region: {RegionName} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, regionName, articles.Count);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("fetch")]
        public async Task<ActionResult<List<Article>>> FetchArticles()
        {
            try
            {
                var articles = await _articleFetchService.FetchArticles();
                _logger.LogInformation("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articles.Count);
                return Ok(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }
    }
}
