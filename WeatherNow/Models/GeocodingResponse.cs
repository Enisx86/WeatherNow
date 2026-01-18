namespace WeatherNow.Models;

public class GeocodingResponse
{
    public GeocodingResult[] results { get; set; }
}

public class GeocodingResult
{
    public int id { get; set; }
    public string name { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double elevation { get; set; }
    public string timezone { get; set; }
    public string country { get; set; }

    public Location Coordinates => new(latitude, longitude, elevation);
}

