using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using WeatherNow.Models;
using WeatherNow.Services;

namespace WeatherNow.ViewModels;


public class MainViewModel : INotifyPropertyChanged, IQueryAttributable
{
    private readonly IWeatherService _weatherService; // MauiProgram.cs

    public ObservableCollection<HourlyForecast> HourlyForecasts { get; set; } = new();

    private GeocodingResult _city;
    public GeocodingResult City
    {
        get => _city;
        set
        {
            _city = value;
            OnPropertyChanged(nameof(City));

            //if (_city != null) // city info loaded, we can now get weather
                //_ = LoadWeather();
                
        }
    }

    private WeatherResponse? _weather;
    public WeatherResponse? Weather
    {
        get => _weather;
        set
        {
            _weather = value;
            OnPropertyChanged(nameof(Weather));

            if (_weather is not null)
            {
                ParseHourlyForecasts();

                HumidityMeterColor = GetHumidityMeterColor(Weather?.current.HumidityPercent);
                WindMeterColor = GetWindMeterColor(Weather?.current.WindSpeedPercent);
                PressureMeterColor = GetPressureMeterColor(Weather?.current.PressurePercent);
            }
        }
    }

    private Color _humidityMeterColor = Colors.LightGray;
    public Color HumidityMeterColor
    {
        get => _humidityMeterColor;
        set { _humidityMeterColor = value; OnPropertyChanged(nameof(HumidityMeterColor)); }
    }

    private Color _windMeterColor = Colors.LightGray;
    public Color WindMeterColor
    {
        get => _windMeterColor;
        set { _windMeterColor = value; OnPropertyChanged(nameof(WindMeterColor)); }
    }

    private Color _pressureMeterColor = Colors.LightGray;
    public Color PressureMeterColor
    {
        get => _pressureMeterColor;
        set { _pressureMeterColor = value; OnPropertyChanged(nameof(PressureMeterColor)); }
    }

    public bool IsValidCity => City != null; // only show data if city is visible
    public bool IsNotValidCity => !IsValidCity; 

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;


        // line 20 uncomment lines when finished with mocking
        string fakeCityJson = @"{""id"":3186573,""name"":""Zenica"",""latitude"":44.20169,""longitude"":17.90397,""elevation"":325,""timezone"":""Europe/Sarajevo"",""country"":""Bosnia and Herzegovina"",""country_code"":""BA"",""CountryEmoji"":""\uD83C\uDDE7\uD83C\uDDE6"",""Coordinates"":{""Timestamp"":""2026-01-27T15:17:26.0580999+00:00"",""Latitude"":44.20169,""Longitude"":17.90397,""Altitude"":325,""Accuracy"":null,""VerticalAccuracy"":null,""ReducedAccuracy"":false,""Speed"":null,""Course"":null,""IsFromMockProvider"":false,""AltitudeReferenceSystem"":0}}";
        string fakeWeatherJson = @"{""latitude"":44.202515,""longitude"":17.90799,""generationtime_ms"":8.883595,""utc_offset_seconds"":3600,""timezone"":""Europe/Sarajevo"",""timezone_abbreviation"":""GMT\u002B1"",""elevation"":326,""current_units"":{""time"":""iso8601"",""interval"":""seconds"",""temperature_2m"":""\u00B0C"",""relative_humidity_2m"":""%"",""surface_pressure"":""hPa"",""wind_speed_10m"":""km/h"",""weather_code"":""wmo code""},""current"":{""time"":""2026-01-27T16:15"",""interval"":900,""pressure_msl"":1024,""temperature_2m"":9,""apparent_temperature"":6.4,""relative_humidity_2m"":53,""surface_pressure"":966.9,""wind_speed_10m"":3.2,""weather_code"":0,""WeatherDescription"":""Clear"",""TemperatureText"":""9\u00B0"",""ApparentTemperatureText"":""6.4\u00B0""},""hourly_units"":{""time"":""iso8601"",""temperature_2m"":""\u00B0C"",""wind_speed_10m"":""km/h"",""temperature_80m"":""\u00B0C"",""weather_code"":""wmo code""},""hourly"":{""time"":[""2026-01-27T16:00"",""2026-01-27T17:00"",""2026-01-27T18:00"",""2026-01-27T19:00"",""2026-01-27T20:00"",""2026-01-27T21:00"",""2026-01-27T22:00"",""2026-01-27T23:00"",""2026-01-28T00:00"",""2026-01-28T01:00"",""2026-01-28T02:00"",""2026-01-28T03:00"",""2026-01-28T04:00"",""2026-01-28T05:00"",""2026-01-28T06:00"",""2026-01-28T07:00"",""2026-01-28T08:00"",""2026-01-28T09:00"",""2026-01-28T10:00"",""2026-01-28T11:00"",""2026-01-28T12:00"",""2026-01-28T13:00"",""2026-01-28T14:00"",""2026-01-28T15:00""],""temperature_2m"":[9.4,7.5,5.2,4,3.3,2.8,2.4,2.2,2.5,2.8,3,3.3,3.7,4.2,4.4,4.7,5.1,6.1,7.1,7.7,8.8,10,10.4,10],""wind_speed_10m"":[3.2,3.6,3.6,5,4.7,2.5,2.9,6.5,5.8,5.4,2.9,4.7,6.1,6.8,7.2,6.5,5,7.9,10.1,11.5,13,16.2,11.9,14.8],""temperature_80m"":[8.3,8.1,7.3,6.3,5.8,4.8,4.4,4.9,5.5,5.3,4.9,4.8,5.4,5.6,6.1,6,6.2,6.7,7.3,7.7,8.2,9,9.5,9.5],""weather_code"":[0,0,0,0,0,0,3,3,3,0,3,3,3,3,3,3,3,3,3,3,3,3,3,3]},""daily_units"":{""time"":""iso8601"",""uv_index_max"":"""",""sunset"":""iso8601"",""weather_code"":""wmo code"",""temperature_2m_max"":""\u00B0C"",""temperature_2m_min"":""\u00B0C""},""daily"":{""time"":[""2026-01-27"",""2026-01-28"",""2026-01-29""],""uv_index_max"":[2.4,1.2,1.75],""sunset"":[""2026-01-27T16:50"",""2026-01-28T16:52"",""2026-01-29T16:53""],""weather_code"":[3,51,61],""temperature_2m_max"":[9.7,10.4,7.3],""temperature_2m_min"":[1.6,2.5,2.3],""AverageTemperatureMax"":10.4,""AverageTemperatureMin"":7.3,""AverageTemperatureRange"":""7.3\u00B0/10.4\u00B0""}}";

        City = JsonSerializer.Deserialize<GeocodingResult>(fakeCityJson);
        Weather = JsonSerializer.Deserialize<WeatherResponse>(fakeWeatherJson);
    }


    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-10.0
        // QueryProperty doesn't work for some reason, IQueryAttributable is used instead
        // certified .NET MAUI moment :)

        // issue: navigation doesn't update values for some reason, so you have to update it yourself...

        if (query.TryGetValue("City", out object cityModel) && cityModel is GeocodingResult)
        {
            City = (GeocodingResult) cityModel;

            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(IsValidCity));
            OnPropertyChanged(nameof(IsNotValidCity));
        }
    }
    
    // from light blue to red depending on wind speed
    public Color GetHumidityMeterColor(double? humidityPercent)
    {
        if (humidityPercent is null) return Colors.LightGray;

        return humidityPercent switch
        {
            < 0.30 => Colors.MediumAquamarine,
            <= 0.55 => Colors.CornflowerBlue,
            <= 0.75 => Colors.DodgerBlue,
            _ => Colors.RoyalBlue
        };
    }

    public Color GetWindMeterColor(double? windSpeedDangerPercent)
    {
        if (windSpeedDangerPercent is null) return Colors.LightGray;

        return windSpeedDangerPercent switch
        {
            < 0.15 => Colors.MediumSeaGreen,
            <= 0.40 => Colors.SeaGreen,
            <= 0.75 => Colors.Goldenrod,
            _ => Colors.Red
        };
    }

    public Color GetPressureMeterColor(double? pressurePercent)
    {
        if (pressurePercent is null) return Colors.LightGray;

        return pressurePercent switch
        {
            < 0.30 => Colors.Red,
            <= 0.75 => Colors.MediumSeaGreen,
            _ => Colors.Orange
        };
    }

    private void ParseHourlyForecasts()
    {
        if (Weather is null) return;
        
        HourlyForecasts.Clear();

        int i = 0;
        foreach (string time in Weather.hourly.time) // time is an array of dates by API
        {
            string clockTime = DateTime.Parse(time).ToString("HH:mm"); // 24h clock
            string temperature = $"{Weather.hourly.temperature_2m[i]:0}°";
            string windSpeed = $"{Weather.hourly.wind_speed_10m[i]:0} km/h";

            HourlyForecasts.Add(new HourlyForecast { Time = clockTime, Temperature = temperature, WindSpeed = windSpeed });
            i++;
        }
    }

    private async Task LoadWeather()
    {
        Weather = await _weatherService.GetWeatherAsync(City);
    }

    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

}
