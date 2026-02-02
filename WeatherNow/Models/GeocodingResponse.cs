using System.ComponentModel;

namespace WeatherNow.Models;

public class GeocodingResponse
{
    public GeocodingResult[] results { get; set; }
}

public class GeocodingResult : INotifyPropertyChanged
{
    public int id { get; set; }
    public string name { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double elevation { get; set; }
    public string timezone { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }

    public string CountryEmoji => CountryCodeToFlagEmoji(country_code);
    public Location Coordinates => new(latitude, longitude, elevation);

    private bool _favorite;
    public bool Favorite
    {
        get => _favorite;
        set { _favorite = value; OnPropertyChanged(nameof(Favorite)); }
    }

    // https://stackoverflow.com/questions/47272182/how-to-convert-two-letter-country-codes-to-flag-emojis
    public static string CountryCodeToFlagEmoji(string country)
        => string.Concat(country.ToUpper().Select(code => char.ConvertFromUtf32(code + 0x1F1E6 - 0x41)));

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

