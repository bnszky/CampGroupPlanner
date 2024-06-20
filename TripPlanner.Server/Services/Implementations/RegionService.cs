using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;
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
        private readonly IMapper _mapper;

        public RegionService(TripDbContext dbContext, ICityService cityService, IImageService imageService, IErrorService errorService, ILogger<RegionService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _cityService = cityService;
            _imageService = imageService;
            _errorService = errorService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RegionGetDto?> GetRegionByNameAsync(string regionName)
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

            return _mapper.Map<RegionGetDto?>(region);
        }

        public async Task<ErrorResponse?> CreateRegionAsync(RegionCreateDto regionCreate)
        {
            if (_dbContext.Regions.Any(r => r.Name.ToLower().Equals(regionCreate.Name.ToLower())))
            {
                var err = _errorService.CreateError("Region exists in the database");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region exists in the database");

                _logger.LogError("Region {RegionName} already exists", regionCreate.Name);
                return err;
            }

            var newRegion = _mapper.Map<Region>(regionCreate);

            var citiesError = await ValidateAndAddCitiesAsync(regionCreate.Cities, newRegion);
            if (citiesError != null)
            {
                return citiesError;
            }

            _dbContext.Regions.Add(newRegion);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<ErrorResponse?> UpdateRegionAsync(string regionName, RegionCreateDto regionCreate)
        {
            var regionEdited = await _dbContext.Regions
                .Include(r => r.Cities)
                .FirstOrDefaultAsync(r => r.Name.ToLower().Equals(regionName.ToLower()));

            if (regionEdited == null)
            {
                var err = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region not found");
                _logger.LogError(ResponseMessages.RegionNotFound + "regionName: {RegionName}", regionName);
                return err;
            }

            _dbContext.Cities.RemoveRange(regionEdited.Cities);

            var region = _mapper.Map(regionCreate, regionEdited);

            var citiesError = await ValidateAndAddCitiesAsync(regionCreate.Cities, region);
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

        private async Task<ErrorResponse?> ValidateAndAddCitiesAsync(List<string> cities, Region region)
        {
            foreach (var cityName in cities)
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

        public async Task<List<RegionMiniGetDto>> GetAllRegionMinisAsync()
        {
            var regions = await _dbContext.Regions
            .Include(r => r.Attractions)
            .ToListAsync();

            return _mapper.Map<IEnumerable<RegionMiniGetDto>>(regions).ToList();
        }

        public async Task<RegionMiniGetDto?> GetRegionMiniByNameAsync(string regionName)
        {
            var region = await _dbContext.Regions
            .Include(r => r.Attractions)
            .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

            if (region == null)
            {
                return null;
            }

            return _mapper.Map<RegionMiniGetDto>(region);
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
