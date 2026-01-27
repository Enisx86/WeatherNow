using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

using WeatherNow.Services;
using WeatherNow.Models;

namespace WeatherNow.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly IWeatherService _weatherService; // MauiProgram.cs

    public MainViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
     => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
