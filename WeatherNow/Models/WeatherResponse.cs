namespace WeatherNow.Models;

// because multiple models can utilize it
public static class WeatherExtensions
{    
    public static string ToDescription(this WeatherCode code)
    {
        return code switch
        {
            WeatherCode.ClearSky => "Clear",
            WeatherCode.MainlyClear => "Mainly Clear",
            WeatherCode.PartlyCloudy => "Partly Cloudy",
            WeatherCode.Overcast => "Overcast",
            WeatherCode.Fog or WeatherCode.DepositingRimeFog => "Foggy",
            WeatherCode.DrizzleLight or WeatherCode.DrizzleModerate or WeatherCode.DrizzleDense => "Drizzling",
            WeatherCode.RainSlight or WeatherCode.RainModerate or WeatherCode.RainHeavy => "Raining",
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
    public float surface_pressure { get; set; }
    public float pressure_msl { get; set; }
    public float wind_speed_10m { get; set; }
    public int weather_code { get; set; }

    private WeatherCode _weatherDescription => (WeatherCode) weather_code;
    public string WeatherDescription => _weatherDescription.ToDescription();

    public string TemperatureText => $"{temperature_2m:0}°";
    public string ApparentTemperatureText => $"Feels like {apparent_temperature:0}°";

    public double HumidityPercent => relative_humidity_2m / 100.0; // ex. 75 -> 0.75
    public double WindSpeedPercent => Math.Clamp(wind_speed_10m / 100.0, 0, 1.0);
    public double PressurePercent => Math.Clamp((pressure_msl - 950.0) / (1050.0 - 950.0), 0.0, 1.0); // 950 hPA low, 1050 hPA high
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
}

public class HourlyForecast // for UI
{
    public string Time { get; set; }
    public string WindSpeed { get; set; }
    public string Temperature { get; set; }
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

    public string TodayTemperatureRange => $"{temperature_2m_min[0]:0}°/{temperature_2m_max[0]:0}°";
    public string TomorrowTemperatureRange => $"{temperature_2m_min[1]:0}°/{temperature_2m_max[1]:0}";
    public string DayThreeTemperatureRange => $"{temperature_2m_min[2]:0}°/{temperature_2m_max[2]:0}°";

    public string DayThreeName => DateTime.Parse(time[2]).ToString("dddd");

}