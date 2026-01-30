namespace WeatherNow.Models;

// because multiple models can utilize it
public static class WeatherExtensions
{
    public static string ToIconSource(this WeatherCode code)
    {
        return code switch
        {
            WeatherCode.ClearSky => "sunny.png",
            WeatherCode.MainlyClear or WeatherCode.PartlyCloudy => "pcloudy.png",
            WeatherCode.Overcast => "mcloudy.png",
            WeatherCode.Fog or WeatherCode.DepositingRimeFog => "foggy.png",
            WeatherCode.DrizzleLight or WeatherCode.DrizzleModerate => "lrain.png",
            WeatherCode.DrizzleDense => "rain.png",
            WeatherCode.RainSlight => "lrain.png",
            WeatherCode.RainModerate => "rain.png",
            WeatherCode.RainHeavy => "hail.png",
            WeatherCode.SnowFallSlight or WeatherCode.SnowFallModerate => "lsnow.png",
            WeatherCode.SnowFallHeavy => "snow_no_day_and_night.png",
            WeatherCode.Thunderstorm => "tstorm.png",
            _ => "Unknown"
        };
    }

    public static string ToDescription(this WeatherCode code)
    {
        return code switch
        {
            WeatherCode.ClearSky => "Clear Sky",
            WeatherCode.MainlyClear => "Mainly Clear",
            WeatherCode.PartlyCloudy => "Partly Cloudy",
            WeatherCode.Overcast => "Overcast",
            WeatherCode.Fog => "Foggy",
            WeatherCode.DepositingRimeFog => "Foggy with Rime",
            WeatherCode.DrizzleLight => "Light Drizzle",
            WeatherCode.DrizzleModerate => "Moderate Drizzle",
            WeatherCode.DrizzleDense => "Dense Drizzle",
            WeatherCode.RainSlight => "Light Rain",
            WeatherCode.RainModerate => "Moderate Rain",
            WeatherCode.RainHeavy => "Heavy Rain",
            WeatherCode.SnowFallSlight => "Light Snow",
            WeatherCode.SnowFallModerate => "Moderate Snow",
            WeatherCode.SnowFallHeavy => "Heavy Snow",
            WeatherCode.Thunderstorm => "Thunderstorm",
            _ => "Unknown"
        };
    }
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

public class WeatherResponse
{
    public float latitude { get; set; }
    public float longitude { get; set; }
    public float generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezone_abbreviation { get; set; }
    public double elevation { get; set; }
    public CurrentUnits current_units { get; set; }
    public Current current { get; set; }
    public HourlyUnits hourly_units { get; set; }
    public Hourly hourly { get; set; }
    public DailyUnits daily_units { get; set; }
    public Daily daily { get; set; }
}

public class CurrentUnits
{
    public string time { get; set; }
    public string interval { get; set; }
    public string temperature_2m { get; set; }
    public string relative_humidity_2m { get; set; }
    public string surface_pressure { get; set; }
    public string wind_speed_10m { get; set; }
    public string weather_code { get; set; }
}

public class Current
{
    public string time { get; set; }
    public int interval { get; set; }
    public float temperature_2m { get; set; }
    public float apparent_temperature { get; set; }
    public int relative_humidity_2m { get; set; }
    public float pressure_msl { get; set; }
    public float wind_speed_10m { get; set; }
    public int weather_code { get; set; }

    public WeatherCode WeatherCode => (WeatherCode) weather_code;
}

public class HourlyUnits
{
    public string time { get; set; }
    public string temperature_2m { get; set; }
    public string wind_speed_10m { get; set; }
    public string temperature_80m { get; set; }
    public string weather_code { get; set; }
}

public class Hourly
{
    public string[] time { get; set; }
    public float[] temperature_2m { get; set; }
    public float[] wind_speed_10m { get; set; }
    public float[] temperature_80m { get; set; }
    public int[] weather_code { get; set; }
    public float[] visibility { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string uv_index_max { get; set; }
    public string sunset { get; set; }
    public string weather_code { get; set; }
    public string temperature_2m_max { get; set; }
    public string temperature_2m_min { get; set; }
}

public class Daily
{
    public string[] time { get; set; }
    public float[] uv_index_max { get; set; }
    public string[] sunset { get; set; }
    public int[] weather_code { get; set; }
    public float[] temperature_2m_max { get; set; }
    public float[] temperature_2m_min { get; set; }
}