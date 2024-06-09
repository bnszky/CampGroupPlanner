using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _attractionService;
        private readonly IErrorService _errorService;
        public AttractionController(IAttractionService attractionService, IErrorService errorService)
        {
            _errorService = errorService;
            _attractionService = attractionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Attraction>>> GetAll()
        {
            try
            {
                return await _attractionService.GetAllAsync();
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't fetch attractions from database");
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("region/{regionName}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Attraction>>> GetByRegion(string regionName)
        {
            try
            {
                var articles = await _attractionService.GetAllByRegionAsync(regionName);
                if (articles == null)
                {
                    return NotFound(_errorService.CreateError("Region with this name doesn't exist", 404));
                }
                return Ok(articles);
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't fetch attractions from database");
                return BadRequest(errorResponse);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Attraction>> Get(int id)
        {
            try
            {
                var att = await _attractionService.GetAsync(id);
                if (att == null) { throw new Exception("Couldn't find attraction with this id"); }
                return Ok(att);
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't find attraction with this id");
                return BadRequest(errorResponse);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromForm] AttractionCreate attractionCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attraction = new Attraction();

            var regionError = await _attractionService.ValidateAndSetRegionAsync(attractionCreate, attraction);
            if (regionError != null)
            {
                return BadRequest(regionError);
            }

            var imageError = await _attractionService.HandleImageUploadAsync(attractionCreate, attraction);
            if (imageError != null)
            {
                return BadRequest(imageError);
            }

            await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, attraction.Region, attraction.ImageURL);

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([FromForm] AttractionCreate attractionCreate, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAttraction = await _attractionService.GetAsync(id);
            if (existingAttraction == null)
            {
                var errorResponse = _errorService.CreateError("Attraction not found", 404);
                _errorService.AddNewErrorMessageFor(errorResponse, "AttractionId", "Couldn't find attraction with this id");
                return NotFound(errorResponse);
            }

            var regionError = await _attractionService.ValidateAndSetRegionAsync(attractionCreate, existingAttraction);
            if (regionError != null)
            {
                return BadRequest(regionError);
            }

            var imageError = await _attractionService.HandleImageUploadAsync(attractionCreate, existingAttraction);
            if (imageError != null)
            {
                return BadRequest(imageError);
            }

            await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, existingAttraction.Region, existingAttraction.ImageURL, existingAttraction);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id) {
            var errorMessage = await _attractionService.DeleteAsync(id);
            if (errorMessage != null) {
                return BadRequest(errorMessage);
            }

            return Ok();
        }
    }
}
