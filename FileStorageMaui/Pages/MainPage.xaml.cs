using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorageMaui.Services;

namespace FileStorageMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string nom = NameEntry.Text;
            string prenom = SurnameEntry.Text;

            if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom))
            {
                await FileService.SaveUserDataAsync(nom, prenom);
                UserInfoLabel.Text = $"Données enregistrées : {nom} {prenom}";
            }
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            List<Personne> personnes = await FileService.LoadAllUserDataAsync();

            if (personnes.Count == 0)
            {
                UserInfoLabel.Text = "Aucune donnée trouvée.";
                return;
            }

            UserInfoLabel.Text = "Personnes enregistrées :\n" + string.Join("\n", personnes);
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            FileService.DeleteUserData();
            UserInfoLabel.Text = "Fichier supprimé.";
        }
    }
}
