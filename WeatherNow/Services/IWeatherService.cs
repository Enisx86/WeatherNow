using WeatherNow.Models;
namespace WeatherNow.Services;

public interface IWeatherService
{
    Task<GeocodingResult?> GetGeocodedCityAsync(string cityName);

    Task<Weather?> GetWeatherAsync(GeocodingResult city);
    Task<Weather?> GetWeatherAsync(double latitude, double longitude); // keeping latitude & longitude in case of precise GPS usage??
}
