using Microsoft.Extensions.Logging;
using WeatherNow.Services;
using WeatherNow.ViewModels;

namespace WeatherNow
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IWeatherService, WeatherService>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<SearchPageViewModel>();
            builder.Services.AddSingleton<MainPage>();


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
