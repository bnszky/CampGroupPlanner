using Azure;
using Azure.AI.TextAnalytics;
using CampGroupPlanner.Models;
using CampGroupPlanner.Services.Abstractions;

namespace CampGroupPlanner.Services.Implementations
{
    public class LocationDetectionService : ILocationDetectionService
    {
        private TextAnalyticsClient _client;
        public LocationDetectionService(IConfiguration configuration) {
            var credentials = new AzureKeyCredential(configuration["AzureKeyCredential"]);
            var endpoint = new Uri(configuration["AzureLanguageEndpoint"]);

            _client = new TextAnalyticsClient(endpoint, credentials);
        }

        public Task AddLocationToArticles()
        {
            throw new NotImplementedException();
        }
        public async Task<List<Localization>> GetLocalizationsFromTextAsync(string text)
        {
            // basic execution
            // only for checking that Azure is well linked
            var response = await _client.RecognizeEntitiesAsync(text);
            return null;
        }
    }
}
