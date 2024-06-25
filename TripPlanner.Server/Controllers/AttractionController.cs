using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;
using TripPlanner.Server.Messages;
using Microsoft.Extensions.Logging;
using TripPlanner.Server.Models.Database;
using AutoMapper;
using TripPlanner.Server.Models.DTOs.Outgoing;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models;

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
        private readonly IMapper _mapper;

        public AttractionController(IAttractionService attractionService, IErrorService errorService, IRegionService regionService, IAttractionFetchService attractionFetchService, ILogger<AttractionController> logger, IMapper mapper)
        {
            _errorService = errorService;
            _attractionService = attractionService;
            _regionService = regionService;
            _attractionFetchService = attractionFetchService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Return all attractions
        /// </summary>
        /// <response code="200">Returns list of attractions</response>
        /// <response code="400">Error object</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<AttractionGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<AttractionGetDto>>> GetAll()
        {
            try
            {
                var attractions = await _attractionService.GetAllAsync();
                var attractionDtos = _mapper.Map<IEnumerable<AttractionGetDto>>(attractions).ToList();
                _logger.LogInformation("{Message} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, attractionDtos.Count);
                return Ok(attractionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Return all attractions for given region
        /// </summary>
        /// <response code="200">Returns list of attractions for given region</response>
        /// <response code="400">Error object</response>
        [HttpGet("region/{regionName}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<AttractionGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<AttractionGetDto>>> GetByRegion(string regionName)
        {
            try
            {
                var attractions = await _attractionService.GetAllByRegionAsync(regionName);
                var attractionDtos = _mapper.Map<IEnumerable<AttractionGetDto>>(attractions).ToList();
                _logger.LogInformation("{Message} Region: {RegionName} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, regionName, attractionDtos.Count);
                return Ok(attractionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Get attraction with specific id
        /// </summary>
        /// <response code="200">Return attraction</response>
        /// <response code="400">Error object</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AttractionGetDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AttractionGetDto>> Get(int id)
        {
            try
            {
                var attraction = await _attractionService.GetAsync(id);
                var attractionDto = _mapper.Map<AttractionGetDto>(attraction);
                if (attractionDto == null)
                {
                    _logger.LogError("{Message} Attraction ID: {AttractionId}", ResponseMessages.AttractionNotFound, id);
                    return NotFound(_errorService.CreateError(ResponseMessages.AttractionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} Attraction: {Attraction}", ResponseMessages.AttractionsFetched, attractionDto);
                return Ok(attractionDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Create attraction
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromForm] AttractionCreateDto attractionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var attraction = _mapper.Map<Attraction>(attractionCreate);

                var regionError = await _attractionService.ValidateAndSetRegionAsync(attraction);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _attractionService.HandleImageUploadAsync(attraction, attractionCreate.Image);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _attractionService.CreateOrUpdateAttractionAsync(attraction, true);
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

        /// <summary>
        /// Edit attraction
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit([FromForm] AttractionCreateDto attractionEdited, int id)
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

                var attraction = _mapper.Map(attractionEdited, existingAttraction);

                var regionError = await _attractionService.ValidateAndSetRegionAsync(attraction);
                if (regionError != null)
                {
                    _logger.LogError("{Message} Error: {RegionError}", ResponseMessages.RegionValidationError, regionError);
                    return BadRequest(regionError);
                }

                var imageError = await _attractionService.HandleImageUploadAsync(attraction, attractionEdited.Image);
                if (imageError != null)
                {
                    _logger.LogError("{Message} Error: {ImageError}", ResponseMessages.ImageUploadError, imageError);
                    return BadRequest(imageError);
                }

                await _attractionService.CreateOrUpdateAttractionAsync(attraction, false);
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

        /// <summary>
        /// Delete attraction
        /// </summary>
        /// <response code="400">Error object</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Fetch attractions from api and store in db
        /// </summary>
        /// <response code="500">Error object</response>
        /// <response code="200">Return List of Fetched Attractions</response>
        [HttpGet("fetch/{regionName}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(List<AttractionGetDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AttractionGetDto>>> FetchAttractionsByRegionName(string regionName)
        {
            try
            {
                var cities = await _regionService.GetCitiesByRegionName(regionName);
                var attractions = await _attractionFetchService.FetchAttractionsForGivenCities(cities, 10);
                var attractionDtos = _mapper.Map<IEnumerable<AttractionGetDto>>(attractions).ToList();
                _logger.LogInformation("{Message} Region: {RegionName} AttractionsCount: {Count}", ResponseMessages.AttractionsFetched, regionName, attractionDtos.Count);
                return Ok(attractionDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchAttractions);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchAttractions);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
