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

        try
        {
            List<GeocodingResult>? list = JsonSerializer.Deserialize<List<GeocodingResult>>(json);

            // remove nulls bcuz json is being an ass
            return list?.Where(x => x != null).ToList() ?? new List<GeocodingResult>();
        }
        catch
        {
            return new List<GeocodingResult>();
        }
    }

    public void ToggleFavorite(GeocodingResult city)
    {
        List<GeocodingResult> favorites = GetFavorites();
        GeocodingResult? favorite = favorites.FirstOrDefault(c => c.id == city.id);

        if (favorite != null)
        {
            city.Favorite = false;
            favorites.Remove(favorite);
        }
        else
        {
            city.Favorite = true;
            favorites.Add(city);
        }

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
