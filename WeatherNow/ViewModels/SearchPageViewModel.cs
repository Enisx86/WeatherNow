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
    public ICommand RefreshFavoritesCommand { get; set; }

    public ObservableCollection<GeocodingResult> AvailableCities { get; set; } = new();

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
        SelectCityCommand = new Command<GeocodingResult>(SelectCity);
        FavoriteCommand = new Command<GeocodingResult>(SetFavorite);
        RefreshFavoritesCommand = new Command(RefreshFavorites);
    }

    private async void SelectCity(GeocodingResult city)
    {
        // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-10.0
        var navigationParameter = new Dictionary<string, object>
        {
            { "City", city }
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
                city.Favorite = _favoriteService.IsFavorite(city);
                AvailableCities.Add(city);
            }

            IsSearching = false;
        }
        catch (Exception e)
        {

        }
    }

    private void RefreshFavorites()
    {
        List<GeocodingResult> favorites = _favoriteService.GetFavorites();

        foreach (GeocodingResult city in AvailableCities)
        {
            bool isFavorited = favorites.Any(favCity => favCity.id == city.id);

            if (city.Favorite != isFavorited)
            {
                // mismatch. UI state doesn't reflect the global state, refresh!
                city.Favorite = isFavorited;
            }
        }
    }

    private void SetFavorite(GeocodingResult city)
    {
        city.Favorite = !city.Favorite;
        _favoriteService.ToggleFavorite(city);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
