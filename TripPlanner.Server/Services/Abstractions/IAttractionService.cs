using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IAttractionService
    {
        Task<ErrorResponse?> ValidateAndSetRegionAsync(AttractionCreate attractionCreate, Attraction attraction);
        Task<ErrorResponse?> HandleImageUploadAsync(AttractionCreate attractionCreate, Attraction attraction);
        Task<Attraction> CreateOrUpdateAttractionAsync(AttractionCreate attractionCreate, Region region, string? ImageURL = null, Attraction existingAttraction = null);
        Task<Attraction?> GetAsync(int id);
        Task<List<Attraction>> GetAllAsync();
        Task<List<Attraction>> GetAllByRegionAsync(string regionName);
        Task<ErrorResponse?> DeleteAsync(int id);
    }

}
