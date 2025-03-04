using ComLineCommon;
using ComlineServices;

namespace Manosque.Maui.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly string PromptLogin;
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void OnSites(object sender, EventArgs e)
        {
            // Connect
            ServiceApi.Command = new ComLineData.ComlineData();
            App.MonServiceApi.ConnectApi(new UserLogin { Username = UserNom.Text, Password = UserPassword.Text });

            try
            {
                var table = ServiceApi.Command.Results.Tables["Info"];
                App.User = (string?) table?.Rows[0][1];
                if (App.User != null)
                {
                    await Shell.Current.GoToAsync($"//sites?user={App.User}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion : {ex.Message}");
            }

        }
    }

}
