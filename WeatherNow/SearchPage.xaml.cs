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

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not SearchPageViewModel vm) return;

        vm.RefreshFavoritesCommand.Execute(null);
    }

    private async void OnCardTapped(object sender, TappedEventArgs e)
    {
		Border card = (Border)sender;

		await card.ScaleTo(0.85, 50, Easing.CubicOut);
        await card.ScaleTo(1, 50, Easing.CubicIn);

		if (BindingContext is not SearchPageViewModel vm) return;
		if (card.BindingContext is not GeocodingResult city) return;

		vm.SelectCityCommand.Execute(city);
    }

    private async void Favorite_Clicked(object sender, EventArgs e)
    {
        ImageButton fav = (ImageButton)sender;

        if (BindingContext is not SearchPageViewModel vm) return;
        if (fav.BindingContext is not GeocodingResult city) return;

        await fav.ScaleTo(0.85, 150, Easing.CubicOut);
        await fav.ScaleTo(1.0, 150, Easing.CubicIn);

        vm.FavoriteCommand.Execute(city);
    }
}