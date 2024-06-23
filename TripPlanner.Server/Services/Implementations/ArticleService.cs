using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly TripDbContext _dbContext;
        private readonly IErrorService _errorService;
        private readonly IImageService _imageService;

        public ArticleService(TripDbContext dbContext, IErrorService errorService, IImageService imageService)
        {
            _dbContext = dbContext;
            _errorService = errorService;
            _imageService = imageService;
        }

        public async Task<ErrorResponse?> ValidateAndSetRegionAsync(Article article)
        {
            if (article.RegionName == null) return null;

            var region = await _dbContext.Regions
                .Where(r => r.Name.ToLower().Equals(article.RegionName.ToLower()))
                .FirstOrDefaultAsync();

            if (region == null)
            {
                var errorResponse = _errorService.CreateError(ResponseMessages.RegionNotFound);
                _errorService.AddNewErrorMessageFor(errorResponse, "RegionName", ResponseMessages.RegionNotFound);
                return errorResponse;
            }

            article.Region = region;

            return null;
        }

        public async Task<ErrorResponse?> HandleImageUploadAsync(Article article, IFormFile? image)
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
                    article.ImageURL = imageUrl;
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

        public ErrorResponse? CheckArticleExists(Article article)
        {
            if (_dbContext.Articles.Any(a => a.SourceLink == article.SourceLink))
            {
                var errorResponse = _errorService.CreateError("Article already exists");
                _errorService.AddNewErrorMessageFor(errorResponse, "SourceLink", "An article with this source link already exists in the database");
                return errorResponse;
            }
            return null;
        }

        public async Task<Article> CreateOrUpdateArticleAsync(Article article, bool isAdded = true)
        {
            if (isAdded)
            {
                _dbContext.Articles.Add(article);
            }

            await _dbContext.SaveChangesAsync();
            return article;
        }

        public async Task<Article?> GetAsync(int id)
        {
            return await _dbContext.Articles.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _dbContext.Articles.OrderByDescending(a => a.PositioningRate).ToListAsync();
        }

        public async Task<ErrorResponse?> DeleteAsync(int id)
        {
            var article = await _dbContext.Articles.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            if (article == null)
            {
                var errorResponse = _errorService.CreateError("Couldn't find article with this id");
                _errorService.AddNewErrorMessageFor(errorResponse, "Id", "Couldn't find article with this id");
                return errorResponse;
            }

            try
            {
                if (article.ImageURL != null)
                {
                    await _imageService.DeleteImage(article.ImageURL);
                }
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't delete image");
                _errorService.AddNewErrorMessageFor(errorResponse, "ImageURL", "Couldn't delete image");
                return errorResponse;
            }

            _dbContext.Articles.Remove(article);
            await _dbContext.SaveChangesAsync();

            return null;
        }

        public async Task<List<Article>> GetAllByRegionAsync(string regionName)
        {
            return await _dbContext.Articles
            .Where(a => a.Region.Name.ToLower().Equals(regionName.ToLower())).OrderByDescending(a => a.PositioningRate)
            .ToListAsync();
        }

        public async Task<ErrorResponse?> DeleteBelowRate(int rate)
        {
            try
            {
                var articles = _dbContext.Articles.Where(a => a.PositioningRate < rate).ToList();

                foreach (var article in articles)
                {
                    if (article.ImageURL != null) await _imageService.DeleteImage(article.ImageURL);
                }

                _dbContext.Articles.RemoveRange(articles);
                await _dbContext.SaveChangesAsync();
                return null;
            }
            catch
            {
                var errorResponse = _errorService.CreateError("Couldn't delete articles");
                return errorResponse;
            }
        }
    }

}
