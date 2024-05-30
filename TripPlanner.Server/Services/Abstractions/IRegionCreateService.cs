using TripPlanner.Server.Models;

namespace TripPlanner.Server.Services.Abstractions
{
    public interface IRegionCreateService
    {
        public Task<List<string>> FindCitiesByRegion(string regionName, int maxNumber);
        public Task<string> GetDescriptionForRegion(string regionName, int characterLimit);
        public Task<List<string>> GetImagesForRegion(string regionName, int maxNumber);
    }
}
