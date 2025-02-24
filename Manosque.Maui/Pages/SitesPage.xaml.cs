
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class SitesPage : ContentPage, IQueryAttributable
    {
        private string PromptSites { get { return $@"Get-Execution -Personne ""{User}"" -DateDebut ""{Today}"" -Filter ""ListeSites"""; } }
        private string? User = "";
        private DateOnly Today = new DateOnly(2025, 2, 17);

        public SitesPage()
        {
            InitializeComponent();

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("user"))
            {
                User = query["user"] as string;

                var Sites = new ObservableCollection<Site>();

                App.MonServiceAPi.Command.Prompt = PromptSites;
                App.MonServiceAPi.Command.Results.Tables.Clear();
                App.MonServiceAPi.Execute();

                var tableList = App.MonServiceAPi.Command.Results.TableList;
                if (tableList.Contains("Error"))
                {

                }
                else if (tableList.Contains("Execution"))
                {
                    DataTable tableSites;
                    tableSites = App.MonServiceAPi.Command.Results.Tables["Execution"];
                    var id = default(Guid);
                    foreach (DataRow row in tableSites.Rows)
                    {
                        if (Guid.TryParse(row["Emplacement"].ToString(), out id))
                            Sites.Add(new Site { Id = id, Tache = (string)row["Tache"], Libelle = (string)row["Reference"], Statut = (long)row["Statut"] });
                    }
                    this.SitesCollectionView.ItemsSource = Sites;
                }
            }
        }

        private async void OnRetour(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//root/login");

        }
    }

}
