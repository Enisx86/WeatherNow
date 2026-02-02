using WeatherNow.Models;
using WeatherNow.ViewModels;

namespace WeatherNow;

public partial class FavoritesPage : ContentPage
{
	public FavoritesPage(FavoritesPageViewModel vm)
	{
        BindingContext = vm;
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        if (BindingContext is not FavoritesPageViewModel vm) return;

        vm.LoadFavoritesCommand.Execute(null);
    }

    public async void Favorite_Clicked(object sender, EventArgs e)
    {
        ImageButton fav = (ImageButton)sender;

        if (BindingContext is not FavoritesPageViewModel vm) return;
        if (fav.BindingContext is not GeocodingResult city) return;

        Element parent = fav.Parent; // we need to track the parent Border (card), so we can shrink it
        while (parent != null && parent is not Border)
        {
            parent = parent.Parent;
        }

        if (parent is not Border card) return;

        // button anims
        await fav.ScaleTo(0.85, 50, Easing.CubicOut);
        await fav.ScaleTo(1.0, 50, Easing.CubicIn);

        // card shrinking
        await card.ScaleTo(0, 200, Easing.CubicOut);

        vm.RemoveFavoriteCommand.Execute(city);
    }

    public async void OnCardTapped(object sender, TappedEventArgs e)
    {
        Border card = (Border)sender;

        await card.ScaleTo(0.85, 150, Easing.CubicOut);
        await card.ScaleTo(1.0, 150, Easing.CubicIn);

        if (BindingContext is not FavoritesPageViewModel vm) return;
        if (card.BindingContext is not GeocodingResult city) return;

        vm.SelectCityCommand.Execute(city);
    }
}