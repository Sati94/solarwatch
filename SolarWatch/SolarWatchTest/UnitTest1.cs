using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarWatch.Controllers;
using SolarWatch.Models.Data;
using SolarWatch.Services;
using System.Net;
using SolarWatch.Services.Json;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using SolarWatch.Models;

namespace SolarWatchTest
{
    [TestFixture]
    public class Tests
    {
        private SunriseController _controller;
        private Mock<ILogger<SunriseController>> _loggerMock;
        private Mock<IWeatherDataProvider> _weatherDataProviderMock;
        private Mock<IJsonProcessor> _jsonProcessorMock;
        private Mock<HttpClient> _httpClientMock;
        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<SunriseController>>();
            _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
            _jsonProcessorMock = new Mock<IJsonProcessor>();
            _httpClientMock = new Mock<HttpClient>();
            _controller = new SunriseController(_loggerMock.Object, _weatherDataProviderMock.Object, _jsonProcessorMock.Object);
            
        }
        [Test]

        public async Task GetSolarInfo_InvalidCity_ReturnsBadRequest()
        {
            // Arrange
            var cityName = "InvalidCity";
            var date = DateTime.Now;

            _weatherDataProviderMock.Setup(w => w.GetLatLonAsync(cityName))
            .ReturnsAsync((string)null); 

            // Act
            var result = await _controller.GetSunriseSunsetAsync(cityName, date);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
        [Test]
        public async Task GetSunriseSunsetAsync_ValidData_ReturnsOkResult()
        {
            // Arrange
            string cityName = "TestCity";
            DateTime date = DateTime.Now;
            var geoData = "sampleGeoData";
            var geoApiResponse = new GeocodingApiResponse { /* ... */ };
            var weatherData = "sampleWeatherData";
            var sunriseSunsetResults = new SunriseSunsetResults { /* ... */ };

            _weatherDataProviderMock.Setup(w => w.GetLatLonAsync(cityName)).ReturnsAsync(geoData);
            _jsonProcessorMock.Setup(j => j.GetGeocodingApiResponseAsync(geoData)).ReturnsAsync(geoApiResponse);
            _weatherDataProviderMock.Setup(w => w.GetSunriseSunsetAsync(It.IsAny<double>(), It.IsAny<double>(), date)).ReturnsAsync(weatherData);
            _jsonProcessorMock.Setup(j => j.ProcessAsync(weatherData, cityName, date)).ReturnsAsync(sunriseSunsetResults);

            // Act
            var result = await _controller.GetSunriseSunsetAsync(cityName, date);

            // Assert
            Assert.IsInstanceOf<ActionResult<SunriseSunsetResults>>(result);
        }

    }
}