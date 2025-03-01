﻿
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Manosque.Maui.Pages
{
    public partial class SitesPage : ContentPage, IQueryAttributable
    {
        private string? User;
        private Guid? UserId;
        private string? Execution;
        private Guid? ExecutionId;

        public SitesPage()
        {
            InitializeComponent();
            myDatePicker.PropertyChanged += DatePicker_PropertyChanged;
        }
        private void DatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DatePicker.Date))
            {
                ExecuteApi();
            }
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            // Init
            User = query.TryGetValue("user", out object? value) ? value as string : null;
            Execution = query.TryGetValue("execution", out object? value2) ? value2 as string : "NULL";
            if (query.ContainsKey("date") && DateTime.TryParse((string)query["date"], out DateTime value3)) myDatePicker.Date = value3;
            ExecuteApi();
        }
        public void ExecuteApi()
        {
            // Stay here in SitesPage
            App.MonServiceAPi.Command.Prompts = [
                "Set-Option -Service Data;",
                "Get-Execution -Reference NULL".Replace("NULL", $@"""{Execution}"""),
                $@"Get-Execution -Execution ^ -Personne ""{User}"" -Mode ""Debug"" -DateDebut ""{myDatePicker.Date}"" -Filter ""ListeSites"""];
            App.MonServiceAPi.Command.Reset();
            App.MonServiceAPi.Execute();

            var tableList = App.MonServiceAPi.Command.Results.TableList;
            if (tableList.Contains("Error"))
            {

            }
            else if (tableList.Contains("Execution"))
            {
                ObservableCollection<Site> Sites = [];
                var id = default(Guid);
                var list = App.MonServiceAPi.Command.Results.Tables["Execution"]?.Rows.Cast<DataRow>().Where(row => Guid.TryParse(row["Emplacement"].ToString(), out id));
                if (list != null)
                    foreach (var row in list)
                    {
                        Sites.Add(new Site
                        {
                            Id = id,
                            Tache = row["Tache"] as string,
                            Libelle = (string)row["Reference"],
                            Statut = (long)row["Statut"],
                            Date = DateOnly.FromDateTime(myDatePicker.Date)
                        });
                    }

                this.SitesCollectionView.ItemsSource = Sites;
            }
        }

        private async void OnRetour(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");

        }
    }

}
