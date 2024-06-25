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
using static System.Runtime.InteropServices.JavaScript.JSType;
using TripPlanner.Server.Models;

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

        /// <summary>
        /// Return all articles (for Admin), articles with MinPositivityRate (for others)
        /// </summary>
        /// <response code="200">Returns list of articles</response>
        /// <response code="400">Error object</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ArticleGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ArticleGetDto>>> Index()
        {
            try
            {
                List<Article> articles;

                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    articles = await _articleService.GetAllAsync();
                }
                else
                {
                    articles = await _articleService.GetWithMinimalPositivityRateAsync();
                }

                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogDebug("{Message} ArticlesDtosCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.UnexpectedError);
                var errorResponse = _errorService.CreateError(ResponseMessages.UnexpectedError);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Return all articles for given region (for Admin), articles with MinPositivityRate (for others)
        /// </summary>
        /// <response code="200">Returns list of articles for given region</response>
        /// <response code="400">Error object</response>
        [HttpGet("region/{regionName}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<ArticleGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ArticleGetDto>>> GetByRegion(string regionName)
        {
            try
            {
                List<Article> articles;

                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    articles = await _articleService.GetAllByRegionAsync(regionName);
                }
                else
                {
                    articles = await _articleService.GetWithMinimalPositivityRateByRegionAsync(regionName);
                }

                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                if (articleDtos == null)
                {
                    _logger.LogError("{Message} Region: {RegionName}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogDebug("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get article with specific id
        /// </summary>
        /// <response code="200">Return article</response>
        /// <response code="400">Error object</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ArticleGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
                _logger.LogDebug("{Message} Article: {Article}", ResponseMessages.ArticlesFetched, articleDto);
                return Ok(articleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Create Article
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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

                _logger.LogDebug("{Message} Article: {Article}", ResponseMessages.ArticleCreated, article);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleCreateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleCreateError);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Delete Article
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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

                _logger.LogDebug("{Message} Article ID: {ArticleId}", ResponseMessages.ArticleDeleted, id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleDeleteError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleDeleteError);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Edit Article
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
                _logger.LogDebug("{Message} Article: {Article}", ResponseMessages.ArticleUpdated, existingArticle);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.ArticleUpdateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.ArticleUpdateError);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Fetch articles from different sources for given region 
        /// </summary>
        /// <response code="200">Fetched articles</response>
        /// <response code="400">Error object</response>
        [HttpGet("fetch/{regionName}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ArticleGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ArticleGetDto>>> FetchArticlesForRegion(string regionName)
        {
            try
            {
                var articles = await _articleFetchService.FetchArticlesByRegionNameAsync(regionName);
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogDebug("{Message} Region: {RegionName} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, regionName, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Fetch articles from different sources
        /// </summary>
        /// <response code="200">Fetched articles</response>
        /// <response code="400">Error object</response>
        [HttpGet("fetch")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ArticleGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ArticleGetDto>>> FetchArticles()
        {
            try
            {
                var articles = await _articleFetchService.FetchArticles();
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogDebug("{Message} ArticlesCount: {Count}", ResponseMessages.ArticlesFetched, articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchArticles);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchArticles);
                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Try to rate and assign articles that has not been assigned or rated yet
        /// </summary>
        /// <response code="200">Rated and assigned articles</response>
        /// <response code="400">Error object</response>
        [HttpPut("rate-or-assign")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<ArticleGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ArticleGetDto>>> TryToAssignAndRateArticlesAgain()
        {
            try
            {
                var articles = await _articleFetchService.TryAssignAndRateExistingArticles();
                var articleDtos = _mapper.Map<IEnumerable<ArticleGetDto>>(articles).ToList();
                _logger.LogDebug("{Message} ArticlesUpdatedCount: {Count}", "Articles updated", articleDtos.Count);
                return Ok(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", "Couldn't update articles");
                var errorResponse = _errorService.CreateError("Couldn't update articles");
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Delete all articles below given rate (0-100)
        /// </summary>
        /// <response code="200">Deleted articles information</response>
        /// <response code="400">Error object</response>
        [HttpDelete("delete-below-rate/{rate}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteArticlesBelowRate(int rate)
        {
            var errorResponse = await _articleService.DeleteBelowRate(rate);
            if(errorResponse == null)
            {
                _logger.LogDebug("Successfully deleted articles below rate {Rate}", rate);
                return Ok($"Successfully deleted articles below rate {rate}");
            }

            _logger.LogError(errorResponse.Title);
            return BadRequest(errorResponse);
        }

        /// <summary>
        /// Set minimal positivity rate of articles visible for users
        /// </summary>
        /// <response code="200">Successfully set minimal positivity rate</response>
        /// <response code="500">Server exception info</response>
        /// <response code="400">Inappropriate rate</response>
        [HttpPut("set-min-rate/{rate}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SetMinPositivityRate(int rate)
        {
            try
            {
                if(rate < 0 || rate > 100)
                {
                    _logger.LogError("Rate must be in range of 0-100");
                    return BadRequest("Rate must be in range of 0-100");
                }
                await _articleService.SetMinimalPositivityRate(rate);
                _logger.LogDebug("Successfully set minimal positivity rate to {Rate}", rate);
                return Ok($"Successfully set minimal positivity rate to {rate}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't set minimal positivity rate");
                return StatusCode(500, "Couldn't set minimal positivity rate");
            }
        }

        /// <summary>
        /// Get minimal positivity rate of articles visible for users
        /// </summary>
        /// <response code="200">Successfully get minimal positivity rate (integer)</response>
        /// <response code="500">Server exception info</response>
        [HttpGet("get-min-rate")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetMinPositivityRate()
        {
            try
            {
                var rate = await _articleService.GetMinimalPositivityRate();
                _logger.LogDebug("Successfully get minimal positivity rate, rate = {Rate}", rate);
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get minimal positivity rate");
                return StatusCode(500, "Couldn't get minimal positivity rate");
            }
        }
    }
}
