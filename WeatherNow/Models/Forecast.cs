using System;

namespace WeatherNow.Models;

// primarily for UI presentation btw

public record TodayForecast
{
    public WeatherCode WeatherCode { get; set; }
    public string WeatherIconSource => WeatherCode.ToIconSource();
    public string WeatherDescription => WeatherCode.ToDescription();

    public float TemperatureMin { get; set; }
    public float TemperatureMax { get; set; }

    public float Windspeed { get; set; } // km/h
    public double WindspeedPercent => Math.Clamp(Windspeed / 120.0, 0, 1.0); // from 0.0 to 1.0 for the UI meter
    public Color WindspeedMeterColor => Windspeed switch
    {
        < 30 => Colors.CadetBlue,
        < 60 => Colors.MediumSeaGreen,
        < 90 => Colors.Orange,
        _ => Colors.Red
    };


    public int Humidity { get; set; }
    public double HumidityPercent => Humidity / 100.0;
    public Color HumidityMeterColor => Humidity switch
    {
        < 30 => Colors.MediumAquamarine,
        <= 55 => Colors.CornflowerBlue,
        <= 75 => Colors.DodgerBlue,
        _ => Colors.RoyalBlue
    };


    public float Pressure { get; set; }
    public string PressureDescription => Pressure switch
    {
        < 980 => "Storm warning",
        < 1000 => "Rain likely",
        <= 1013 => "Cloudy",
        <= 1025 => "Clear or sunny",
        _ => "Dry"
    };
    public Color PressureTextColor => Pressure switch
    {
        < 980 => Colors.DarkRed,
        < 1000 => Colors.CadetBlue,
        <= 1013 => Colors.MediumSeaGreen,
        <= 1025 => Colors.Goldenrod,
        _ => Colors.SaddleBrown
    };

    public float Visibility { get; set; } // meters
    public float VisibilityKilometers => Visibility / 1000f;
    public string VisibilityDescription => Visibility switch
    {
        < 100 => "Dense fog",
        < 1000 => "Poor",
        < 4000 => "Moderate",
        < 10000 => "Clear",
        _ => "Perfect"
    };
    public Color VisibilityTextColor => Visibility switch
    {
        < 100 => Colors.DarkRed,
        < 1000 => Colors.Red,
        < 4000 => Colors.Goldenrod,
        < 10000 => Colors.CadetBlue,
        _ => Colors.MediumSeaGreen
    };

    public float UVIndexMax { get; set; }
    public float UVIndexMaxPercent => Math.Clamp(UVIndexMax / 12f, 0, 1f); // from 0.0 to 1.0 for the UI meter
    public Color UVIndexMeterColor => UVIndexMax switch
    {
        <= 2 => Colors.MediumSeaGreen,
        <= 5 => Colors.Goldenrod,
        <= 7 => Colors.Orange,
        < 10 => Colors.Red,
        _ => Colors.Purple
    };

    public float CurrentTemperature { get; set; }
    public float ApparentTemperature { get; set; }
}

public record DailyForecast
{
    public WeatherCode WeatherCode { get; set; }
    public string WeatherIconSource => WeatherCode.ToIconSource();

    public float TemperatureMin { get; set; }
    public float TemperatureMax { get; set; }

    public DateTime Time { get; set; }
    public string DayName // it's either Today, Tomorrow or (name of day after tomorrow)
    {
        get
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            if (Time.Date == today) return "Today";
            else if (Time.Date == tomorrow) return "Tomorrow";
            else return Time.ToString("dddd");
        }
    }
}

public record HourlyForecast
{
    public WeatherCode WeatherCode { get; set; }
    public string WeatherIconSource => WeatherCode.ToIconSource();

    public DateTime Time { get; set; }
    public float Windspeed { get; set; }
    public float Temperature { get; set; }

    public string TimeText => Time.ToString("HH:mm"); // 24h clock
}