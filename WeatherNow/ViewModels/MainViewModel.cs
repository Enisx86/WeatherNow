using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

using WeatherNow.Services;
using WeatherNow.Models;

namespace WeatherNow.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IWeatherService _weatherService; // MauiProgram.cs

    public ICommand SearchCommand { get; set; }

    private string _searchCityText = "";
    public string SearchCityText
    {
        get => _searchCityText;
        set
        {
            if (_searchCityText != value)
            {
                _searchCityText = value;
                OnPropertyChanged(nameof(SearchCityText));
            }
        }
    }

    private string _searchResultText = "";
    public string SearchResultText
    {
        get => _searchResultText;
        set
        {
            if (_searchResultText != value)
            {
                _searchResultText = value;
                OnPropertyChanged(nameof(SearchResultText));
            }
        }
    }

    private bool _isSearching = false;
    public bool IsSearching
    {
        get => _isSearching;
        set
        {
            if (_isSearching != value)
            {
                _isSearching = value;
                OnPropertyChanged(nameof(IsSearching));
            }
        }
    }



    public MainViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;

        SearchCommand = new Command(Search);

    }

    private async void Search()
    {
        try
        {
            if (IsSearching) return;
            IsSearching = true;

            SearchResultText = "";

            GeocodingResult? city = await _weatherService.GetGeocodedCityAsync(SearchCityText);
            if (city == null)
            {
                SearchResultText = "No location found!";
                IsSearching = false;
                return;
            }

            await Task.Delay(500); // this API is a little too fast ngl

            SearchResultText = $"Lat: {city.latitude} Lon: {city.longitude}";
            IsSearching = false;
        } 
        catch (Exception e)
        {
            Debug.Fail($"Failed to search city: {e.Message}");
            return;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
