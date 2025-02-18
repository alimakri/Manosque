
using ComlineApp.Services;
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class SitesPage : ContentPage, IQueryAttributable
    {
        public SitesPage()
        {
            InitializeComponent();

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("user"))
            {
                string user = query["user"] as string;

                var Sites = new ObservableCollection<Site>();

                var today = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                App.MonServiceAPi.Command.Prompt = $@"Get-Execution -Personne ""{user}"" -DateDebut ""{today}"" -Filter ""ListeSites""";
                App.MonServiceAPi.Command.Results.Tables.Clear();
                App.MonServiceAPi.Execute();
                DataTable tableSites;
                try
                {
                    tableSites = App.MonServiceAPi.Command.Results.Tables[0];
                }
                catch (Exception ex)
                {
                    return;
                }
                if (tableSites.TableName == "Error")
                {
                    //ShowAlert((string)tableSites.Rows[0]["Libelle"]);

                }
                else
                {
                    var id = default(Guid);
                    foreach (DataRow row in tableSites.Rows)
                    {
                        if (Guid.TryParse(row["Emplacement"].ToString(), out id))
                            Sites.Add(new Site { Id = id, Tache = (string)row["Tache"], Libelle = (string)row["Reference"], Statut = (long)row["Statut"] });
                    }
                }
            }
        }

        private async void OnRetour(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//root/login");

        }
    }

}
