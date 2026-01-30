using System.ComponentModel;

namespace WeatherNow.Models;

// primarily for UI presentation btw

public record CityItem : INotifyPropertyChanged
{
    public GeocodingResult City { get; set; }

    private bool _favorite;
    public bool Favorite
    {
        get => _favorite;
        set { _favorite = value; OnPropertyChanged(nameof(Favorite)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}