using System.Text.Json;
using WeatherNow.Models;

namespace WeatherNow.Services;

public class FavoriteService : IFavoriteService
{
    public List<GeocodingResult> GetFavorites()
    {
        string json = Preferences.Default.Get("favorites", string.Empty);

        if (string.IsNullOrEmpty(json)) 
            return new List<GeocodingResult>();

        return JsonSerializer.Deserialize<List<GeocodingResult>>(json) ?? new List<GeocodingResult>();
    }

    public void ToggleFavorite(GeocodingResult city)
    {
        List<GeocodingResult> favorites = GetFavorites();
        GeocodingResult? favorite = favorites.FirstOrDefault(c => c.id == city.id);

        if (favorite != null)
            favorites.Remove(favorite);
        else
            favorites.Add(city);

        string json = JsonSerializer.Serialize(favorites);
        Preferences.Default.Set("favorites", json);
    }

    public bool IsFavorite(GeocodingResult city)
    {
        List<GeocodingResult> favorites = GetFavorites();
        GeocodingResult? favorite = favorites.FirstOrDefault(c => c.id == city.id);

        return favorite != null; // true if favorited, false if not
    }

}
