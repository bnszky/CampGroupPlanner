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

        [HttpGet("{id}")]
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

            await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, attraction.Region);

            return Ok();
        }

        [HttpPut("{id}")]
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

            await _attractionService.CreateOrUpdateAttractionAsync(attractionCreate, existingAttraction.Region, existingAttraction);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            var errorMessage = await _attractionService.DeleteAsync(id);
            if (errorMessage != null) {
                return BadRequest(errorMessage);
            }

            return Ok();
        }
    }
}
