using ComlineApp.Manager;
using ComlineApp.Services;
using MauiApp1.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(IServiceApi serviceApi)
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel(serviceApi);
        }

    }

}
