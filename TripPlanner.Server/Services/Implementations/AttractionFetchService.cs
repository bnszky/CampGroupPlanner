using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using TripPlanner.Server.Data;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class AttractionFetchService : IAttractionFetchService
    {
        private readonly ILogger<AttractionFetchService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly TripDbContext _context;
        public AttractionFetchService(ILogger<AttractionFetchService> logger, IConfiguration configuration, HttpClient httpClient, TripDbContext tripDbContext)
        {
            _logger = logger;
            _apiKey = configuration["Foursquare:Key"];
            _httpClient = httpClient;
            _context = tripDbContext;
        }

        private async Task<string> GetResponseTextFromRequest(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            _httpClient.DefaultRequestHeaders.Clear();
            request.Headers.TryAddWithoutValidation("Authorization", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        private async Task AddImageToAttraction(Attraction attraction)
        {
            try
            {
                string requestUri = $"https://api.foursquare.com/v3/places/{attraction.FRS_ID}/photos?limit=1";
                var response = await GetResponseTextFromRequest(requestUri);
                var json = JArray.Parse(response);

                attraction.ImageURL = json[0]["prefix"].ToString() + "original" + json[0]["suffix"].ToString(); 

                _logger.LogInformation("Added ImageUrl to attraction {AttractionName}", attraction.Name);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Couldn't add image to attraction {AttractionName}", attraction.Name);
            }
            
        }
        public async Task<List<Attraction>> FetchAttractionsForGivenCities(List<City> cities, int maxNumberOfAttractions)
        {
            List<Attraction> attractions = new List<Attraction>();
            var seenIds = new HashSet<string>();

            // add attractions without duplicates
            foreach (City city in cities)
            {
                var cityAttractions = await FetchAttractionsForGivenCity(city);
                foreach(var attraction in cityAttractions)
                {
                    if (seenIds.Add(attraction.FRS_ID) && !_context.Attractions.Where(a => a.FRS_ID.Equals(attraction.FRS_ID)).Any())
                    {
                        attractions.Add(attraction);
                    }
                }
            }

            if(maxNumberOfAttractions < attractions.Count) {
                var random = new Random();
                attractions = attractions.OrderBy(x => random.Next()).Take(maxNumberOfAttractions).ToList();
            }

            _logger.LogInformation("Filtered {AttractionsCount} attractions", attractions.Count);

            foreach(var attraction in attractions)
            {
                await AddImageToAttraction(attraction);
            }

            _context.AddRange(attractions);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added {AttractionsCount} articles to the database", attractions.Count);

            return attractions;
        }

        public async Task<List<Attraction>> FetchAttractionsForGivenCity(City city)
        {
            try
            {
                string requestUri = $"https://api.foursquare.com/v3/places/search?ll={city.Latitude.ToString(CultureInfo.InvariantCulture)},{city.Longitude.ToString(CultureInfo.InvariantCulture)}&radius=20000&categories=16000&limit=5";
                var response = await GetResponseTextFromRequest(requestUri);
                var json = JObject.Parse(response);

                var attractions = new List<Attraction>();

                foreach(var att in json["results"])
                {
                    var location = att["geocodes"]["main"];

                    var description = $"{att["location"]["formatted_address"]}, {att["location"]["region"]}, {att["location"]["country"]}";

                    var attraction = new Attraction
                    {
                        Name = att["name"].ToString(),
                        Latitude = (double)location["latitude"],
                        Longitude = (double)location["longitude"],
                        Description = description,
                        RegionId = city.RegionId,
                        RegionName = city.Region.Name,
                        FRS_ID = att["fsq_id"].ToString()
                    };
                    attractions.Add(attraction);

                    _logger.LogInformation("Added attraction {AttractionName} for city {CityName}", attraction.Name, city.Name);
                }

                _logger.LogInformation("Successfully addded {AttractionsCount} attractions for city {CityName}", attractions.Count, city.Name);
                return attractions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't fetch attractions through api for {CityName}", city.Name);
                return new List<Attraction>();
            }
        }
    }
}
