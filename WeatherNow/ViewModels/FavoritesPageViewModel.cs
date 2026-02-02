using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WeatherNow.Models;
using WeatherNow.Services;

namespace WeatherNow.ViewModels;

public class FavoritesPageViewModel : INotifyPropertyChanged
{
    private IFavoriteService _favoriteService;

    public ICommand LoadFavoritesCommand { get; set; }
    public ICommand RemoveFavoriteCommand { get; set; }
    public ICommand SelectCityCommand { get; set; }

    public ObservableCollection<GeocodingResult> AvailableFavorites { get; set; } = new();

    public bool HasFavorites => AvailableFavorites.Count > 0;

    public FavoritesPageViewModel(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;

        LoadFavoritesCommand = new Command(LoadFavorites);
        SelectCityCommand = new Command<GeocodingResult>(SelectCity);
        RemoveFavoriteCommand = new Command<GeocodingResult>(RemoveFavorite);
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

    private async void RemoveFavorite(GeocodingResult city)
    {
        _favoriteService.ToggleFavorite(city);
        AvailableFavorites.Remove(city);
    }

    private void LoadFavorites()
    {
        AvailableFavorites.Clear();

        List<GeocodingResult> favorites = _favoriteService.GetFavorites();

        foreach (GeocodingResult city in favorites)
        {
            AvailableFavorites.Add(city);
        }

        OnPropertyChanged(nameof(HasFavorites));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
