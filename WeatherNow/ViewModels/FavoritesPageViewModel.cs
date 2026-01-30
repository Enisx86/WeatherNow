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

    public ObservableCollection<GeocodingResult> AvailableFavorites = new();

    public bool HasFavorites => AvailableFavorites.Count > 0;

    public FavoritesPageViewModel(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;

        LoadFavoritesCommand = new Command(LoadFavorites);
    }

    private void LoadFavorites()
    {
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
