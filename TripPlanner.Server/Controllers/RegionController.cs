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
        private IRegionCreateService _regionCreateService;
        private ICitiesService _cityService;
        private IImageService _imageService;
        public RegionController(TripDbContext dbContext, IRegionCreateService regionCreateService, ICitiesService citiesService, IImageService imageService) { 
            _regionCreateService = regionCreateService;
            _cityService = citiesService;
            _dbContext = dbContext;
            _imageService = imageService;
        }

        [HttpGet("description/{region}")]
        public async Task<ActionResult<string>> FetchDescription(string region)
        {
            return await _regionCreateService.GetDescriptionForRegion(region, 1000);
        }

        [HttpGet("cities/{region}")]
        public async Task<ActionResult<List<string>>> FetchCities(string region)
        {
            return await _regionCreateService.FindCitiesByRegion(region, 5);
        }

        [HttpGet("images/{region}")]
        public async Task<ActionResult<List<string>>> FetchImages(string region)
        {
            return await _regionCreateService.GetImagesForRegion(region, 10);
        }

        [HttpGet("names")]
        public async Task<ActionResult<List<string>>> GetAllNames()
        {
            try
            {
                return await _dbContext.Regions.Select(r => r.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<RegionMini>>> GetAllMini()
        {
            try
            {
                var regions = await _dbContext.Regions
                    .Include(r => r.Images)
                    .Select(r => new RegionMini
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        Image = r.Images.FirstOrDefault().Link
                    })
                    .ToListAsync();

                return regions;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{regionName}/mini")]
        public async Task<ActionResult<RegionMini>> GetMini(string regionName)
        {
            try
            {
                var region = await _dbContext.Regions
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

                if (region == null)
                {
                    return NotFound();
                }

                var regionMini = new RegionMini
                {
                    Id = region.Id,
                    Name = region.Name,
                    Description = region.Description,
                    Image = region.Images.FirstOrDefault()?.Link
                };

                return regionMini;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{regionName}")]
        public async Task<ActionResult<RegionGet>> Get(string regionName)
        {
            try
            {
                var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

                if (region == null)
                {
                    return NotFound();
                }

                var regionGet = new RegionGet
                {
                    Name = region.Name,
                    Description = region.Description,
                    Country = region.Country,
                    Cities = region.Cities.Select(c => c.Name).ToList(),
                    Images = region.Images.Select(i => i.Link).ToList()
                };

                return regionGet;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RegionCreate regionFromBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (_dbContext.Regions.Any(r => r.Name.ToLower().Equals(regionFromBody.Name.ToLower())))
                {
                    return BadRequest("Region exist in the database");
                }

                var newRegion = new Region
                {
                    Name = regionFromBody.Name,
                    Description = regionFromBody.Description,
                    Country = regionFromBody.Country
                };

                // Validate cities
                if (regionFromBody.Cities == null || !regionFromBody.Cities.Any())
                {
                    ModelState.AddModelError(nameof(regionFromBody.Cities), "You must add some cities");
                    return BadRequest(ModelState);
                }

                // Validate images
                if (regionFromBody.Images == null || !regionFromBody.Images.Any())
                {
                    ModelState.AddModelError(nameof(regionFromBody.Images), "You must add some images");
                    return BadRequest(ModelState);
                }

                // Validate number of images
                if (regionFromBody.Images != null && regionFromBody.Images.Count() >= 10)
                {
                    ModelState.AddModelError(nameof(regionFromBody.Images), "Number of images has exceeded limit");
                    return BadRequest(ModelState);
                }

                foreach (var cityName in regionFromBody.Cities)
                {
                    City city = await _cityService.FetchInformationAboutCityFromName(cityName);
                    city.Region = newRegion;

                    if (city.Country == regionFromBody.Country &&
                        !_dbContext.Cities.Any(c => c.Name.Equals(cityName)))
                    {
                        _dbContext.Cities.Add(city);
                    }
                }

                foreach (var imageFile in regionFromBody.Images)
                {
                    try
                    {
                        var image = new Image { Link = await _imageService.UploadImage(imageFile), Region = newRegion };
                        _dbContext.ImageURLs.Add(image);
                    }
                    catch
                    {
                        ModelState.AddModelError(nameof(regionFromBody.Images), "Couldn't load image");
                        return BadRequest(ModelState);
                    }
                }

                _dbContext.Regions.Add(newRegion);
                await _dbContext.SaveChangesAsync();
                
                return Ok();
            }
            catch
            {
                return BadRequest("Unknown error");
            }
        }

        [HttpPut("{regionName}")]
        public async Task<IActionResult> Edit(string regionName, [FromForm] RegionCreate regionFromBody)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validate cities
                if (regionFromBody.Cities == null || !regionFromBody.Cities.Any())
                {
                    ModelState.AddModelError(nameof(regionFromBody.Cities), "You must add some cities");
                    return BadRequest(ModelState);
                }

                // Validate images
                if (regionFromBody.Images == null || !regionFromBody.Images.Any())
                {
                    ModelState.AddModelError(nameof(regionFromBody.Images), "You must add some images");
                    return BadRequest(ModelState);
                }

                // Validate number of images
                if (regionFromBody.Images != null && regionFromBody.Images.Count() >= 10)
                {
                    ModelState.AddModelError(nameof(regionFromBody.Images), "Number of images has exceeded limit");
                    return BadRequest(ModelState);
                }

                var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

                if (region == null)
                {
                    return NotFound();
                }

                foreach (var image in region.Images)
                {
                    await _imageService.DeleteImage(image.Link);
                }

                _dbContext.Cities.RemoveRange(region.Cities);
                _dbContext.ImageURLs.RemoveRange(region.Images);

                region.Name = regionFromBody.Name;
                region.Description = regionFromBody.Description;
                region.Country = regionFromBody.Country;

                foreach (var cityName in regionFromBody.Cities)
                {
                    City city = await _cityService.FetchInformationAboutCityFromName(cityName);
                    city.Region = region;

                    if (city.Country == regionFromBody.Country &&
                        !_dbContext.Cities.Any(c => c.Name.Equals(cityName)))
                    {
                        _dbContext.Cities.Add(city);
                    }
                }

                foreach (var imageFile in regionFromBody.Images)
                {
                    try
                    {
                        var image = new Image { Link = await _imageService.UploadImage(imageFile), Region = region };
                        _dbContext.ImageURLs.Add(image);
                    }
                    catch
                    {
                        ModelState.AddModelError(nameof(regionFromBody.Images), "Couldn't load image");
                        return BadRequest(ModelState);
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{regionName}")]
        public async Task<IActionResult> Delete(string regionName)
        {
            try
            {
                var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Images)
                .Include(r => r.Attractions)
                .Include(r => r.Reviews)
                .Include(r => r.Articles)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

                if (region == null)
                {
                    return NotFound();
                }

                foreach (var image in region.Images)
                {
                    await _imageService.DeleteImage(image.Link);
                }

                _dbContext.Cities.RemoveRange(region.Cities);
                _dbContext.ImageURLs.RemoveRange(region.Images);
                _dbContext.Attractions.RemoveRange(region.Attractions);
                _dbContext.Reviews.RemoveRange(region.Reviews);
                
                foreach(var article in region.Articles)
                {
                    article.Region = null;
                    article.RegionId = null;
                }

                _dbContext.Remove(region);

                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
