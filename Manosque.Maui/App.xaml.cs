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

            //// ServiceSystem
            //ServiceSystem.Options.Add("Service", "System");
            //ServiceSystem.Options.Add("DisplayMode", "Normal");

            //// ServiceData
            //ServiceData.ConnectionString = "Server=.\\SQLEXPRESS;Database=ManosqueBD;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            // ServiceApi
            ServiceApi.Url = "https://makrisoft.net/"; // localhost:7298 - makrisoft.net

            MainPage = new AppShell();
        }
    }
}
