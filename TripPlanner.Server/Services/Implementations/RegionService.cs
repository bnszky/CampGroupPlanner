using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
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

        public RegionService(TripDbContext dbContext, ICityService cityService, IImageService imageService, IErrorService errorService)
        {
            _dbContext = dbContext;
            _cityService = cityService;
            _imageService = imageService;
            _errorService = errorService;
        }

        public async Task<RegionGet?> GetRegionByNameAsync(string regionName)
        {
            var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

            if (region == null)
            {
                return null;
            }

            return new RegionGet
            {
                Name = region.Name,
                Description = region.Description,
                Country = region.Country,
                Cities = region.Cities.Select(c => c.Name).ToList(),
                Images = region.Images.Select(i => i.Link).ToList()
            };
        }

        public async Task<ErrorResponse?> CreateRegionAsync(RegionCreate regionCreate)
        {
            if (_dbContext.Regions.Any(r => r.Name.ToLower().Equals(regionCreate.Name.ToLower())))
            {
                var err = _errorService.CreateError("Region exists in the database");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region exists in the database");
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

            var imagesError = await ValidateAndAddImagesAsync(regionCreate, newRegion);
            if (imagesError != null)
            {
                return imagesError;
            }

            _dbContext.Regions.Add(newRegion);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<ErrorResponse?> UpdateRegionAsync(string regionName, RegionCreate regionCreate)
        {
            var region = await _dbContext.Regions
                .Include(r => r.Cities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Name.ToLower() == regionName.ToLower());

            if (region == null)
            {
                var err = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region not found");
                return err;
            }

            foreach (var image in region.Images)
            {
                await _imageService.DeleteImage(image.Link);
            }

            _dbContext.Cities.RemoveRange(region.Cities);
            _dbContext.ImageURLs.RemoveRange(region.Images);

            region.Name = regionCreate.Name;
            region.Description = regionCreate.Description;
            region.Country = regionCreate.Country;

            var citiesError = await ValidateAndAddCitiesAsync(regionCreate, region);
            if (citiesError != null)
            {
                return citiesError;
            }

            var imagesError = await ValidateAndAddImagesAsync(regionCreate, region);
            if (imagesError != null)
            {
                return imagesError;
            }

            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<ErrorResponse?> DeleteRegionAsync(string regionName)
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
                var err = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(err, "Name", "Region not found");
                return err;
            }

            foreach (var image in region.Images)
            {
                await _imageService.DeleteImage(image.Link);
            }

            _dbContext.Cities.RemoveRange(region.Cities);
            _dbContext.ImageURLs.RemoveRange(region.Images);
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

                city.Region = region;

                _dbContext.Cities.Add(city);
            }

            return null;
        }

        private async Task<ErrorResponse?> ValidateAndAddImagesAsync(RegionCreate regionCreate, Region region)
        {
            if (regionCreate.Images != null && regionCreate.Images.Count >= 10)
            {
                var err = _errorService.CreateError("Number of images has exceeded limit");
                _errorService.AddNewErrorMessageFor(err, "Images", "Number of images has exceeded limit");
                return err;
            }

            foreach (var imageFile in regionCreate.Images)
            {
                try
                {
                    var image = new Image { Link = await _imageService.UploadImage(imageFile), Region = region };
                    _dbContext.ImageURLs.Add(image);
                }
                catch
                {
                    var err = _errorService.CreateError("Couldn't upload image");
                    _errorService.AddNewErrorMessageFor(err, "Images", "Couldn't upload image");
                    return err;
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
            .Include(r => r.Images)
            .Select(r => new RegionMini
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Image = r.Images.FirstOrDefault().Link
            })
            .ToListAsync();
        }

        public async Task<RegionMini?> GetRegionMiniByNameAsync(string regionName)
        {
            var region = await _dbContext.Regions
            .Include(r => r.Images)
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
                Image = region.Images.FirstOrDefault()?.Link
            };
        }
    }

}
