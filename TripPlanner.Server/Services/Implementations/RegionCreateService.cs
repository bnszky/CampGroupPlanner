using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace TripPlanner.Server.Services.Implementations
{
    public class RegionCreateService : IRegionCreateService
    {
        private readonly HttpClient _httpClient;

        private string ConvertHtmlToPlainText(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            doc.DocumentNode.Descendants()
                .Where(n => n.Name == "script" || n.Name == "style")
                .ToList()
                .ForEach(n => n.Remove());

            string text = doc.DocumentNode.InnerText;

            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s+", " ").Trim();

            return text;
        }

        public RegionCreateService()
        {
            _httpClient = new HttpClient();
        }

        private List<string> ParseCitiesFromJson(string response)
        {
            var json = JObject.Parse(response);
            var cityList = new List<string>();

            var citiesJson = json["geonames"];

            if(citiesJson == null || !citiesJson.HasValues) {
                throw new Exception("Couldn't fetch cities for this region");
            }

            foreach(var city in citiesJson)
            {
                cityList.Add(city["name"].ToString());
            }

            return cityList;
        }

        public async Task<List<string>> FindCitiesByRegion(string regionName, int maxNumber)
        {
            string username = "adam";
            string url = $"http://api.geonames.org/searchJSON?q={Uri.EscapeDataString(regionName)}&maxRows={maxNumber}&username={username}&featureClass=P";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return ParseCitiesFromJson(responseBody);
            }
            else
            {
                throw new Exception("Failed to fetch cities from internet");
            }
        }

        private string ExtractDescriptionFromJSON(string responseBody)
        {
            var json = JObject.Parse(responseBody);
            var pages = json["query"]["pages"];

            if(pages == null || !pages.HasValues)
            {
                throw new Exception("Description has not been found");
            }

            var firstPage = pages.First.First;
            return firstPage?["extract"]?.ToString() ?? throw new Exception("Extract not found in the JSON response.");
        }

        public async Task<string> GetDescriptionForRegion(string regionName, int characterLimit)
        {
            string url = $"https://en.wikipedia.org/w/api.php?action=query&prop=extracts&exintro&titles={Uri.EscapeDataString(regionName)}&format=json";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                string description = ConvertHtmlToPlainText(ExtractDescriptionFromJSON(responseBody));
                return description.Substring(0, characterLimit-3) + "...";
            }
            else
            {
                throw new Exception("Failed to fetch description");
            }
        }

        private List<string> ExtractImageUrls(string html)
        {
            var imageUrls = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            foreach (var node in doc.DocumentNode.SelectNodes("//img[@src]"))
            {
                string src = node.GetAttributeValue("src", null);
                if (!string.IsNullOrEmpty(src))
                {
                    if (src.StartsWith("http") || src.StartsWith("//"))
                    {
                        imageUrls.Add(src.StartsWith("//") ? "http:" + src : src);
                    }
                }
            }

            return imageUrls;
        }
        public async Task<List<string>> GetImagesForRegion(string regionName, int maxNumber)
        {
            var images = new List<string>();
            string url = $"https://www.bing.com/images/search?q={Uri.EscapeDataString(regionName)}_places";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var imageUrls = ExtractImageUrls(responseBody);

                foreach (var imageUrl in imageUrls)
                {
                    if (maxNumber <= 0) break;
                    maxNumber--;
                    images.Add(imageUrl);
                }
            }
            else
            {
                throw new Exception("Failed to fetch images");
            }

            return images;
        }
    }
}
