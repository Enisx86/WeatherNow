namespace WeatherNow.Models;

public class WeatherResponse
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public Weather current { get; set; }
}

public class Weather
{
    public string time { get; set; }
    public int interval { get; set; }
    public double temperature_2m { get; set; }
    public WeatherCode weather_code { get; set; } // please use weatherType instead

    public string WeatherType => weather_code switch
    {
        WeatherCode.ClearSky => "Clear Sky",
        WeatherCode.MainlyClear or WeatherCode.PartlyCloudy or WeatherCode.Overcast => "Cloudy",
        WeatherCode.Fog or WeatherCode.DepositingRimeFog => "Foggy",
        WeatherCode.DrizzleLight or WeatherCode.DrizzleModerate or WeatherCode.DrizzleDense => "Drizzle",
        WeatherCode.RainSlight or WeatherCode.RainModerate or WeatherCode.RainHeavy => "Rainy",
        WeatherCode.SnowFallSlight or WeatherCode.SnowFallModerate or WeatherCode.SnowFallHeavy => "Snow",
        WeatherCode.Thunderstorm => "Thunderstorm",
        _ => "Unknown"
    };
}

public enum WeatherCode
{
    ClearSky = 0,
    MainlyClear = 1,
    PartlyCloudy = 2,
    Overcast = 3,
    Fog = 45,
    DepositingRimeFog = 48,
    DrizzleLight = 51,
    DrizzleModerate = 53,
    DrizzleDense = 55,
    RainSlight = 61,
    RainModerate = 63,
    RainHeavy = 65,
    SnowFallSlight = 71,
    SnowFallModerate = 73,
    SnowFallHeavy = 75,
    Thunderstorm = 95,
    Unknown = 999
}
