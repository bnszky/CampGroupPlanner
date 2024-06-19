using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class RegionService : IRegionService
    {
        private readonly TripDbContext _dbContext;
        private readonly ICityService _cityService;
        private readonly IImageService _imageService;
        private readonly IErrorService _errorService;
        private readonly ILogger<RegionService> _logger;

        public RegionService(TripDbContext dbContext, ICityService cityService, IImageService imageService, IErrorService errorService, ILogger<RegionService> logger)
        {
            _dbContext = dbContext;
            _cityService = cityService;
            _imageService = imageService;
            _errorService = errorService;
            _logger = logger;
        }

        public async Task<RegionGet?> GetRegionByNameAsync(string regionName)
        {
            var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Attractions)
                .FirstOrDefaultAsync(r => r.Name.ToLower().Equals(regionName.ToLower()));

            if (region == null)
            {
                _logger.LogError("Region {RegionName} doesn't exist", regionName);
                return null;
            }

            return new RegionGet
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                Country = region.Country,
                Cities = region.Cities.Select(c => c.Name).ToList(),
                Images = region.Attractions.Where(a => a.ImageURL != null).Select(a => a.ImageURL).ToList()
            };
        }

        public async Task<ErrorResponse?> CreateRegionAsync(RegionCreate regionCreate)
        {
            if (_dbContext.Regions.Any(r => r.Name.ToLower().Equals(regionCreate.Name.ToLower())))
            {
                var err = _errorService.CreateError("Region exists in the database");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region exists in the database");

                _logger.LogError("Region {RegionName} already exists", regionCreate.Name);
                return err;
            }

            var newRegion = new Region
            {
                Name = regionCreate.Name,
                Description = regionCreate.Description,
                Country = regionCreate.Country
            };

            var citiesError = await ValidateAndAddCitiesAsync(regionCreate, newRegion);
            if (citiesError != null)
            {
                return citiesError;
            }

            _dbContext.Regions.Add(newRegion);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<ErrorResponse?> UpdateRegionAsync(string regionName, RegionCreate regionCreate)
        {
            var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .FirstOrDefaultAsync(r => r.Name.ToLower().Equals(regionName.ToLower()));

            if (region == null)
            {
                var err = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region not found");
                _logger.LogError(ResponseMessages.RegionNotFound + "regionName: {RegionName}", regionName);
                return err;
            }

            _dbContext.Cities.RemoveRange(region.Cities);

            region.Name = regionCreate.Name;
            region.Description = regionCreate.Description;
            region.Country = regionCreate.Country;

            var citiesError = await ValidateAndAddCitiesAsync(regionCreate, region);
            if (citiesError != null)
            {
                return citiesError;
            }

            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<ErrorResponse?> DeleteRegionAsync(string regionName)
        {
            var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Attractions)
                .Include(r => r.Reviews)
                .Include(r => r.Articles)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

            if (region == null)
            {
                var err = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region not found");
                _logger.LogError(ResponseMessages.RegionNotFound + "regionName: {RegionName}", regionName);
                return err;
            }

            foreach (var attraction in region.Attractions)
            {
                if(attraction.ImageURL != null) await _imageService.DeleteImage(attraction.ImageURL);
            }

            _dbContext.Cities.RemoveRange(region.Cities);
            _dbContext.Attractions.RemoveRange(region.Attractions);
            _dbContext.Reviews.RemoveRange(region.Reviews);

            foreach (var article in region.Articles)
            {
                article.Region = null;
                article.RegionId = null;
            }

            _dbContext.Remove(region);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        private async Task<ErrorResponse?> ValidateAndAddCitiesAsync(RegionCreate regionCreate, Region region)
        {
            foreach (var cityName in regionCreate.Cities)
            {
                var city = await _cityService.FetchInformationAboutCityFromName(cityName);

                if (city != null)
                {
                    city.Region = region;

                    _dbContext.Cities.Add(city);
                }
            }

            return null;
        }

        public async Task<List<string>> GetAllRegionNamesAsync()
        {
            return await _dbContext.Regions.Select(r => r.Name).ToListAsync();
        }

        public async Task<List<RegionMini>> GetAllRegionMinisAsync()
        {
            return await _dbContext.Regions
            .Include(r => r.Attractions)
            .Select(r => new RegionMini
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Image = r.Attractions.Where(a => a.ImageURL != null).Select(a => a.ImageURL).FirstOrDefault()
            })
            .ToListAsync();
        }

        public async Task<RegionMini?> GetRegionMiniByNameAsync(string regionName)
        {
            var region = await _dbContext.Regions
            .Include(r => r.Attractions)
            .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

            if (region == null)
            {
                return null;
            }

            return new RegionMini
            {
                Id = region.Id,
                Name = region.Name,
                Description = region.Description,
                Image = region.Attractions.Where(a => a.ImageURL != null).Select(a => a.ImageURL).FirstOrDefault()
            };
        }

        public async Task<List<City>> GetCitiesByRegionName(string regionName)
        {
            var region = await _dbContext.Regions.Where(r => r.Name.ToLower().Equals(regionName.ToLower())).Include(r => r.Cities).FirstOrDefaultAsync();
            if (region == null)
            {
                _logger.LogError(ResponseMessages.RegionNotFound + "regionName: {RegionName}", regionName);
                throw new Exception("Region with this name not found");
            }

            return region.Cities.ToList();
        }
    }

}
