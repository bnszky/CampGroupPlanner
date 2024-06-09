using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;
using static System.Net.Mime.MediaTypeNames;
using Image = TripPlanner.Server.Models.Image;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly TripDbContext _dbContext;
        private IRegionFetchService _regionFetchService;
        private ICityService _cityService;
        private IImageService _imageService;
        private IErrorService _errorService;
        private IRegionService _regionService;
        public RegionController(TripDbContext dbContext, IRegionFetchService regionFetchService, ICityService citiesService, IImageService imageService, IErrorService errorService, IRegionService regionService) { 
            _regionFetchService = regionFetchService;
            _cityService = citiesService;
            _dbContext = dbContext;
            _imageService = imageService;
            _errorService = errorService;
            _regionService = regionService;
        }

        [HttpGet("description/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> FetchDescription(string region)
        {
            return await _regionFetchService.GetDescriptionForRegion(region, 1000);
        }

        [HttpGet("cities/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<string>>> FetchCities(string region)
        {
            return await _regionFetchService.FindCitiesByRegion(region, 5);
        }

        [HttpGet("images/{region}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<string>>> FetchImages(string region)
        {
            return await _regionFetchService.GetImagesForRegion(region, 10);
        }

        [HttpGet("names")]
        [AllowAnonymous]
        public async Task<ActionResult<List<string>>> GetAllNames()
        {
            try
            {
                return await _regionService.GetAllRegionNamesAsync();
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<RegionMini>>> GetAllMini()
        {
            try
            {
                return await _regionService.GetAllRegionMinisAsync();
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
            }
        }

        [HttpGet("{regionName}/mini")]
        [AllowAnonymous]
        public async Task<ActionResult<RegionMini>> GetMini(string regionName)
        {
            try
            {
                var region = await _regionService.GetRegionMiniByNameAsync(regionName);
                if(region == null)
                {
                    return BadRequest(_errorService.CreateError("Region with this name doesn't exist"));
                }
                return region;
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
            }
        }

        [HttpGet("{regionName}")]
        [AllowAnonymous]
        public async Task<ActionResult<RegionGet>> Get(string regionName)
        {
            try
            {
                var region = await _regionService.GetRegionByNameAsync(regionName);
                if (region == null)
                {
                    return NotFound(_errorService.CreateError("Region not found"));
                }

                return Ok(region);
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Region not found"));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] RegionCreate regionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var errorResponse = await _regionService.CreateRegionAsync(regionCreate);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }

                return Ok();
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
            }
        }

        [HttpPut("{regionName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string regionName, [FromForm] RegionCreate regionCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var errorResponse = await _regionService.UpdateRegionAsync(regionName, regionCreate);
                if (errorResponse != null)
                {
                    return BadRequest(errorResponse);
                }

                return Ok();
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
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
                    return BadRequest(errorResponse);
                }

                return Ok();
            }
            catch
            {
                return BadRequest(_errorService.CreateError("Unexpected error"));
            }
        }


    }
}
