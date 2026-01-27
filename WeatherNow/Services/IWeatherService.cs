using WeatherNow.Models;
namespace WeatherNow.Services;

public interface IWeatherService
{
    Task<GeocodingResult?> GetGeocodedCityAsync(string cityName);
    Task<GeocodingResult[]> GetGeocodedCitiesAsync(string cityName); // 10 results, can feature partial city name like "Zen" for Zenica


    Task<WeatherResponse?> GetWeatherAsync(GeocodingResult city);
    Task<WeatherResponse?> GetWeatherAsync(double latitude, double longitude); // keeping latitude & longitude in case of precise GPS usage??
}
