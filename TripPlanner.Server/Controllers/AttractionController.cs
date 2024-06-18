using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;
using TripPlanner.Server.Messages;
using Microsoft.Extensions.Logging;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _attractionService;
        private readonly IErrorService _errorService;
        private readonly IRegionService _regionService;
        private readonly IAttractionFetchService _attractionFetchService;
        private readonly ILogger<AttractionController> _logger;

        public AttractionController(
            IAttractionService attractionService,
            IErrorService errorService,
            IRegionService regionService,
            IAttractionFetchService attractionFetchService,
            ILogger<AttractionController> logger)
        {
            _errorService = errorService;
            _attractionService = attractionService;
            _regionService = regionService;
            _attractionFetchService = attractionFetchService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Attraction>>> GetAll()
        {
            try
            {
                var attractions = await _attractionService.GetAllAsync();
                _logger.LogInformation("{Message} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, attractions.Count);
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("region/{regionName}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Attraction>>> GetByRegion(string regionName)
        {
            try
            {
                var attractions = await _attractionService.GetAllByRegionAsync(regionName);
                if (attractions == null || attractions.Count == 0)
                {
                    _logger.LogError("{Message} Region: {RegionName}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} Region: {RegionName} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, regionName, attractions.Count);
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Attraction>> Get(int id)
        {
            try
            {
                var attraction = await _attractionService.GetAsync(id);
                if (attraction == null)
                {
                    _logger.LogError("{Message} Attraction ID: {AttractionId}", ResponseMessages.AttractionNotFound, id);
                    return NotFound(_errorService.CreateError(ResponseMessages.AttractionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} Attraction: {Attraction}", ResponseMessages.AttractionsFetched, attraction);
                return Ok(attraction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromForm] AttractionCreate attractionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var attraction = new Attraction();

                var regionError = await _attractionService.ValidateAndSetRegionAsync(attractionCreate, attraction);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _attractionService.HandleImageUploadAsync(attractionCreate, attraction);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, attraction.Region, attraction.ImageURL);
                _logger.LogInformation("{Message} Attraction: {Attraction}", ResponseMessages.AttractionCreated, attraction);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.AttractionCreateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.AttractionCreateError);
                return BadRequest(errorResponse);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([FromForm] AttractionCreate attractionCreate, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var existingAttraction = await _attractionService.GetAsync(id);
                if (existingAttraction == null)
                {
                    var errorResponse = _errorService.CreateError(ResponseMessages.AttractionNotFound, StatusCodes.Status404NotFound);
                    _errorService.AddNewErrorMessageFor(errorResponse, "AttractionId", ResponseMessages.AttractionNotFound);
                    _logger.LogError("{Message} Attraction ID: {AttractionId}", ResponseMessages.AttractionNotFound, id);
                    return NotFound(errorResponse);
                }

                var regionError = await _attractionService.ValidateAndSetRegionAsync(attractionCreate, existingAttraction);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _attractionService.HandleImageUploadAsync(attractionCreate, existingAttraction);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, existingAttraction.Region, existingAttraction.ImageURL, existingAttraction);
                _logger.LogInformation("{Message} Attraction: {Attraction}", ResponseMessages.AttractionUpdated, existingAttraction);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.AttractionUpdateError);
                var errorResponse = _errorService.CreateError(ResponseMessages.AttractionUpdateError);
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var errorMessage = await _attractionService.DeleteAsync(id);
                if (errorMessage != null)
                {
                    _logger.LogError("{Message} Error: {DeleteError}", ResponseMessages.AttractionDeleteError, errorMessage);
                    return BadRequest(errorMessage);
                }
                _logger.LogInformation("{Message} Attraction ID: {AttractionId}", ResponseMessages.AttractionDeleted, id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.AttractionDeleteError);
                var errorResponse = _errorService.CreateError(ResponseMessages.AttractionDeleteError);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("fetch/{regionName}")]
        public async Task<ActionResult<List<Attraction>>> FetchAttractionsByRegionName(string regionName)
        {
            try
            {
                var cities = await _regionService.GetCitiesByRegionName(regionName);
                var attractions = await _attractionFetchService.FetchAttractionsForGivenCities(cities, 10);
                _logger.LogInformation("{Message} Region: {RegionName} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, regionName, attractions.Count);
                return Ok(attractions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }
    }
}
