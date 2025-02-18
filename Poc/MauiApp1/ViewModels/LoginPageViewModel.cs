using System;
using System.Windows.Input;
using System.Threading.Tasks;
using ComlineApp.Manager;
using ComlineApp.Services;
using Microsoft.Maui.Controls;

namespace MauiApp1.ViewModels
{
    public class LoginPageViewModel : BindableObject
    {
        private readonly IServiceApi MonServiceApi;
        private string nom;
        private string password;
        private Color connectButtonColor;

        public string Nom
        {
            get => Nom;
            set
            {
                Nom = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => Password;
            set
            {
                Password = value;
                OnPropertyChanged();
            }
        }

        public Color ConnectButtonColor
        {
            get => ConnectButtonColor;
            set
            {
                ConnectButtonColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand SeConnecterCommand { get; }
        public ICommand OnForgotPasswordTapped { get; }

        public LoginPageViewModel(IServiceApi serviceApi)
        {
            MonServiceApi = serviceApi;
            SeConnecterCommand = new Command(async () => await SeConnecter());
            OnForgotPasswordTapped = new Command(async () => await ForgotPassword());
            ConnectButtonColor = Colors.Purple; // Couleur initiale
        }

        private async Task SeConnecter()
        {
            ConnectButtonColor = Colors.Gray;

            ComlineData command = new ComlineData
            {
                Prompt = $@"Get-Personne -Reference ""{Nom}"" -Password ""{Password}"" -Return ""OnlyCount"""
            };
            MonServiceApi.Execute();

            try
            {
                if (true || int.TryParse(command.Results.Tables["Personne"].Rows[0][0].ToString(), out int n) && n == 1)
                {
                    MonServiceApi.User = Nom;
                    ConnectButtonColor = Colors.Green;
                    await Shell.Current.GoToAsync("//MainPage"); // Redirige vers la page principale
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion : {ex.Message}");
            }

            ConnectButtonColor = Colors.Red;
        }

        private async Task ForgotPassword()
        {
            await Application.Current.MainPage.DisplayAlert("Mot de passe oublié", "Veuillez contacter l'administrateur.", "OK");
        }
    }
}
