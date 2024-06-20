using TripPlanner.Server.Models;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Models.DTOs.Incoming;
using TripPlanner.Server.Models.DTOs.Outgoing;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IRegionService
    {
        Task<List<string>> GetAllRegionNamesAsync();
        Task<List<RegionMiniGetDto>> GetAllRegionMinisAsync();
        Task<RegionMiniGetDto?> GetRegionMiniByNameAsync(string regionName);
        Task<RegionGetDto?> GetRegionByNameAsync(string regionName);
        Task<ErrorResponse?> CreateRegionAsync(RegionCreateDto regionCreate);
        Task<ErrorResponse?> UpdateRegionAsync(string regionName, RegionCreateDto regionCreate);
        Task<ErrorResponse?> DeleteRegionAsync(string regionName);
        Task<List<City>> GetCitiesByRegionName(string regionName);
    }

}
