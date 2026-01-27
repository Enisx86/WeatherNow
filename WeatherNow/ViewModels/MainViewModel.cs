using System.ComponentModel;
using System.Text.Json;
using WeatherNow.Models;
using WeatherNow.Services;

namespace WeatherNow.ViewModels;

public class MainViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private readonly IWeatherService _weatherService; // MauiProgram.cs

    private GeocodingResult _city;
    public GeocodingResult City
    {
        get => _city;
        set
        {
            _city = value;
            OnPropertyChanged(nameof(City));

            if (_city != null) // city info loaded, we can now get weather
                _ = LoadWeather();
                
        }
    }

    private WeatherResponse? _weather;
    public WeatherResponse? Weather
    {
        get => _weather;
        set
        {
            _weather = value;
            OnPropertyChanged(nameof(Weather));
        }
    }

    public bool IsValidCity => City != null; // only show data if city is visible
    public bool IsNotValidCity => !IsValidCity; 

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }


    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-10.0
        // QueryProperty doesn't work for some reason, IQueryAttributable is used instead
        // certified .NET MAUI moment :)

        // issue: navigation doesn't update values for some reason, so you have to update it yourself...

        if (query.TryGetValue("City", out object cityModel) && cityModel is GeocodingResult)
        {
            City = (GeocodingResult) cityModel;

            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(IsValidCity));
            OnPropertyChanged(nameof(IsNotValidCity));
        }
    }

    private async Task LoadWeather()
    {
        Weather = await _weatherService.GetWeatherAsync(City);
    }

    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
