using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using TripPlanner.Server.Models.Database;
using TripPlanner.Server.Services.Implementations;

namespace TripPlanner.Services.Tests
{
    public class ArticleKeywordsMatchingServiceTests
    {
        private readonly Mock<ILogger<ArticleKeywordsMatchingSevice>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private ArticleKeywordsMatchingSevice _service;

        public ArticleKeywordsMatchingServiceTests()
        {
            _loggerMock = new Mock<ILogger<ArticleKeywordsMatchingSevice>>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(config => config["ProjectPath"]).Returns("C:\\Users\\micha\\source\\repos\\TripPlanner\\TripPlanner.Server");

            _service = new ArticleKeywordsMatchingSevice(_loggerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task RateArticles_ShouldCalculateCorrectPositioningRate()
        {
            // Arrange
            _service = new ArticleKeywordsMatchingSevice(_loggerMock.Object, _configurationMock.Object);
            var articles = new List<Article>
            {
                new Article { Title = "This is a great adventure" },
                new Article { Title = "Camping and hiking are amazing activities for a holiday." }
            };

            // Act
            await _service.RateArticles(articles);

            // Assert
            Assert.Equal(10, articles[0].PositioningRate); // 1 keyword -> (1/10) -> 10
            Assert.Equal(30, articles[1].PositioningRate); // 3 keywords -> (3/10) -> 30
        }

        [Fact]
        public async Task RateArticles_CalculateCorrectPositioningRateForGivenKeywordsList()
        {
            // Arrange
            List<string> keywordsForTest = ["travel", "water", "beach"];
            _service = new ArticleKeywordsMatchingSevice(_loggerMock.Object, keywordsForTest);
            var articles = new List<Article>
            {
                new Article { Title = "I like drinking WaTeR when I spend my holiday on the beach" },
                new Article { Title = "I bought that travel trip in traVel... AGency because I like travelling with travellers" },
                new Article { Title = """
                water
                AWaTerL
                TrBeacH
                TrABeVel
                """},
                new Article { Title = "travel,beach,travel,beach,travel," +
                "beach,travel,beach,travel,beach,water,water,agency,water,water,water,water,water,water,water,water,water,water"},
                new Article { Title = "No keyword from the list" },
            };

            // Act
            await _service.RateArticles(articles);

            // Assert
            Assert.Equal(20, articles[0].PositioningRate); // 2 K -> 20
            Assert.Equal(40, articles[1].PositioningRate); // 4 K -> 40
            Assert.Equal(30, articles[2].PositioningRate); // 3 K -> 30
            Assert.Equal(100, articles[3].PositioningRate); // >20 K -> 100
            Assert.Equal(0, articles[4].PositioningRate); // 0 K -> 0
        }

        [Fact]
        public async Task AssignArticlesByRegionNames_AssignedArticlesForGivenRegionsList()
        {
            // Arrange
            _service = new ArticleKeywordsMatchingSevice(_loggerMock.Object, _configurationMock.Object);
            var articles = new List<Article>
            {
                new Article { Title = "This is the Catalonia" },
                new Article { Title = "PolandIsBeatifulCountry" },
                new Article { Title = "SpainIsBeatifulCountry" },
                new Article { Title = "Silesia is one of the most developed region in PL" },
                new Article { Title = "I love Morocco" },
            };
            List<string> regionNames = ["Catalonia", "Lombardy", "Silesia"];
            List<string> countries = ["Spain", "Italy", "Poland"];

            // Act
            await _service.AssignArticlesByRegionNames(articles, regionNames, countries);

            // Assert
            Assert.Equal("Catalonia", articles[0].RegionName);
            Assert.Equal("Silesia", articles[1].RegionName);
            Assert.Equal("Catalonia", articles[2].RegionName);
            Assert.Equal("Silesia", articles[3].RegionName);
            Assert.Equal(null, articles[4].RegionName);
        }

    }
}