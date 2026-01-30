using WeatherNow.Models;

namespace WeatherNow.Services;

public interface IFavoriteService
{
    List<GeocodingResult> GetFavorites();
    void ToggleFavorite(GeocodingResult city);
    bool IsFavorite(GeocodingResult city);
}
