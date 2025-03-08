using ComLineCommon;
using ComLineData;
using ComlineServices;

namespace Manosque.Maui
{
    public partial class App : Application
    {
        public static readonly ServiceApi MonServiceApi = new (new ComlineData());
        public static string? User = "";

        public App()
        {
            InitializeComponent();

            // ServiceApi
            ServiceApi.Url = "https://makrisoft.net/"; // localhost:7298 - makrisoft.net

            MainPage = new AppShell();
        }
    }
}
