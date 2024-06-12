using Newtonsoft.Json.Linq;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class CityService : ICityService
    {
        private HttpClient _httpClient;
        public CityService() {
            _httpClient = new HttpClient();
        }
        public async Task<City> FetchInformationAboutCityFromName(string cityName)
        {
            string username = "adam";
            string url = $"http://api.geonames.org/searchJSON?q={Uri.EscapeDataString(cityName)}&maxRows=1&username={username}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);

                var city = new City();

                var cityInformation = json["geonames"][0];

                city.Name = cityInformation["name"].ToString();
                city.Latitude = (double)cityInformation["lat"];
                city.Longitude = (double)cityInformation["lng"];
                city.Country = cityInformation["countryName"].ToString();

                return city;
            }
            else
            {
                return null;
            }
        }
    }
}
