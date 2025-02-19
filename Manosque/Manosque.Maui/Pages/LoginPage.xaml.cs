using ComlineApp.Manager;
using ComlineApp.Services;

namespace Manosque.Maui.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnSites(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//root/pages/sites?user={App.User}");

            //var command = App.MonServiceAPi.Command;
            //command.Prompt = $@"Get-Personne -Reference ""{UserNom.Text}"" -Password ""{UserPassword.Text}"" -Return ""OnlyCount""";
            //App.MonServiceAPi.Execute(EnumDataSimulation.Password);

            //try
            //{
            //    var table = command.Results.Tables["Personne"];
            //    int? n = table?.Rows[0][0] as int?;
            //    if (n == 1)
            //    {
            //        App.User = UserNom.Text;
            //        await Shell.Current.GoToAsync($"//root/pages/sites?user={App.User}");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Erreur de connexion : {ex.Message}");
            //}

        }
    }

}
