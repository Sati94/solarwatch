using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models.Data;
using SolarWatch.Models;
using SolarWatch.Services;
using SolarWatch.Services.Json;

namespace SolarWatch.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SunriseController : Controller
    {
        private readonly ILogger<SunriseController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IWeatherDataProvider _weatherDataProvider;
        private readonly IJsonProcessor _jsonProcessor;
        public SunriseController(ILogger<SunriseController> logger, IWeatherDataProvider weatherDataProvider,
           IJsonProcessor jsonProcessor)
        {
            _logger = logger;
            _weatherDataProvider = weatherDataProvider;
            _jsonProcessor = jsonProcessor;
            _httpClient = new HttpClient();
        }
        [HttpGet]
        [Route("api/solar")]
        public async Task<ActionResult<SunriseSunsetResults>> GetSunriseSunsetAsync(string city, DateTime date)
        {
            try
            {
                var GeoData = await _weatherDataProvider.GetLatLonAsync(city);
                var GeoResult = await _jsonProcessor.GetGeocodingApiResponseAsync(GeoData);

                var lat = GeoResult.Coord.Lat;
                var lon = GeoResult.Coord.Lon;

                var weatherData = await _weatherDataProvider.GetSunriseSunsetAsync(lat, lon, date);

                return Ok(_jsonProcessor.ProcessAsync(weatherData, city, date));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting weather data");
                return NotFound("Error getting weather data");
            }
        }

    }
}
