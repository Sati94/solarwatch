using System.Net;

namespace SolarWatch.Services
{
    public class WeatherProvider : IWeatherDataProvider
    {
        private readonly ILogger<WeatherProvider> _logger;
        private readonly string apiKey = "805a90c3b25da32da994eddbc4fb096c";
        public WeatherProvider(ILogger<WeatherProvider> logger)
        {
            _logger = logger;
        }
        public async  Task<string> GetLatLonAsync(string city)
        {

            var geoUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";
            using var client = new HttpClient();

            _logger.LogInformation("Calling OpenWeather API with url: {url}", geoUrl);
            var response = await client.GetAsync(geoUrl);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> GetSunriseSunsetAsync(double lat, double lon, DateTime date)
        {
            var sunriseSunsetUrl = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date:yyyy-MM-dd}&formatted=0";
            using var client = new HttpClient();

            _logger.LogInformation("Calling OpenWeather API with url: {url}", sunriseSunsetUrl);
            var response = await client.GetAsync(sunriseSunsetUrl);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
