using System;

namespace WeatherNow.Controls;

public partial class FloatingNavbar : ContentView
{
    public static readonly BindableProperty ActiveTabProperty =
    BindableProperty.Create(nameof(ActiveTab), typeof(string), typeof(FloatingNavbar), "",
    propertyChanged: (bindable, oldValue, newValue) => {
        if (bindable is not FloatingNavbar navbar) return;
        if (newValue is not string tab) return;

        navbar.HomeButton.BackgroundColor = Colors.Transparent;
        navbar.SearchButton.BackgroundColor = Colors.Transparent;
        navbar.FavoritesButton.BackgroundColor = Colors.Transparent;

        if (tab == "Home") navbar.HomeButton.BackgroundColor = Colors.LightBlue;
        if (tab == "Search") navbar.SearchButton.BackgroundColor = Colors.LightBlue;
        if (tab == "Favorites") navbar.FavoritesButton.Background = Colors.LightBlue;
    });

    public string ActiveTab
    {
        get => (string)GetValue(ActiveTabProperty);
        set => SetValue(ActiveTabProperty, value); 
    }

    public FloatingNavbar()
	{
        InitializeComponent();
    }

    private async void Home_Clicked(object sender, EventArgs e)
    {
		if (ActiveTab == "Home") return; // don't refresh
		if (sender is not ImageButton button) return;

		await Shell.Current.GoToAsync("//MainPage");
    }

    private async void Search_Clicked(object sender, EventArgs e)
    {
        if (ActiveTab == "Search") return; // don't refresh
        if (sender is not ImageButton button) return;

        await Shell.Current.GoToAsync("//SearchPage");
    }

    private async void Favorites_Clicked(object sender, EventArgs e)
    {
        if (ActiveTab == "Favorites") return; // don't refresh
        if (sender is not ImageButton button) return;

        await Shell.Current.GoToAsync("//FavoritesPage");
    }
}