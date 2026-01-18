using WeatherNow.Models;
namespace WeatherNow.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client = new()
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public Task<Weather> GetWeatherAsync()
    {
        throw new NotImplementedException();
    }
}
