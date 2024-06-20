using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class AttractionService : IAttractionService
    {
        private readonly TripDbContext _dbContext;
        private readonly IErrorService _errorService;
        private readonly IImageService _imageService;

        public AttractionService(TripDbContext dbContext, IErrorService errorService, IImageService imageService)
        {
            _dbContext = dbContext;
            _errorService = errorService;
            _imageService = imageService;
        }

        public async Task<ErrorResponse?> ValidateAndSetRegionAsync(Attraction attraction)
        {
            var region = await _dbContext.Regions
                .Where(r => r.Name.ToLower().Equals(attraction.RegionName.ToLower()))
                .FirstOrDefaultAsync();

            if (region == null)
            {
                var errorResponse = _errorService.CreateError(ResponseMessages.RegionNotFound);
                _errorService.AddNewErrorMessageFor(errorResponse, "RegionName", ResponseMessages.RegionNotFound);
                return errorResponse;
            }

            attraction.Region = region;
            return null;
        }

        public async Task<ErrorResponse?> HandleImageUploadAsync(Attraction attraction, IFormFile? image)
        {
            if (image != null)
            {
                if (!_imageService.IsJpgOrPng(image))
                {
                    var errorResponse = _errorService.CreateError("Invalid file extension");
                    _errorService.AddNewErrorMessageFor(errorResponse, "Image", "Incorrect file extension. It must be .jpg or .png");
                    return errorResponse;
                }

                try
                {
                    string imageUrl = await _imageService.UploadImage(image);
                    attraction.ImageURL = imageUrl;
                }
                catch
                {
                    var errorResponse = _errorService.CreateError("Image upload failed");
                    _errorService.AddNewErrorMessageFor(errorResponse, "Image", "Couldn't upload image to the database");
                    return errorResponse;
                }
            }

            return null;
        }

        public async Task<Attraction> CreateOrUpdateAttractionAsync(Attraction attraction, bool isAdd)
        {
            if (isAdd)
            {
                _dbContext.Attractions.Add(attraction);
            }

            await _dbContext.SaveChangesAsync();
            return attraction;
        }

        public async Task<Attraction?> GetAsync(int id)
        {
            return await _dbContext.Attractions.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Attraction>> GetAllAsync()
        {
            return await _dbContext.Attractions.ToListAsync();
        }

        public async Task<ErrorResponse?> DeleteAsync(int id)
        {
            var att = await _dbContext.Attractions.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            if(att == null)
            {
                var errorResponse = _errorService.CreateError("Couldn't find attraction with this id");
                _errorService.AddNewErrorMessageFor(errorResponse, "Id", "Couldn't find attraction with this id");
                return errorResponse;
            }

            try
            {
                if(att.ImageURL != null)
                {
                    await _imageService.DeleteImage(att.ImageURL);
                }
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't delete image");
                _errorService.AddNewErrorMessageFor(errorResponse, "ImageURL", "Couldn't delete image");
                return errorResponse;
            }

            _dbContext.Attractions.Remove(att);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<List<Attraction>> GetAllByRegionAsync(string regionName)
        {
            return await _dbContext.Attractions
            .Where(a => a.Region.Name.ToLower().Equals(regionName.ToLower()))
            .ToListAsync();
        }
    }

}
