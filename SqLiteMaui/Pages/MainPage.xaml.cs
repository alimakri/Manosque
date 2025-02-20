using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqLiteMaui.Models;
using SqLiteMaui.Services;

namespace SqLiteMaui.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly Database _Database = new Database();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string nom = NameEntry.Text;
            string prenom = SurnameEntry.Text;
            string rue = StreetEntry.Text;
            string ville = CityEntry.Text;
            string codePostal = PostalEntry.Text;
            string entrepriseNom = CompanyEntry.Text;
            string secteur = SectorEntry.Text;

            if (!string.IsNullOrEmpty(nom) && !string.IsNullOrEmpty(prenom))
            {
                Adresse adresse = new Adresse { Rue = rue, Ville = ville, CodePostal = codePostal };
                await _Database.SaveAdresseAsync(adresse);

                Entreprise entreprise = new Entreprise { Nom = entrepriseNom, Secteur = secteur };
                await _Database.SaveEntrepriseAsync(entreprise);

                Personne personne = new Personne(nom, prenom, adresse.Id, entreprise.Id);
                await _Database.SavePersonneAsync(personne);

                UserInfoLabel.Text = $"Ajouté : {nom} {prenom}\n{rue}, {ville} ({codePostal})\n{entrepriseNom} - {secteur}";
            }
        }

        private async void OnLoadClicked(object sender, EventArgs e)
        {
            List<Personne> personnes = await _Database.GetPersonnesAsync();
            UserInfoLabel.Text = personnes.Any()
                ? string.Join("\n", personnes.Select(p => p.ToString()))
                : "Aucune donnée en base.";
        }

        private async void OnClearClicked(object sender, EventArgs e)
        {
            await _Database.ClearDatabase();
            UserInfoLabel.Text = "Base de données vidée.";
        }
    }
}
