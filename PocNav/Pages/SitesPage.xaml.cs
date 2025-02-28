namespace PocNav.Pages;

public partial class SitesPage : ContentPage
{
	public SitesPage()
	{
		InitializeComponent();
	}

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TachePage");
    }
}