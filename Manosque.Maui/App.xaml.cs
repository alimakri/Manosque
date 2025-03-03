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
            MainPage = new AppShell();
        }
    }
}
