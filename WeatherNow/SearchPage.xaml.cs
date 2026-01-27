using WeatherNow.ViewModels;

namespace WeatherNow;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}