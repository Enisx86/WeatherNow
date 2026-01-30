using System.Net.Http.Json;
using System.Text.Json;
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

    public async Task<GeocodingResult[]> GetGeocodedCitiesAsync(string cityName)
    {
        string url = $"https://geocoding-api.open-meteo.com/v1/search?name={cityName}&count=10&language=en&format=json";

        try
        {
            GeocodingResponse? response = await _client.GetFromJsonAsync<GeocodingResponse>(url);

            if (response?.results == null) return null; // city name couln't be found ;(

            return response.results;
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error", $"Geocoding API failure. Message: {e.Message}", "OK");
            return null;
        }
    }

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
            await Shell.Current.DisplayAlert("Error", $"Geocoding API failure. Message: {e.Message}", "OK");
            return null;
        }
    }

    public async Task<WeatherResponse?> GetWeatherAsync(GeocodingResult city)
        => await GetWeatherAsync(city.latitude, city.longitude);

    public async Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=uv_index_max,sunset,weather_code,temperature_2m_max,temperature_2m_min&hourly=temperature_2m,wind_speed_10m,temperature_80m,visibility,weather_code&current=temperature_2m,apparent_temperature,relative_humidity_2m,surface_pressure,pressure_msl,wind_speed_10m,weather_code&timezone=auto&forecast_days=3&forecast_hours=24";

        try
        {
            WeatherResponse? response = await _client.GetFromJsonAsync<WeatherResponse>(url);

            return response;
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error", $"Weather API failure. Message: {e.Message}", "OK");
            return null;
        }
    }
}
