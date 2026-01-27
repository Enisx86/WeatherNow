using System.Threading.Tasks;
using WeatherNow.Models;
using WeatherNow.ViewModels;

namespace WeatherNow;

public partial class SearchPage : ContentPage
{
	public SearchPage(SearchPageViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

    private async void OnCardTapped(object sender, TappedEventArgs e)
    {
		Border card = (Border)sender;

		await card.ScaleTo(0.95, 50, Easing.CubicOut);
        await card.ScaleTo(1.0, 50, Easing.CubicIn);

		if (BindingContext is not SearchPageViewModel vm) return;
		if (card.BindingContext is not GeocodingResult city) return;

		vm.SelectCityCommand.Execute(city);
    }
}