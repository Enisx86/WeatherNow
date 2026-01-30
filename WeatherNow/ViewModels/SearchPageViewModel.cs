using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WeatherNow.Models;
using WeatherNow.Services;

namespace WeatherNow.ViewModels;

public class SearchPageViewModel : INotifyPropertyChanged
{
    private readonly IWeatherService _weatherService; // MauiProgram.cs
    private readonly IFavoriteService _favoriteService;

    public ICommand SearchCommand { get; set; }
    public ICommand SelectCityCommand { get; set; } // sends them back to MainPage with updated city info
    public ICommand FavoriteCommand { get; set; }

    public ObservableCollection<CityItem> AvailableCities { get; set; } = new();

    public bool IsNotSearching => !IsSearching;
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

    private string _searchBarText = "";
    public string SearchBarText
    {
        get => _searchBarText;
        set
        {
            if (_searchBarText != value)
            {
                _searchBarText = value;
                OnPropertyChanged(nameof(SearchBarText));
            }
        }
    }

    public SearchPageViewModel(IWeatherService weatherService, IFavoriteService favoriteService)
    {
        _weatherService = weatherService;
        _favoriteService = favoriteService;

        SearchCommand = new Command(SearchCities);
        SelectCityCommand = new Command<CityItem>(SelectCity);
        FavoriteCommand = new Command<CityItem>(SetFavorite);
    }

    private async void SelectCity(CityItem cityItem)
    {
        // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-10.0
        var navigationParameter = new Dictionary<string, object>
        {
            { "City", cityItem.City }
        };

        await Shell.Current.GoToAsync("//MainPage", navigationParameter); // routes are registered in AppShell.xaml
    }

    private async void SearchCities()
    {
        try
        {
            if (IsSearching) return;
            IsSearching = true;

            AvailableCities.Clear();

            GeocodingResult[] cities = await _weatherService.GetGeocodedCitiesAsync(SearchBarText);
            if (cities == null) return;

            List<GeocodingResult> uniqueCities = cities
                .DistinctBy(c => new {c.name, c.country}) // prevent duplicates from the same country
                .ToList();

            await Task.Delay(365); // this API is a little too fast ngl

            foreach (GeocodingResult city in uniqueCities)
            {
                CityItem cityItem = new() { City = city, Favorite = _favoriteService.IsFavorite(city) };
                AvailableCities.Add(cityItem);
            }

            IsSearching = false;
        }
        catch (Exception e)
        {

        }
    }

    private void SetFavorite(CityItem cityItem)
    {
        cityItem.Favorite = !cityItem.Favorite;
        _favoriteService.ToggleFavorite(cityItem.City);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
