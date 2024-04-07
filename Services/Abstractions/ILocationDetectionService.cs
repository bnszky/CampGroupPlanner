using CampGroupPlanner.Models;

namespace CampGroupPlanner.Services.Abstractions
{
    public interface ILocationDetectionService
    {
        public Task<List<Localization>> GetLocalizationsFromTextAsync(string text);
        public Task AddLocationToArticles();
    }
}
