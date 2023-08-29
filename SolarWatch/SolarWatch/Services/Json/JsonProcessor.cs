using SolarWatch.Models;
using SolarWatch.Models.Data;
using System.Text.Json;

namespace SolarWatch.Services.Json
{
    public class JsonProcessor : IJsonProcessor
    {
        public async Task<GeocodingApiResponse> GetGeocodingApiResponseAsync(string data)
        {
            using (JsonDocument json = JsonDocument.Parse(data))
            {
                JsonElement coord = json.RootElement.GetProperty("coord");

                Coordinates coordinates = new Coordinates
                {
                    Lat = coord.GetProperty("lat").GetDouble(),
                    Lon = coord.GetProperty("lon").GetDouble()
                };

                GeocodingApiResponse response = new GeocodingApiResponse()
                {
                    Coord = coordinates
                };

                return response;
            }
        }
        public async Task<SunriseSunsetResults> ProcessAsync(string data, string city, DateTime date)
        {
            using(JsonDocument json = JsonDocument.Parse(data))
            {
                JsonElement res = json.RootElement.GetProperty("results");

                SunriseSunsetResults sunriseSunset = new SunriseSunsetResults()
                {
                    City = city,
                    Date = date,
                    Sunrise = res.GetProperty("sunrise").GetDateTimeOffset().ToLocalTime(),
                    Sunset = res.GetProperty("sunset").GetDateTimeOffset().ToLocalTime()
                };
                return sunriseSunset;
            }

        }
    }
}
