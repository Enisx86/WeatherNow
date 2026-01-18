using System.Net.Http.Json;
using WeatherNow.Models;
namespace WeatherNow.Services;

// Weather API: https://open-meteo.com/en/docs
// Geocoding API: https://open-meteo.com/en/docs/geocoding-api

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public async Task<GeocodingResult?> GetGeocodedCityAsync(string cityName)
    {
        string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=1&language=en&format=json";

        try
        {
            GeocodingResponse? response = await _client.GetFromJsonAsync<GeocodingResponse>(url);

            if (response?.results == null) return null; // city name couln't be found ;(

            return response.results[0]; // return first city
        } 
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<Weather?> GetWeatherAsync(GeocodingResult city)
    {
        Weather? weather = await GetWeatherAsync(city.Coordinates.Latitude, city.Coordinates.Longitude);

        return weather;
    }

    public async Task<Weather?> GetWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current=temperature_2m,weather_code,wind_speed_10m&timezone=auto&forecast_days=1";

        try
        {
            WeatherResponse? response = await _client.GetFromJsonAsync<WeatherResponse>(url);
            return response?.current; // current weather
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
