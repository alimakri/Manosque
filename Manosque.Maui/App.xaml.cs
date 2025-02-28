using ComLineData;
using ComlineServices;

namespace Manosque.Maui
{
    public partial class App : Application
    {
        public static readonly ServiceApi MonServiceAPi = new (new ComlineData());
        public static string User = string.Empty;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
