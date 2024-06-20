using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAttractionService
    {
        Task<ErrorResponse?> ValidateAndSetRegionAsync(Attraction attraction);
        Task<ErrorResponse?> HandleImageUploadAsync(Attraction attraction, IFormFile image);
        Task<Attraction> CreateOrUpdateAttractionAsync(Attraction attraction, bool isAdd);
        Task<Attraction?> GetAsync(int id);
        Task<List<Attraction>> GetAllAsync();
        Task<List<Attraction>> GetAllByRegionAsync(string regionName);
        Task<ErrorResponse?> DeleteAsync(int id);
    }

}
