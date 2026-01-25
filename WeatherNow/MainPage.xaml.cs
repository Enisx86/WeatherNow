using WeatherNow.ViewModels;

namespace WeatherNow
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        public MainPage()
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}