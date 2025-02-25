
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class SitesPage : ContentPage, IQueryAttributable
    {
        private List<string> PromptsSites;
        private string? User = "";
        private string? Execution = "";
        private DateOnly Today = new DateOnly(2025, 2, 17);

        public SitesPage()
        {
            InitializeComponent();
            PromptsSites = ["Get-Execution -Reference NULL", $@"Get-Execution -Execution ^ -Personne ""{User}"" -DateDebut ""{Today}"" -Filter ""ListeSites"""];
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("execution"))
            {
                Execution = query["execution"] as string;
                PromptsSites[0].Replace("NULL", $@"""{Execution}""");
            }
            if (query.ContainsKey("user"))
            {
                User = query["user"] as string;
            }
            {
                User = query["user"] as string;

                var Sites = new ObservableCollection<Site>();

                App.MonServiceAPi.Command.Results.Tables.Clear();
                App.MonServiceAPi.Execute(PromptsSites);

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
                            Sites.Add(new Site { Id = id, Tache = row["Tache"] as string, Libelle = (string)row["Reference"], Statut = (long)row["Statut"] });
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
