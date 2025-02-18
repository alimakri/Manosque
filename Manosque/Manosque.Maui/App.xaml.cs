using ComlineApp.Manager;
using ComlineApp.Services;
using Manosque.Maui.Pages;

namespace Manosque.Maui
{
    public partial class App : Application
    {
        public static ServiceApi MonServiceAPi = new (new ComlineData());
        public static string User = string.Empty;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
