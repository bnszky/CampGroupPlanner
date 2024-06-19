﻿using Microsoft.EntityFrameworkCore;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models;
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

        public async Task<ErrorResponse?> ValidateAndSetRegionAsync(ArticleCreate articleCreate, Article article)
        {
            if (articleCreate.RegionName == null) return null;

            var region = await _dbContext.Regions
                .Where(r => r.Name.ToLower().Equals(articleCreate.RegionName.ToLower()))
                .FirstOrDefaultAsync();

            if (region == null)
            {
                var errorResponse = _errorService.CreateError("Region not found");
                _errorService.AddNewErrorMessageFor(errorResponse, "RegionName", "Couldn't find region with this name");
                return errorResponse;
            }

            article.Region = region;

            string regionName = articleCreate.RegionName.ToUpper();
            regionName = regionName[0] + regionName.Substring(1).ToLower();

            article.RegionName = regionName;
            return null;
        }

        public async Task<ErrorResponse?> HandleImageUploadAsync(ArticleCreate articleCreate, Article article)
        {
            if (articleCreate.ImageFile != null)
            {
                if (!_imageService.IsJpgOrPng(articleCreate.ImageFile))
                {
                    var errorResponse = _errorService.CreateError("Invalid file extension");
                    _errorService.AddNewErrorMessageFor(errorResponse, "Image", "Incorrect file extension. It must be .jpg or .png");
                    return errorResponse;
                }

                try
                {
                    string imageUrl = await _imageService.UploadImage(articleCreate.ImageFile);
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

        public ErrorResponse? CheckArticleExists(ArticleCreate articleCreate)
        {
            if (_dbContext.Articles.Any(a => a.SourceLink == articleCreate.SourceLink))
            {
                var errorResponse = _errorService.CreateError("Article already exists");
                _errorService.AddNewErrorMessageFor(errorResponse, "SourceLink", "An article with this source link already exists in the database");
                return errorResponse;
            }
            return null;
        }

        public int GeneratePositioningRate(Article article)
        {
            Random random = new Random();
            return random.Next(0, 101);
        }
        public async Task<Article> CreateOrUpdateArticleAsync(ArticleCreate articleCreate, Article article, bool isAdded = true)
        {
            article.Title = articleCreate.Title;
            article.Description = articleCreate.Description;
            if (article.CreatedAt == null) { article.CreatedAt = DateTime.Now; }
            article.EditedAt = DateTime.Now;
            if (isAdded) { article.SourceLink = articleCreate.SourceLink; }
            if(articleCreate.PositioningRate == null)
            {
                article.PositioningRate = GeneratePositioningRate(article);
            }
            article.IsVisible = articleCreate.IsVisible;

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
            return await _dbContext.Articles.ToListAsync();
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
            .Where(a => a.Region.Name.ToLower().Equals(regionName.ToLower()))
            .ToListAsync();
        }
    }

}