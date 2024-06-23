using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Services.Abstractions;
using Microsoft.Extensions.Logging;
using TripPlanner.Server.Models.Database;
using AutoMapper;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService, IErrorService errorService, IArticleFetchService articleFetchService, ILogger<ArticlesController> logger, IMapper mapper)
        {
            _articleService = articleService;
            _errorService = errorService;
            _articleFetchService = articleFetchService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ArticleGetDto>>> Index()
        {
            try
            {
                var articles = await _articleService.GetAllAsync();
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogInformation("{Message} ArticlesDtosCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
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
        public async Task<ActionResult<List<ArticleGetDto>>> GetByRegion(string regionName)
        {
            try
            {
                var articles = await _articleService.GetAllByRegionAsync(regionName);
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                if (articleDtos == null || articleDtos.Count == 0)
                {
                    _logger.LogError("{Message} Region: {RegionName}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
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
        public async Task<ActionResult<ArticleGetDto>> Get(int id)
        {
            try
            {
                var article = await _articleService.GetAsync(id);
                var articleDto = _mapper.Map<ArticleGetDto>(article);
                if (articleDto == null)
                {
                    _logger.LogError("{Message} Article ID: {ArticleId}", ResponseMessages.ArticleNotFound, id);
                    return NotFound(_errorService.CreateError(ResponseMessages.ArticleNotFound));
                }
                _logger.LogInformation("{Message} Article: {Article}", ResponseMessages.ArticlesFetched, articleDto);
                return Ok(articleDto);
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
        public async Task<IActionResult> Create([FromForm] ArticleCreateDto articleCreate)
        
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var article = _mapper.Map<Article>(articleCreate);

                var regionError = await _articleService.ValidateAndSetRegionAsync(article);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _articleService.HandleImageUploadAsync(article, articleCreate.ImageFile);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                var articleExistsError = _articleService.CheckArticleExists(article);
                if (articleExistsError != null)
                {
                    _logger.LogError("{Message} Error: {ArticleExistsError}", ResponseMessages.ArticleExists, articleExistsError);
                    return BadRequest(articleExistsError);
                }

                await _articleService.CreateOrUpdateArticleAsync(article, true);

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
        public async Task<IActionResult> Edit(int id, [FromForm] ArticleCreateDto editedArticle)
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

                var article = _mapper.Map(editedArticle, existingArticle);

                var regionError = await _articleService.ValidateAndSetRegionAsync(article);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _articleService.HandleImageUploadAsync(article, editedArticle.ImageFile);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _articleService.CreateOrUpdateArticleAsync(article, false);
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
        public async Task<ActionResult<List<ArticleGetDto>>> FetchArticlesForRegion(string regionName)
        {
            try
            {
                var articles = await _articleFetchService.FetchArticlesByRegionNameAsync(regionName);
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogInformation("{Message} Region: {RegionName} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, regionName, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("fetch")]
        public async Task<ActionResult<List<ArticleGetDto>>> FetchArticles()
        {
            try
            {
                var articles = await _articleFetchService.FetchArticles();
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogInformation("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("rate-or-assign")]
        public async Task<ActionResult<List<ArticleGetDto>>> TryToAssignAndRateArticlesAgain()
        {
            try
            {
                var articles = await _articleFetchService.TryAssignAndRateExistingArticles();
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogInformation("{Message} ArticlesUpdatedCount: {Count}", "Articles updated", articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", "Couldn't update articles");
                var errorResponse = _errorService.CreateError("Couldn't update articles");
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("delete-below-rate/{rate}")]
        public async Task<ActionResult> DeleteArticlesBelowRate(int rate)
        {
            var errorResponse = await _articleService.DeleteBelowRate(rate);
            if(errorResponse == null)
            {
                _logger.LogInformation("Successfully deleted articles below rate {Rate}", rate);
                return Ok($"Successfully deleted articles below rate {rate}");
            }

            _logger.LogError(errorResponse.Title);
            return BadRequest(errorResponse);
        }
    }
}
