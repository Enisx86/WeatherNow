using WeatherNow.Models;
namespace WeatherNow.Services;

public interface IWeatherService
{
    Task<Weather> GetWeatherAsync();
}
