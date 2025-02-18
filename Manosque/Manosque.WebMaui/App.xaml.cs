using ComlineApp.Manager;
using ComlineApp.Services;

namespace Manosque.WebMaui
{
    public partial class App : Application
    {
        public CoreComline? MainComline;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();


        }
    }
}
