using Microsoft.Maui.Controls;
using System;
using AndroidMaui;


namespace AndroidMaui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadPersonne();
        }

        private void LoadPersonne()
        {
            Personne personne = StorageService.GetPersonne();
            long size = StorageService.GetStorageSize(); // Récupère la taille
            string maxSize = StorageService.GetMaxStorageSize(); // Taille max

            UserInfoLabel.Text = $"{personne.ToString()}\nTaille du stockage : {size} octets\nTaille max : {maxSize}";
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            string nom = NameEntry.Text;
            string prenom = SurnameEntry.Text;

            if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom))
            {
                Personne personne = new Personne(nom, prenom);
                StorageService.SavePersonne(personne);
                UserInfoLabel.Text = personne.ToString();
            }
        }
    }
}
