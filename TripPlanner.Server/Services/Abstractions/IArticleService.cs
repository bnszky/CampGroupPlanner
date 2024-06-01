using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IArticleService
    {
        Task<ErrorResponse?> ValidateAndSetRegionAsync(ArticleCreate articleCreate, Article article);
        Task<ErrorResponse?> HandleImageUploadAsync(ArticleCreate articleCreate, Article article);
        ErrorResponse? CheckArticleExists(ArticleCreate articleCreate);
        Task<Article> CreateOrUpdateArticleAsync(ArticleCreate articleCreate, Article existingArticle, bool isAdded);
        Task<Article?> GetAsync(int id);
        Task<List<Article>> GetAllAsync();
        Task<List<Article>> GetAllByRegionAsync(string regionName);
        Task<ErrorResponse?> DeleteAsync(int id);
    }
}
