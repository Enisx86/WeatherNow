using System.Threading.Tasks;
using WeatherNow.Models;
using WeatherNow.ViewModels;

namespace WeatherNow;

public partial class SearchPage : ContentPage
{
	private bool _isPressing = false;

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
		if (card.BindingContext is not CityItem city) return;

		vm.SelectCityCommand.Execute(city);
    }

    private async void Favorite_Clicked(object sender, EventArgs e)
    {
        ImageButton fav = (ImageButton)sender;

        if (BindingContext is not SearchPageViewModel vm) return;
        if (fav.BindingContext is not CityItem city) return;

        await fav.ScaleTo(0.95, 50, Easing.CubicOut);
        await fav.ScaleTo(1.0, 50, Easing.CubicIn);

        vm.FavoriteCommand.Execute(city);
    }
}