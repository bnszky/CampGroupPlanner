using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleService
    {
        Task<ErrorResponse?> ValidateAndSetRegionAsync(Article article);
        Task<ErrorResponse?> HandleImageUploadAsync(Article article, IFormFile? image);
        ErrorResponse? CheckArticleExists(Article article);
        Task<Article> CreateOrUpdateArticleAsync(Article article, bool isAdded);
        Task<Article?> GetAsync(int id);
        Task<List<Article>> GetAllAsync();
        Task<List<Article>> GetAllByRegionAsync(string regionName);
        Task<ErrorResponse?> DeleteAsync(int id);
    }
}
