using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IRegionService
    {
        Task<List<string>> GetAllRegionNamesAsync();
        Task<List<RegionMini>> GetAllRegionMinisAsync();
        Task<RegionMini?> GetRegionMiniByNameAsync(string regionName);
        Task<RegionGet?> GetRegionByNameAsync(string regionName);
        Task<ErrorResponse?> CreateRegionAsync(RegionCreate regionCreate);
        Task<ErrorResponse?> UpdateRegionAsync(string regionName, RegionCreate regionCreate);
        Task<ErrorResponse?> DeleteRegionAsync(string regionName);
    }

}
