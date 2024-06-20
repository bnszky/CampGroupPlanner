using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Server.Data;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Messages;
using Microsoft.Extensions.Logging;
using TripPlanner.Server.Models.DTOs.Outgoing;
using TripPlanner.Server.Models.DTOs.Incoming;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly TripDbContext _dbContext;
        private readonly IRegionFetchService _regionFetchService;
        private readonly ICityService _cityService;
        private readonly IImageService _imageService;
        private readonly IErrorService _errorService;
        private readonly IRegionService _regionService;
        private readonly ILogger<RegionController> _logger;

        public RegionController(
            TripDbContext dbContext,
            IRegionFetchService regionFetchService,
            ICityService citiesService,
            IImageService imageService,
            IErrorService errorService,
            IRegionService regionService,
            ILogger<RegionController> logger)
        {
            _regionFetchService = regionFetchService;
            _cityService = citiesService;
            _dbContext = dbContext;
            _imageService = imageService;
            _errorService = errorService;
            _regionService = regionService;
            _logger = logger;
        }

        [HttpGet("description/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> FetchDescription(string region)
        {
            try
            {
                var desc = await _regionFetchService.GetDescriptionForRegion(region, 1000);
                _logger.LogInformation("{Message} Region: {Region}, Description: {Description}", ResponseMessages.DescriptionFetchedRegion, region, desc);
                return Ok(desc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} Region: {Region}", ResponseMessages.CouldNotFetchDescription, region);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchDescription);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("cities/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<string>>> FetchCities(string region)
        {
            try
            {
                var cities = await _regionFetchService.FindCitiesByRegion(region, 5);
                _logger.LogInformation("{Message} Region: {Region}, CitiesCount: {Count}", ResponseMessages.CitiesFetchedRegion, region, cities.Count);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} Region: {Region}", ResponseMessages.CouldNotFetchCities, region);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchCities);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("images/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<string>>> FetchImages(string region)
        {
            try
            {
                var images = await _regionFetchService.GetImagesForRegion(region, 10);
                _logger.LogInformation("{Message} Region: {Region}, ImagesCount: {Count}", ResponseMessages.ImagesFetchedRegion, region, images.Count);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} Region: {Region}", ResponseMessages.CouldNotFetchImages, region);
                var errorResponse = _errorService.CreateError(ResponseMessages.CouldNotFetchImages);
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("names")]
        [AllowAnonymous]
        public async Task<ActionResult<List<string>>> GetAllNames()
        {
            try
            {
                var regionNames = await _regionService.GetAllRegionNamesAsync();
                _logger.LogInformation("{Message} RegionNamesCount: {Count}", ResponseMessages.RegionNamesFetched, regionNames.Count);
                return Ok(regionNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchRegionNames);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchRegionNames));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<RegionMiniGetDto>>> GetAllMini()
        {
            try
            {
                var regions = await _regionService.GetAllRegionMinisAsync();
                _logger.LogInformation("{Message} RegionsCount: {Count}", ResponseMessages.RegionFetched, regions.Count);
                return Ok(regions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.CouldNotFetchRegion);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchRegion));
            }
        }

        [HttpGet("{regionName}/mini")]
        [AllowAnonymous]
        public async Task<ActionResult<RegionMiniGetDto>> GetMini(string regionName)
        {
            try
            {
                var region = await _regionService.GetRegionMiniByNameAsync(regionName);
                if (region == null)
                {
                    _logger.LogError("{Message} Region: {Region}", ResponseMessages.RegionNotFound, regionName);
                    return BadRequest(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }
                _logger.LogInformation("{Message} Region: {Region}", ResponseMessages.RegionFetched, region);
                return Ok(region);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} Region: {Region}", ResponseMessages.CouldNotFetchRegion, regionName);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchRegion));
            }
        }

        [HttpGet("{regionName}")]
        [AllowAnonymous]
        public async Task<ActionResult<RegionGetDto>> Get(string regionName)
        {
            try
            {
                var region = await _regionService.GetRegionByNameAsync(regionName);
                if (region == null)
                {
                    _logger.LogError("{Message} Region: {Region}", ResponseMessages.RegionNotFound, regionName);
                    return NotFound(_errorService.CreateError(ResponseMessages.RegionNotFound, StatusCodes.Status404NotFound));
                }

                _logger.LogInformation("{Message} Region: {Region}", ResponseMessages.RegionFetched, region);
                return Ok(region);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message} Region: {Region}", ResponseMessages.CouldNotFetchRegion, regionName);
                return BadRequest(_errorService.CreateError(ResponseMessages.CouldNotFetchRegion));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] RegionCreateDto regionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var errorResponse = await _regionService.CreateRegionAsync(regionCreate);
                if (errorResponse != null)
                {
                    _logger.LogError("{Message} Error: {ErrorResponse}", ResponseMessages.RegionCreateError, errorResponse);
                    return BadRequest(errorResponse);
                }

                _logger.LogInformation("{Message} Region: {Region}", ResponseMessages.RegionCreated, regionCreate);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.UnexpectedError);
                return BadRequest(_errorService.CreateError(ResponseMessages.UnexpectedError));
            }
        }

        [HttpPut("{regionName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string regionName, [FromForm] RegionCreateDto regionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("{Message} ModelState: {ModelState}", ResponseMessages.InvalidModelState, ModelState);
                    return BadRequest(ModelState);
                }

                var errorResponse = await _regionService.UpdateRegionAsync(regionName, regionCreate);
                if (errorResponse != null)
                {
                    _logger.LogError("{Message} Error: {ErrorResponse}", ResponseMessages.RegionUpdateError, errorResponse);
                    return BadRequest(errorResponse);
                }

                _logger.LogInformation("{Message} Region: {Region}", ResponseMessages.RegionUpdated, regionCreate);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.UnexpectedError);
                return BadRequest(_errorService.CreateError(ResponseMessages.UnexpectedError));
            }
        }

        [HttpDelete("{regionName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string regionName)
        {
            try
            {
                var errorResponse = await _regionService.DeleteRegionAsync(regionName);
                if (errorResponse != null)
                {
                    _logger.LogError("{Message} Error: {ErrorResponse}", ResponseMessages.RegionDeleteError, errorResponse);
                    return BadRequest(errorResponse);
                }

                _logger.LogInformation("{Message} Region: {Region}", ResponseMessages.RegionDeleted, regionName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ResponseMessages.UnexpectedError);
                return BadRequest(_errorService.CreateError(ResponseMessages.UnexpectedError));
            }
        }
    }
}
