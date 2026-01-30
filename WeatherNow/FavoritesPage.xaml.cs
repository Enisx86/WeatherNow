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
}