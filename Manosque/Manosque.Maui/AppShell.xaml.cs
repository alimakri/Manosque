using Manosque.Maui.Pages;

namespace Manosque.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("login", typeof(LoginPage));
        }
    }
}
